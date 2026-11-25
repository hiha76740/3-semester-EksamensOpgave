using GameService.ApplicationLib.Mappers;
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
/// <param name="draftRepo"></param>
/// <param name="logger"></param>
public class UpdateQuizDraftHandler(
    IQuizDraftRepository draftRepo,
    ILogger<UpdateQuizDraftHandler> logger
    ) : IUpdateQuizDraftHandler
{
    async Task IUpdateQuizDraftHandler.HandleAsync(UpdateQuizDraftCommand command)
    {
        try
        {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Beginning to update quiz draft {Id}", command.DraftId);

            var id = new QuizDraftId(command.DraftId);

            var draft = await draftRepo.GetDraftByIdAsync(id)
                ?? throw new NotFoundException("Quiz draft was not found");

            draft.Update(
                command.Title,
                command.TitleEn,
                command.ExhibitionId,
                command.QuizLayout,
                command.EndMessage,
                command.EndMessageEn,
                command.Questions.AsNewDraftQuestions());

            await draftRepo.SaveAsync();

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Quiz draft {Id} updated successfully", draft.Id.Value);
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
