using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities.Drafts;
using GameService.FacadeLib.Commands.DTOs;
using GameService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
/// <param name="repo"></param>
/// <param name="logger"></param>
public class DeleteDraftHandler(
    IQuizDraftRepository repo,
    ILogger<DeleteDraftHandler> logger
    ) : IDeleteDraftHandler
{
    async Task IDeleteDraftHandler.HandleAsync(DeleteDraftCommand command)
    {
        try
        {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Beginning deletion of draft {DraftId}", command.DraftId);

            var draftId = new QuizDraftId(command.DraftId);

            var draft = await repo.GetDraftByIdAsync(draftId)
                ?? throw new NotFoundException("Draft was not found");

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Deleting draft from auth database");

            repo.Delete(draft);

            await repo.SaveAsync();

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Draft {Id} was deleted from auth database successfully", draft.Id.Value);

        }
        catch (Exception ex)
        {
            if (logger.IsEnabled(LogLevel.Error))
            {
                logger.LogError("Error {ErrorMessage}", ex.Message);

                if (ex.InnerException != null)
                    logger.LogError("InnerException Error {Error}", ex.InnerException.Message);
            }

            throw;
        }
    }
}
