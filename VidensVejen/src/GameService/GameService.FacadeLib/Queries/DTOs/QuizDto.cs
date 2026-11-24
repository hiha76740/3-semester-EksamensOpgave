namespace GameService.FacadeLib.Queries.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="TitleEn"></param>
/// <param name="ExhibitionId"></param>
/// <param name="QuizLayout"></param>
/// <param name="EndMessage"></param>
/// <param name="EndMessageEn"></param>
/// <param name="Status"></param>
/// <param name="Questions"></param>
public record QuizDto(
    Guid Id,
    string? Title,
    string? TitleEn,
    Guid? ExhibitionId,
    string? QuizLayout,
    string? EndMessage,
    string? EndMessageEn,
    string Status,
    List<QuestionDto> Questions
    );
