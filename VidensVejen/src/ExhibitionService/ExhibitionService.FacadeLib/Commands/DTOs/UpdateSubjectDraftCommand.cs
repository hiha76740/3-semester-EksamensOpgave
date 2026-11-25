namespace ExhibitionService.FacadeLib.Commands.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="TitleEn"></param>
/// <param name="Slug"></param>
/// <param name="Description"></param>
/// <param name="DescriptionEn"></param>
/// <param name="TopicDrafts"></param>
public record UpdateSubjectDraftCommand(
    Guid Id,
    string? Title,
    string? TitleEn,
    string? Slug,
    string? Description,
    string? DescriptionEn,
    List<UpdateTopicDraftCommand> TopicDrafts
    );
