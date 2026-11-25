namespace VidensVejen.ContractsLib.Events;

/// <summary>
/// 
/// </summary>
/// <param name="EventId"></param>
/// <param name="EventTopic"></param>
/// <param name="SessionId"></param>
/// <param name="QuizId"></param>
/// <param name="QuizTitle"></param>
/// <param name="ExhibitionId"></param>
/// <param name="CorrectCount"></param>
/// <param name="TotalCount"></param>
/// <param name="CompletedAt"></param>
/// <example>
/// {
///     "eventId": "5724e1de-00ce-4b6a-9d99-e820fbd5cc9d",
///     "eventTopic": "game-completed",
///     "sessionId": "5724e1de-00ce-4b6a-9d99-e820fbd5cc9d",
///     "quizId": "5724e1de-00ce-4b6a-9d99-e820fbd5cc9d",
///     "quizTitle": "Affaldshierakiet",
///     "exhibitionId": "5724e1de-00ce-4b6a-9d99-e820fbd5cc9d",
///     "correctCount": 5,
///     "totalCount": 8,
///     "completedAt": "2026-11-25T12:00:00Z"
/// }
/// </example>
public record GameCompletedIntegrationEvent(
    Guid EventId,
    string EventTopic,
    Guid SessionId,
    Guid QuizId,
    string QuizTitle,
    Guid ExhibitionId,
    int CorrectCount,
    int TotalCount,
    DateTime CompletedAt
    )
{
    /// <summary>
    /// A unique identifier for the event.
    /// </summary>
    public Guid EventId { get; init; } = EventId;

    /// <summary>
    /// The topic of the event
    /// </summary>
    public string EventTopic { get; init; } = EventTopic;

    /// <summary>
    /// A unique identifier for the finished game session.
    /// </summary>
    public Guid SessionId { get; init; } = SessionId;

    /// <summary>
    /// A unique identifier for the quiz that was played.
    /// </summary>
    public Guid QuizId { get; init; } = QuizId;

    /// <summary>
    /// The title of the quiz, so the statistics still make sense if the quiz is deleted.
    /// </summary>
    public string QuizTitle { get; init; } = QuizTitle;

    /// <summary>
    /// A unique identifier for the exhibition the quiz belongs to.
    /// </summary>
    public Guid ExhibitionId { get; init; } = ExhibitionId;

    /// <summary>
    /// The number of questions answered correctly.
    /// </summary>
    public int CorrectCount { get; init; } = CorrectCount;

    /// <summary>
    /// The number of questions in the quiz.
    /// </summary>
    public int TotalCount { get; init; } = TotalCount;

    /// <summary>
    /// The time the game was finished (UTC).
    /// </summary>
    public DateTime CompletedAt { get; init; } = CompletedAt;
}
