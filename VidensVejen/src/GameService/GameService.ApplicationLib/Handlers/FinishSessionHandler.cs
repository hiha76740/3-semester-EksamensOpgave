using System.Text.Json;
using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities.Sessions;
using GameService.FacadeLib.Commands.Interfaces;
using GameService.FacadeLib.Queries.DTOs;
using VidensVejen.BuildingBlocksLib.Messaging.Outbox;
using VidensVejen.ContractsLib.Events;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
/// <param name="sessionRepo"></param>
/// <param name="quizRepo"></param>
/// <param name="outbox"></param>
public class FinishSessionHandler(IGameSessionRepository sessionRepo, IQuizRepository quizRepo, IOutboxRepository outbox) : IFinishSessionHandler
{
    async Task<SessionScoreDto> IFinishSessionHandler.HandleAsync(Guid sessionId)
    {
        var session = await sessionRepo.GetSessionByIdAsync(new GameSessionId(sessionId))
            ?? throw new NotFoundException("Session was not found");

        var quiz = await quizRepo.GetQuizByIdAsync(session.QuizId)
            ?? throw new NotFoundException("Quiz was not found");

        var score = session.Finish(quiz);

        var integrationEvent = new GameCompletedIntegrationEvent(
            Guid.NewGuid(),
            "game-completed",
            session.Id.Value,
            quiz.Id.Value,
            quiz.Title,
            quiz.ExhibitionId.Value,
            score.CorrectCount,
            score.TotalCount,
            session.CompletedAt!.Value
            );

        var outboxMessage = new OutboxMessage
        {
            Id = integrationEvent.EventId,
            Type = integrationEvent.GetType().FullName!,
            Topic = integrationEvent.EventTopic,
            Content = JsonSerializer.Serialize(integrationEvent),
            CreatedAtUtc = DateTime.UtcNow
        };

        await outbox.AddAsync(outboxMessage);

        await sessionRepo.SaveAsync();

        return new SessionScoreDto(score.CorrectCount, score.TotalCount, score.Percentage);
    }
}
