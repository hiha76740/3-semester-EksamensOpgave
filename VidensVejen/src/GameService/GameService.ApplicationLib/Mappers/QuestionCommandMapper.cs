using GameService.DomainLib.ValueObjects;
using GameService.FacadeLib.Commands.DTOs;

namespace GameService.ApplicationLib.Mappers;

internal static class QuestionCommandMapper
{
    internal static List<NewQuestion> AsNewQuestions(this List<CreateQuestionCommand>? commands)
    {
        var questions = new List<NewQuestion>();

        foreach (var questionCommand in commands ?? [])
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

        return questions;
    }
}
