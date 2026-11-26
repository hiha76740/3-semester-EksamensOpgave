using VidensVejen.ContractsLib.Requests.GameService.Drafts;
using VidensVejen.ContractsLib.Requests.GameService.Quizzes;
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

        model.Questions.Clear();
        model.Questions.AddRange(questions);

        return model;
    }

    private static QuizQuestionInputModel MapQuestion(string? text, string? textEn,
        IEnumerable<QuizAnswerInputModel> answers)
    {
        var question = new QuizQuestionInputModel { Text = text ?? string.Empty, TextEn = textEn ?? string.Empty };
        question.Answers.Clear();
        question.Answers.AddRange(answers);

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

    internal static CreateQuizRequest BuildRequest(Guid draftId, QuizInputModel input)
    {
        return new(draftId, input.Title, input.TitleEn, input.ExhibitionId.GetValueOrDefault(), input.QuizLayout,
            input.Questions.Select(question => new CreateQuestionRequest(
                question.Text, question.TextEn,
                question.Answers.Select(answer => new CreateChoiceRequest(
                    answer.Text, answer.TextEn, answer.IsCorrect,
                    answer.Feedback, answer.FeedbackEn)).ToList())).ToList(),
            input.EndMessage, input.EndMessageEn, input.Instruction, input.InstructionEn, input.RowVersion);
    }

    internal static UpdateQuizDraftRequest BuildDraftRequest(QuizInputModel input)
    {
        return new(input.Title, input.TitleEn, input.ExhibitionId, input.QuizLayout,
            input.Questions.Select(question => new UpdateQuestionDraftRequest(
                question.Text, question.TextEn,
                question.Answers.Select(answer => new UpdateChoiceDraftRequest(
                    answer.Text, answer.TextEn, answer.IsCorrect,
                    answer.Feedback, answer.FeedbackEn)).ToList())).ToList(),
            input.EndMessage, input.EndMessageEn, input.Instruction, input.InstructionEn, input.RowVersion);
    }

    internal static void ValidatePublication(QuizInputModel input, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(input.Title) || string.IsNullOrWhiteSpace(input.TitleEn))
        {
            errors.Add("Quizzen skal have en titel på dansk og engelsk.");
        }

        if (string.IsNullOrWhiteSpace(input.Instruction) || string.IsNullOrWhiteSpace(input.InstructionEn))
        {
            errors.Add("Quizzen skal have en instruktion på dansk og engelsk.");
        }

        if (input.ExhibitionId == null || input.ExhibitionId == Guid.Empty)
        {
            errors.Add("Vælg en udstilling til quizzen.");
        }

        if (string.IsNullOrWhiteSpace(input.QuizLayout))
        {
            errors.Add("Vælg et layout til quizzen.");
        }

        if (input.Questions.Count == 0)
        {
            errors.Add("Quizzen skal have mindst ét spørgsmål.");
        }

        for (var i = 0; i < input.Questions.Count; i++)
        {
            var question = input.Questions[i];

            if (string.IsNullOrWhiteSpace(question.Text) || string.IsNullOrWhiteSpace(question.TextEn))
            {
                errors.Add($"Spørgsmål {i + 1} skal have tekst på dansk og engelsk.");
            }

            if (question.Answers.Count < 2)
            {
                errors.Add($"Spørgsmål {i + 1} skal have mindst to svar.");
            }

            if (question.Answers.Count(answer => answer.IsCorrect) != 1)
            {
                errors.Add($"Spørgsmål {i + 1} skal have præcis ét korrekt svar.");
            }

            for (var j = 0; j < question.Answers.Count; j++)
            {
                if (string.IsNullOrWhiteSpace(question.Answers[j].Text) || string.IsNullOrWhiteSpace(question.Answers[j].TextEn))
                {
                    errors.Add($"Spørgsmål {i + 1}, svar {j + 1} skal have svartekst på dansk og engelsk.");
                }
            }
        }
    }
}
