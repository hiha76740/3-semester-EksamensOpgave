namespace VidensVejen.Web.Models.Exhibitions;

/// <summary>
/// Represents the editable Danish and English content of an exhibition topic.
/// </summary>
public sealed record ExhibitionTopicInputModel
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
    /// Gets or sets the main text in Danish.
    /// </summary>
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the main text in English.
    /// </summary>
    public string BodyEn { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the heading of the highlighted callout in Danish.
    /// </summary>
    public string CalloutLabel { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the heading of the highlighted callout in English.
    /// </summary>
    public string CalloutLabelEn { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the text of the highlighted callout in Danish.
    /// </summary>
    public string CalloutText { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the text of the highlighted callout in English.
    /// </summary>
    public string CalloutTextEn { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional quotation in Danish.
    /// </summary>
    public string Quote { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the optional quotation in English.
    /// </summary>
    public string QuoteEn { get; set; } = string.Empty;
}
