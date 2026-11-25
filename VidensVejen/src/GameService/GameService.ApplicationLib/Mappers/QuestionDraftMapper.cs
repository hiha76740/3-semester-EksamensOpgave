using GameService.DomainLib.ValueObjects.Drafts;
using GameService.FacadeLib.Commands.DTOs;

namespace GameService.ApplicationLib.Mappers;

internal static class QuestionDraftMapper
{
    internal static List<NewQuestionDraft> AsNewDraftQuestions(this List<UpdateQuestionDraftCommand>? commands)
    {
        var questions = new List<NewQuestionDraft>();

        foreach (var questionCommand in commands ?? [])
        {
            var choices = new List<ChoiceDraft>();

            foreach (var choiceCommand in questionCommand.Choices ?? [])
            {
                choices.Add(new ChoiceDraft(
                    choiceCommand.Text,
                    choiceCommand.TextEn,
                    choiceCommand.CorrectAnswer,
                    choiceCommand.Feedback,
                    choiceCommand.FeedbackEn
                    ));
            }

            questions.Add(new NewQuestionDraft(
                questionCommand.Text,
                questionCommand.TextEn,
                choices
                ));
        }

        return questions;
    }
}
