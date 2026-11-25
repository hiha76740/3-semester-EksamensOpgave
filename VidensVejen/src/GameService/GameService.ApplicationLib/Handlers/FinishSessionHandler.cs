using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities;
using GameService.FacadeLib.Commands.Interfaces;
using GameService.FacadeLib.Queries.DTOs;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
/// <param name="sessionRepo"></param>
/// <param name="quizRepo"></param>
public class FinishSessionHandler(IGameSessionRepository sessionRepo, IQuizRepository quizRepo) : IFinishSessionHandler
{
    async Task<SessionScoreDto> IFinishSessionHandler.HandleAsync(Guid sessionId)
    {
        var session = await sessionRepo.GetSessionByIdAsync(new GameSessionId(sessionId))
            ?? throw new NotFoundException("Session was not found");

        var quiz = await quizRepo.GetQuizByIdAsync(session.QuizId)
            ?? throw new NotFoundException("Quiz was not found");

        var score = session.Finish(quiz);

        await sessionRepo.SaveAsync();

        return new SessionScoreDto(score.CorrectCount, score.TotalCount, score.Percentage);
    }
}
