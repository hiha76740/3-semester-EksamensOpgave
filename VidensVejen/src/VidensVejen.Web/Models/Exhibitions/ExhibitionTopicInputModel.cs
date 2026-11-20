namespace VidensVejen.Web.Models.Exhibitions;

/// <summary>
/// 
/// </summary>
public sealed record ExhibitionTopicInputModel
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
    public string Body { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public string CalloutLabel { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public string CalloutText { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public string Quote { get; set; } = string.Empty;
}
