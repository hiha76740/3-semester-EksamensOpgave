using System.Data;
using System.Text.Json;
using AuthService.ApplicationLib.Repositories;
using AuthService.DomainLib.Entities;
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
/// <param name="outbox"></param>
/// <param name="unitOfWork"></param>
/// <param name="logger"></param>
public class DeleteUserHandler(
    IAuthUserRepository authUserRepo,
    IOutboxRepository outbox,
    IUnitOfWork unitOfWork,
    ILogger<DeleteUserHandler> logger
    ) : IDeleteUserHandler
{
    async Task IDeleteUserHandler.HandleAsync(DeleteAuthUserCommand command)
    {
        try
        {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Beginning deletion of user {UserId}", command.Id);

            unitOfWork.BeginTransaction(IsolationLevel.Serializable);

            var authUserId = new AuthUserId(command.Id);

            var authUser = await authUserRepo.GetAuthUserByIdAsync(authUserId)
                ?? throw new NotFoundException("User was not found");

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Deleting auth user from auth database");

            authUserRepo.Delete(authUser);

            authUserRepo.SetOriginalRowVersion(authUser, command.RowVersion);

            var integrationEvent = new UserDeletionIntegrationEvent(
                Guid.NewGuid(),
                "user-deletion",
                authUser.Id.Value
                );

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
                logger.LogInformation("User {Id} was deleted from auth database successfully", authUser.Id.Value);

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
