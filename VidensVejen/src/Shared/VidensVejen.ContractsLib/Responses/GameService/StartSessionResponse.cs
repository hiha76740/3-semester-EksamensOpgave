namespace VidensVejen.ContractsLib.Responses.GameService;

/// <summary>
/// 
/// </summary>
/// <param name="SessionId"></param>
/// <param name="Quiz"></param>
/// <example>
/// {
///     "sessionId": "123e4567-e89b-12d3-a456-426614174000",
///     "quiz": {
///         "id": "123e4567-e89b-12d3-a456-426614174000",
///         "title": "Affaldshierakiet",
///         "quizLayout": "Vertikalt",
///         "endMessage": "Godt klaret!",
///         "questions": [
///             {
///                 "id": "123e4567-e89b-12d3-a456-426614174000",
///                 "text": "Hvad skal dette sorteres som?",
///                 "choices": [
///                     { "index": 0, "text": "Plastik" },
///                     { "index": 1, "text": "Metal" }
///                 ]
///             }
///         ]
///     }
/// }
/// </example>
public record StartSessionResponse(
    Guid SessionId,
    PlayQuizResponse Quiz
    )
{
    /// <summary>
    /// A unique identifier for the game session. Used when the guest answers and finishes.
    /// </summary>
    public Guid SessionId { get; init; } = SessionId;

    /// <summary>
    /// The quiz to play, without the correct answers.
    /// </summary>
    public PlayQuizResponse Quiz { get; init; } = Quiz;
}
