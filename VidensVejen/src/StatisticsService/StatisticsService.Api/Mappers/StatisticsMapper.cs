using StatisticsService.FacadeLib.Queries.DTOs;
using VidensVejen.ContractsLib.Responses.StatisticsService;

namespace StatisticsService.Api.Mappers;

internal static class StatisticsMapper
{
    internal static QuizAverageResponse AsResponse(this QuizAverageDto dto)
    {
        var output = new QuizAverageResponse(
            dto.QuizId,
            dto.AveragePercentage,
            dto.GameCount
            );

        return output;
    }
}
