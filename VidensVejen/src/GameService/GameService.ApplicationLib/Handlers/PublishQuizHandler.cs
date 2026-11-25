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
/// <param name="quizRepo"></param>
/// <param name="logger"></param>
public class PublishQuizHandler(IQuizDraftRepository draftRepo, IQuizRepository quizRepo, ILogger<PublishQuizHandler> logger) : IPublishQuizHandler
{
    async Task<Guid> IPublishQuizHandler.HandleAsync(Guid draftId, CreateQuizCommand command)
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Beginning to publish quiz draft {Id}", draftId);

        var draft = await draftRepo.GetDraftByIdAsync(new QuizDraftId(draftId))
            ?? throw new NotFoundException("Quiz draft was not found");

        var quiz = Quiz.Create(
            command.Title,
            command.TitleEn,
            command.ExhibitionId,
            command.QuizLayout,
            command.EndMessage,
            command.EndMessageEn,
            command.Questions.AsNewQuestions());

        await quizRepo.AddAsync(quiz);

        draftRepo.Delete(draft);

        await draftRepo.SaveAsync();

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Quiz {QuizId} published and draft {DraftId} deleted", quiz.Id.Value, draftId);

        return quiz.Id.Value;
    }
}
