namespace VidensVejen.Web.Models.Games;

/// <summary>
/// 
/// </summary>
public sealed class QuizAnswerInputModel
{
    /// <summary>
    /// 
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();
    /// <summary>
    /// 
    /// </summary>
    public string Text { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public string Feedback { get; set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public bool IsCorrect { get; set; }
}
