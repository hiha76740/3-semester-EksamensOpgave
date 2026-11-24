using GameService.DomainLib.Enums;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.DomainLib.Entities;

/// <summary>
/// 
/// </summary>
public class GameSession
{
    /// <summary>
    /// 
    /// </summary>
    public GameSessionId Id { get; init; } = null!;
    /// <summary>
    /// 
    /// </summary>
    public QuizId QuizId { get; init; } = null!;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="quiz"></param>
    /// <returns></returns>
    /// <exception cref="DomainException"></exception>
    public static GameSession Start(Quiz quiz)
    {
        if (quiz.Status != PublicationStatus.Published)
            throw new DomainException("A session can only be started on a published quiz");

        var session = new GameSession(quiz.Id);

        return session;
    }

    private GameSession(QuizId quizId)
    {
        Id = new GameSessionId(Guid.NewGuid());
        QuizId = quizId;
    }

    // EF Core constructor
    private GameSession() { }
}
