using GameService.ApplicationLib.Mappers;
using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities;
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
public class UpdateQuizDraftHandler(IQuizDraftRepository draftRepo, ILogger<UpdateQuizDraftHandler> logger) : IUpdateQuizDraftHandler
{
    async Task IUpdateQuizDraftHandler.HandleAsync(Guid draftId, CreateQuizCommand command)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Beginning to update quiz draft {Id}", draftId);

        var draft = await draftRepo.GetDraftByIdAsync(new QuizDraftId(draftId))
            ?? throw new NotFoundException("Quiz draft was not found");

        draft.Update(
            command.Title,
            command.TitleEn,
            command.ExhibitionId,
            command.QuizLayout,
            command.EndMessage,
            command.EndMessageEn,
            command.Questions.AsNewQuestions());

        await draftRepo.SaveAsync();

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Quiz draft {Id} updated successfully", draftId);
    }
}
