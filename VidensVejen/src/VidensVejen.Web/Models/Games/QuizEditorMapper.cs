using VidensVejen.ContractsLib.Responses.GameService;

namespace VidensVejen.Web.Models.Games;

internal static class QuizEditorMapper
{
    internal static QuizInputModel FromDraft(QuizDraftResponse draft)
    {
        return Create(
        draft.Title, draft.TitleEn, draft.ExhibitionId, draft.QuizLayout, draft.Instruction, draft.InstructionEn,
        draft.EndMessage, draft.EndMessageEn, draft.Questions.Select(question => MapQuestion(
            question.Text, question.TextEn, question.Choices.Select(choice =>
                MapAnswer(choice.Text, choice.TextEn, choice.CorrectAnswer, choice.Feedback, choice.FeedbackEn)))));
    }

    internal static QuizInputModel FromPublished(QuizResponse quiz)
    {
        return Create(
        quiz.Title, quiz.TitleEn, quiz.ExhibitionId, quiz.QuizLayout, quiz.Instruction, quiz.InstructionEn,
        quiz.EndMessage, quiz.EndMessageEn, quiz.Questions.Select(question => MapQuestion(
            question.Text, question.TextEn, question.Choices.Select(choice =>
                MapAnswer(choice.Text, choice.TextEn, choice.CorrectAnswer, choice.Feedback, choice.FeedbackEn)))));
    }

    private static QuizInputModel Create(string? title, string? titleEn, Guid? exhibitionId,
        string? layout, string? instruction, string? instructionEn, string? endMessage, string? endMessageEn,
        IEnumerable<QuizQuestionInputModel> questions)
    {
        var model = new QuizInputModel
        {
            Title = title ?? string.Empty,
            TitleEn = titleEn ?? string.Empty,
            ExhibitionId = exhibitionId,
            QuizLayout = layout ?? string.Empty,
            Instruction = instruction ?? string.Empty,
            InstructionEn = instructionEn ?? string.Empty,
            EndMessage = endMessage,
            EndMessageEn = endMessageEn
        };

        var savedQuestions = questions.ToList();
        if (savedQuestions.Count > 0)
        {
            model.Questions.Clear();
            model.Questions.AddRange(savedQuestions);
        }

        return model;
    }

    private static QuizQuestionInputModel MapQuestion(string? text, string? textEn,
        IEnumerable<QuizAnswerInputModel> answers)
    {
        var question = new QuizQuestionInputModel { Text = text ?? string.Empty, TextEn = textEn ?? string.Empty };
        question.Answers.Clear();
        question.Answers.AddRange(answers);

        while (question.Answers.Count < 2)
        {
            question.Answers.Add(new QuizAnswerInputModel());
        }

        if (!question.Answers.Any(answer => answer.IsCorrect))
        {
            question.Answers[0].IsCorrect = true;
        }

        return question;
    }

    private static QuizAnswerInputModel MapAnswer(string? text, string? textEn,
        bool correctAnswer, string? feedback, string? feedbackEn)
    {
        return new()
        {
            Text = text ?? string.Empty,
            TextEn = textEn ?? string.Empty,
            IsCorrect = correctAnswer,
            Feedback = feedback ?? string.Empty,
            FeedbackEn = feedbackEn ?? string.Empty
        };
    }
}
