namespace VidensVejen.Web.Models.Games;

/// <summary>
/// Represents an editable question and its answer options.
/// </summary>
public sealed class QuizQuestionInputModel
{
    /// <summary>
    /// Gets the client-side identifier used to track this item while editing the quiz.
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the question text in Danish.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the question text in English.
    /// </summary>
    public string TextEn { get; set; } = string.Empty;

    /// <summary>
    /// Gets the editable answer options in display order. The question starts with two options, the first marked correct.
    /// </summary>
    public List<QuizAnswerInputModel> Answers { get; } =
    [
        new QuizAnswerInputModel { IsCorrect = true },
        new QuizAnswerInputModel()
    ];
}
