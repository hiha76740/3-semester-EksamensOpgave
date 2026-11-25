using GameService.ApplicationLib.Mappers;
using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities;
using GameService.FacadeLib.Commands.DTOs;
using GameService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging;

namespace GameService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
/// <param name="draftRepo"></param>
/// <param name="logger"></param>
public class CreateQuizDraftHandler(IQuizDraftRepository draftRepo, ILogger<CreateQuizDraftHandler> logger) : ICreateQuizDraftHandler
{
    async Task<Guid> ICreateQuizDraftHandler.HandleAsync(CreateQuizCommand command)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Beginning to create quiz draft with name {QuizName}", command.Title);

        var draft = QuizDraft.Create(
            command.Title,
            command.TitleEn,
            command.ExhibitionId,
            command.QuizLayout,
            command.EndMessage,
            command.EndMessageEn,
            command.Questions.AsNewQuestions());

        await draftRepo.AddAsync(draft);
        await draftRepo.SaveAsync();

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Quiz draft {Id} saved in database successfully", draft.Id.Value);

        return draft.Id.Value;
    }
}
