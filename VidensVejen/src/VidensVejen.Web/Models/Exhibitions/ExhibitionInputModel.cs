using Microsoft.AspNetCore.Components.Forms;

namespace VidensVejen.Web.Models.Exhibitions;

/// <summary>
/// 
/// </summary>
public sealed class ExhibitionInputModel
{
    /// <summary>
    /// 
    /// </summary>
    public const long MaxThemeFileSize = 5 * 1024 * 1024;

    /// <summary>
    /// 
    /// </summary>
    public string ExhibitionName { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public IBrowserFile? ThemeFile { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyList<ExhibitionSubjectInputModel> Subjects { get; } =
        [new(), new(), new(), new()];
}
