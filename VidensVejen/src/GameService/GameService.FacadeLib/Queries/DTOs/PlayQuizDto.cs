namespace GameService.FacadeLib.Queries.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="TitleEn"></param>
/// <param name="QuizLayout"></param>
/// <param name="EndMessage"></param>
/// <param name="EndMessageEn"></param>
/// <param name="Questions"></param>
public record PlayQuizDto(
    Guid Id,
    string? Title,
    string? TitleEn,
    string? QuizLayout,
    string? EndMessage,
    string? EndMessageEn,
    List<PlayQuestionDto> Questions
    );
