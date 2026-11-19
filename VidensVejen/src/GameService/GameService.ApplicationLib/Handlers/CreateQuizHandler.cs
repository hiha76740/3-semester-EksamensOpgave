using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities;
using GameService.DomainLib.ValueObjects;
using GameService.FacadeLib.Commands.DTOs;
using GameService.FacadeLib.Commands.Interfaces;

namespace GameService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
/// <param name="quizRepo"></param>
public class CreateQuizHandler(IQuizRepository quizRepo) : ICreateQuizHandler
{
    async Task<Guid> ICreateQuizHandler.HandleAsync(CreateQuizCommand command)
    {
        var questions = new List<NewQuestion>();

        foreach (var questionCommand in command.Questions)
        {
            var choices = new List<Choice>();

            foreach (var choiceCommand in questionCommand.Choices)
            {
                choices.Add(new Choice(choiceCommand.Text, choiceCommand.CorrectAnswer));
            }

            questions.Add(new NewQuestion(
                questionCommand.Text,
                choices,
                questionCommand.CorrectFeedback,
                questionCommand.IncorrectFeedback));
        }

        var quiz = Quiz.Create(
            command.Title,
            command.ExhibitionId,
            command.QuizLayout,
            command.EndMessage,
            questions);

        await quizRepo.AddAsync(quiz);
        await quizRepo.SaveAsync();

        return quiz.Id.Value;
    }
}
