namespace ExhibitionService.FacadeLib.Queries.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="TitleEn"></param>
/// <param name="Slug"></param>
/// <param name="Description"></param>
/// <param name="DescriptionEn"></param>
/// <param name="QuizId"></param>
/// <param name="QuizDraftId"></param>
/// <param name="IsQuiz"></param>
/// <param name="Topics"></param>
public record SubjectDraftDto(
    Guid Id,
    string? Title,
    string? TitleEn,
    string? Slug,
    string? Description,
    string? DescriptionEn,
    Guid? QuizId,
    Guid? QuizDraftId,
    bool IsQuiz,
    List<TopicDraftDto> Topics);
