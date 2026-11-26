namespace VidensVejen.Web.Models.Exhibitions;

/// <summary>
/// Represents a subject slot in an exhibition draft.
/// </summary>
public sealed class ExhibitionSubjectInputModel
{
    /// <summary>
    /// Gets or sets the title in Danish.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the title in English.
    /// </summary>
    public string TitleEn { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description in Danish.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description in English.
    /// </summary>
    public string DescriptionEn { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the subject opens a quiz.
    /// </summary>
    public bool IsQuiz { get; set; }

    /// <summary>
    /// Gets or sets the identifier of the selected existing quiz, if any.
    /// </summary>
    public Guid? QuizId { get; set; }

    /// <summary>Gets or sets the identifier of the linked quiz draft.</summary>
    public Guid? QuizDraftId { get; set; }

    /// <summary>Gets the topics shown for a content subject.</summary>
    public List<ExhibitionTopicInputModel> Topics { get; } = [];
}
