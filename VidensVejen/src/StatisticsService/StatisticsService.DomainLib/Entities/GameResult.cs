namespace StatisticsService.DomainLib.Entities;

/// <summary>
/// 
/// </summary>
public class GameResult
{
    /// <summary>
    /// 
    /// </summary>
    public GameResultId Id { get; init; } = null!;
    /// <summary>
    /// 
    /// </summary>
    public Guid QuizId { get; init; }
    /// <summary>
    /// 
    /// </summary>
    public string QuizTitle { get; init; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public Guid ExhibitionId { get; init; }
    /// <summary>
    /// 
    /// </summary>
    public int CorrectCount { get; init; }
    /// <summary>
    /// 
    /// </summary>
    public int TotalCount { get; init; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime CompletedAt { get; init; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="quizId"></param>
    /// <param name="quizTitle"></param>
    /// <param name="exhibitionId"></param>
    /// <param name="correctCount"></param>
    /// <param name="totalCount"></param>
    /// <param name="completedAt"></param>
    /// <returns></returns>
    public static GameResult Create(Guid sessionId, Guid quizId, string quizTitle, Guid exhibitionId, int correctCount, int totalCount, DateTime completedAt)
    {
        var gameResult = new GameResult(new GameResultId(sessionId), quizId, quizTitle, exhibitionId, correctCount, totalCount, completedAt);

        return gameResult;
    }

    private GameResult(GameResultId id, Guid quizId, string quizTitle, Guid exhibitionId, int correctCount, int totalCount, DateTime completedAt)
    {
        Id = id;
        QuizId = quizId;
        QuizTitle = quizTitle;
        ExhibitionId = exhibitionId;
        CorrectCount = correctCount;
        TotalCount = totalCount;
        CompletedAt = completedAt;
    }

    // EF Core constructor
    private GameResult() { }
}
