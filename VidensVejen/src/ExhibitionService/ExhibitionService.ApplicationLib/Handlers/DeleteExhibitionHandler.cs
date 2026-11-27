using System.Data;
using System.Text.Json;
using ExhibitionService.ApplicationLib.Repositories;
using ExhibitionService.DomainLib.ValueObjects;
using ExhibitionService.FacadeLib.Commands.DTOs;
using ExhibitionService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging;
using VidensVejen.BuildingBlocksLib.Messaging.Outbox;
using VidensVejen.BuildingBlocksLib.UnitOfWork;
using VidensVejen.ContractsLib.Events;
using VidensVejen.SharedKernelLib.Exceptions;

namespace ExhibitionService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
/// <param name="exhibitionRepo"></param>
/// <param name="outbox"></param>
/// <param name="unitOfWork"></param>
/// <param name="logger"></param>
public class DeleteExhibitionHandler(
    IExhibitionRepository exhibitionRepo,
    IOutboxRepository outbox,
    IUnitOfWork unitOfWork,
    ILogger<DeleteExhibitionHandler> logger) : IDeleteExhibitionHandler
{
    async Task IDeleteExhibitionHandler.HandleAsync(DeleteExhibitionCommand command)
    {
        unitOfWork.BeginTransaction(IsolationLevel.Serializable);

        try
        {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Beginning deletion of exhibition {ExhibitionId}", command.ExhibitionId);

            var exhibitionId = new ExhibitionId(command.ExhibitionId);

            var exhibition = await exhibitionRepo.GetExhibitionById(exhibitionId)
                ?? throw new NotFoundException("Exhibition not found");

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Deleting exhibition from exhibition database");

            exhibitionRepo.Delete(exhibition);

            exhibitionRepo.SetOriginalRowVersion(exhibition, command.RowVersion);

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Exhibition {Id} was successfully deleted from exhibition database", exhibition.Id.Value);

            var integrationEvent = new QuizDeletionIntegrationEvent(
                Guid.NewGuid(),
                "quiz-deletion",
                exhibition.Id.Value
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
