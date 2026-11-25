namespace GameService.FacadeLib.Queries.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="CorrectCount"></param>
/// <param name="TotalCount"></param>
/// <param name="Percentage"></param>
public record SessionScoreDto(
    int CorrectCount,
    int TotalCount,
    int Percentage
    );
