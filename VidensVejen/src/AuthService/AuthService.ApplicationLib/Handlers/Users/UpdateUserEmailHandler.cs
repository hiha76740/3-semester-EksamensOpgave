using System.Data;
using System.Text.Json;
using AuthService.ApplicationLib.Repositories;
using AuthService.DomainLib.Entities;
using AuthService.FacadeLib.Commands.DTOs;
using AuthService.FacadeLib.Commands.Interfaces;
using AuthService.FacadeLib.Queries.DTOs;
using Microsoft.Extensions.Logging;
using VidensVejen.BuildingBlocksLib.Messaging.Outbox;
using VidensVejen.BuildingBlocksLib.UnitOfWork;
using VidensVejen.ContractsLib.Events;
using VidensVejen.SharedKernelLib.Exceptions;

namespace AuthService.ApplicationLib.Handlers.Users;

/// <summary>
/// 
/// </summary>
public class UpdateUserEmailHandler(
    IAuthUserRepository authUserRepo,
    IOutboxRepository outbox,
    IUnitOfWork unitOfWork,
    ILogger<UpdateUserEmailHandler> logger
    ) : IUpdateUserEmailHandler
{
    async Task<AuthUserEmailDto> IUpdateUserEmailHandler.HandleAsync(UpdateAuthUserEmailCommand command)
    {
        try
        {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Beginning update of email for user {UserId}", command.Id);

            unitOfWork.BeginTransaction(IsolationLevel.Serializable);

            var authUserId = new AuthUserId(command.Id);

            var authUser = await authUserRepo.GetAuthUserByIdAsync(authUserId)
                ?? throw new NotFoundException("User was not found");


            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Updating auth user email in auth database");

            authUser.ChangeEmail(command.Email);

            authUser.SetStatusPending();

            var integrationEvent = new UserUpdateEmailIntegrationEvent(
                Guid.NewGuid(),
                "user-update-email",
                authUser.Id.Value,
                command.Email
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
                logger.LogInformation("User {Id} was updated successfully", authUser.Id.Value);

            return new AuthUserEmailDto(authUser.Email.Value);
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
