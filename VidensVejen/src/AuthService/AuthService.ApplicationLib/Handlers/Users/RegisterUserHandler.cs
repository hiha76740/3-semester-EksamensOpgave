using System.Data;
using System.Text.Json;
using AuthService.ApplicationLib.Authentication;
using AuthService.ApplicationLib.Repositories;
using AuthService.DomainLib.Entities;
using AuthService.DomainLib.ValueObjects;
using AuthService.FacadeLib.Commands.DTOs;
using AuthService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging;
using VidensVejen.BuildingBlocksLib.Messaging.Outbox;
using VidensVejen.BuildingBlocksLib.UnitOfWork;
using VidensVejen.ContractsLib.Events;
using VidensVejen.SharedKernelLib.Exceptions;

namespace AuthService.ApplicationLib.Handlers.Users;

/// <summary>
/// 
/// </summary>
/// <param name="authUserRepo"></param>
/// <param name="permissionRepo"></param>
/// <param name="outbox"></param>
/// <param name="passwordHashService"></param>
/// <param name="unitOfWork"></param>
/// <param name="logger"></param>
public class RegisterUserHandler(
    IAuthUserRepository authUserRepo,
    IPermissionRepository permissionRepo,
    IOutboxRepository outbox,
    IPasswordHashService passwordHashService,
    IUnitOfWork unitOfWork,
    ILogger<RegisterUserHandler> logger
    ) : IRegisterUserHandler
{
    async Task IRegisterUserHandler.HandleAsync(RegisterUserCommand command)
    {
        try
        {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Beginning to register user with name {Firstname} {Lastname}", command.Firstname, command.Surname);

            unitOfWork.BeginTransaction(IsolationLevel.Serializable);

            var email = new Email(command.Email);

            var usernameExist = await authUserRepo.EmailExistAsync(email);

            if (usernameExist == true)
                throw new InvalidOperationException("User already exsist");

            var accessPermissionList = new List<AccessPermission>();

            foreach (var permission in command.PermissionsIds)
            {
                var id = new AccessPermissionId(permission);
                var accessPermission = await permissionRepo.GetPermissionByIdAsync(id)
                    ?? throw new NotFoundException($"Permission Id {permission} was not found");

                accessPermissionList.Add(accessPermission);
            }

            var hashedPassword = passwordHashService.Hash(command.Password);

            var authUser = AuthUser.Create(command.Email, hashedPassword, accessPermissionList);

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("User created with id: {UserId}", authUser.Id.Value);


            await authUserRepo.RegisterAsync(authUser);

            var integrationEvent = new UserRegisteredIntegrationEvent(
                Guid.NewGuid(),
                "user-registered",
                authUser.Id.Value,
                command.Firstname,
                command.Surname,
                command.Email);


            var outboxMessage = new OutboxMessage
            {
                Id = integrationEvent.EventId,
                Type = integrationEvent.GetType().FullName!,
                Topic = integrationEvent.EventTopic,
                Content = JsonSerializer.Serialize(integrationEvent),
                CreatedAtUtc = DateTime.UtcNow
            };

            await outbox.AddAsync(outboxMessage);

            await unitOfWork.SaveChangesAsync();
            unitOfWork.Commit();

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("User {Id} saved in database successfully", authUser.Id.Value);
        }
        catch (Exception ex)
        {
            if (logger.IsEnabled(LogLevel.Error))
            {
                logger.LogError("Error {ErrorMessage}", ex.Message);

                if (ex.InnerException != null)
                    logger.LogError("InnerException Error {Error}", ex.InnerException.Message);
            }

            unitOfWork.Rollback();
            throw;
        }
    }
}
