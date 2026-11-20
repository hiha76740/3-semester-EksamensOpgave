namespace VidensVejen.Web.Models.Games;

/// <summary>
/// 
/// </summary>
public sealed class QuizQuestionInputModel
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
    public List<QuizAnswerInputModel> Answers { get; } =
    [
        new QuizAnswerInputModel { IsCorrect = true },
        new QuizAnswerInputModel()
    ];
}
