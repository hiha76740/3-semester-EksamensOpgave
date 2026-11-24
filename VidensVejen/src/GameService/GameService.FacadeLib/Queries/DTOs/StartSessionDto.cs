namespace GameService.FacadeLib.Queries.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="SessionId"></param>
/// <param name="Quiz"></param>
public record StartSessionDto(
    Guid SessionId,
    PlayQuizDto Quiz
    );
