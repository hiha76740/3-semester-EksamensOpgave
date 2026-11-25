using Microsoft.EntityFrameworkCore;
using StatisticsService.FacadeLib.Queries.DTOs;
using StatisticsService.FacadeLib.Queries.Interfaces;
using StatisticsService.InfrastructureLib.Persistence;

namespace StatisticsService.InfrastructureLib.QueryHandlers;

/// <summary>
/// 
/// </summary>
/// <param name="db"></param>
public class GameResultQueryHandlerIMPL(StatisticsDbContext db) : IGameResultQueries
{
    async Task<QuizAverageDto> IGameResultQueries.GetTodayAverageAsync(Guid quizId)
    {
        var today = DateTime.UtcNow.Date;

        var percentages = await db.GameResults
            .AsNoTracking()
            .Where(r => r.QuizId == quizId && r.CompletedAt >= today)
            .Select(r => 100.0 * r.CorrectCount / r.TotalCount)
            .ToListAsync();

        if (percentages.Count == 0)
            return new QuizAverageDto(quizId, 0, 0);

        var average = (int)Math.Round(percentages.Average(), MidpointRounding.AwayFromZero);

        return new QuizAverageDto(quizId, average, percentages.Count);
    }
}
