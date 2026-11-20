namespace VidensVejen.Web.Models.Games;

/// <summary>
/// 
/// </summary>
public sealed class QuizInputModel
{
    /// <summary>
    /// 
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string Instruction { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public Guid? ExhibitionId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public List<QuizQuestionInputModel> Questions { get; } = [new QuizQuestionInputModel()];
}
