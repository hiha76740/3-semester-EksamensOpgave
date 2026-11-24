namespace VidensVejen.Web.Models.Games;

/// <summary>
/// Represents an editable answer option for a quiz question.
/// </summary>
public sealed class QuizChoiceInputModel
{
    /// <summary>
    /// Gets the client-side identifier used to track this item while editing the quiz.
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets the answer text in Danish.
    /// </summary>
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the answer text in English.
    /// </summary>
    public string TextEn { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional feedback for the answer in Danish.
    /// </summary>
    public string Feedback { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional feedback for the answer in English.
    /// </summary>
    public string FeedbackEn { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether this option is the correct answer.
    /// </summary>
    public bool IsCorrect { get; set; }
}
