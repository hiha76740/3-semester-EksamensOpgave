using Microsoft.AspNetCore.Components.Forms;

namespace VidensVejen.Web.Models.Exhibitions;

/// <summary>
/// Represents the editable draft of an exhibition.
/// </summary>
public sealed class ExhibitionInputModel
{
    /// <summary>
    /// The maximum permitted theme image size, in bytes.
    /// </summary>
    public const long MaxThemeFileSize = 5 * 1024 * 1024;

    /// <summary>
    /// Gets or sets the exhibition name in Danish.
    /// </summary>
    public string ExhibitionName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the exhibition name in English.
    /// </summary>
    public string ExhibitionNameEn { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the theme image selected in the browser, if any.
    /// </summary>
    public IBrowserFile? ThemeFile { get; set; }

    /// <summary>
    /// Gets the four subject slots in display order.
    /// </summary>
    public IReadOnlyList<ExhibitionSubjectInputModel> Subjects { get; } =
        [new(), new(), new(), new()];
}
