namespace GameService.FacadeLib.Queries.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="QuizLayout"></param>
/// <param name="EndMessage"></param>
/// <param name="Questions"></param>
public record PlayQuizDto(
    Guid Id,
    string? Title,
    string? QuizLayout,
    string? EndMessage,
    List<PlayQuestionDto> Questions
    );
