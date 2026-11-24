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
    public Guid? ExhibitionId { get; set; }

    /// <summary>
    /// Gets or sets the layout of the quiz.
    /// </summary>
    public string QuizLayout { get; set; } = string.Empty;

    /// <summary>
    /// Gets the editable questions in display order. The draft starts with one question.
    /// </summary>
    public List<QuizQuestionInputModel> Questions { get; } = [new QuizQuestionInputModel()];

    /// <summary>
    /// Gets or sets the end message of the quiz in Danish.
    /// </summary>
    public string? EndMessage { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the end message of the quiz in English.
    /// </summary>
    public string? EndMessageEn { get; set; } = string.Empty;
}
