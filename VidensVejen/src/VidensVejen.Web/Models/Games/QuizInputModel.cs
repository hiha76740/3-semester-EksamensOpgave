namespace VidensVejen.Web.Models.Games;

/// <summary>
/// Represents the editable draft of a quiz.
/// </summary>
public sealed class QuizInputModel
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
    /// Gets or sets the instructions shown to the player in Danish.
    /// </summary>
    public string Instruction { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the instructions shown to the player in English.
    /// </summary>
    public string InstructionEn { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the identifier of the associated exhibition, if one has been selected.
    /// </summary>
    public Guid ExhibitionId { get; set; }

    /// <summary>
    /// Gets the editable questions in display order. The draft starts with one question.
    /// </summary>
    public List<QuizQuestionInputModel> Questions { get; } = [new QuizQuestionInputModel()];
}
