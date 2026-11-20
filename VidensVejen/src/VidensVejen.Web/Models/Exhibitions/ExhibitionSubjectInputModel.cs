namespace VidensVejen.Web.Models.Exhibitions;

/// <summary>
/// 
/// </summary>
public sealed class ExhibitionSubjectInputModel
{
    /// <summary>
    /// 
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public List<ExhibitionTopicInputModel> Topics { get; } = [];
}
