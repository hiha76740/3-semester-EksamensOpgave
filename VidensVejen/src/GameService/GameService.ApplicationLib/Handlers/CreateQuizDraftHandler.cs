using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities;
using GameService.DomainLib.ValueObjects;
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

        var questions = new List<NewQuestion>();

        foreach (var questionCommand in command.Questions ?? [])
        {
            var choices = new List<Choice>();

            foreach (var choiceCommand in questionCommand.Choices ?? [])
            {
                choices.Add(new Choice(
                    choiceCommand.Text,
                    choiceCommand.TextEn,
                    choiceCommand.CorrectAnswer,
                    choiceCommand.Feedback,
                    choiceCommand.FeedbackEn
                    ));
            }

            questions.Add(new NewQuestion(
                questionCommand.Text,
                questionCommand.TextEn,
                choices
                ));
        }

        var draft = QuizDraft.Create(
            command.Title,
            command.TitleEn,
            command.ExhibitionId,
            command.QuizLayout,
            command.EndMessage,
            command.EndMessageEn,
            questions);

        await draftRepo.AddAsync(draft);
        await draftRepo.SaveAsync();

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Quiz draft {Id} saved in database successfully", draft.Id.Value);

        return draft.Id.Value;
    }
}
