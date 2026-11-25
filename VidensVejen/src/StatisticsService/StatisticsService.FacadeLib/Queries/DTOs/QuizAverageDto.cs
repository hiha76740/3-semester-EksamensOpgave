namespace StatisticsService.FacadeLib.Queries.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="QuizId"></param>
/// <param name="AveragePercentage"></param>
/// <param name="GameCount"></param>
public record QuizAverageDto(
    Guid QuizId,
    int AveragePercentage,
    int GameCount
    );
