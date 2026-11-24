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
/// <param name="quizRepo"></param>
/// <param name="logger"></param>
public class CreateQuizHandler(IQuizRepository quizRepo, ILogger<CreateQuizHandler> logger) : ICreateQuizHandler
{
    async Task<Guid> ICreateQuizHandler.HandleAsync(CreateQuizCommand command)
    {
        try
        {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Beginning to create quiz with name {QuizName}", command.Title);

            var questions = new List<NewQuestion>();

            foreach (var questionCommand in command.Questions)
            {
                var choices = new List<Choice>();

                foreach (var choiceCommand in questionCommand.Choices)
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

            var quiz = Quiz.Create(
                command.Title,
                command.TitleEn,
                command.ExhibitionId,
                command.QuizLayout,
                command.EndMessage,
                command.EndMessageEn,
                questions);

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Quiz created with id: {QuizId}", quiz.Id.Value);

            await quizRepo.AddAsync(quiz);
            await quizRepo.SaveAsync();

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Quiz {Id} saved in database successfully", quiz.Id.Value);

            return quiz.Id.Value;
        }
        catch (Exception)
        {

            throw;
        }
    }
}
