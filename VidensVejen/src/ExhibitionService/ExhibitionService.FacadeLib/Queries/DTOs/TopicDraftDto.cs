namespace ExhibitionService.FacadeLib.Queries.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Title"></param>
/// <param name="Description"></param>
/// <param name="Body"></param>
/// <param name="CalloutLabel"></param>
/// <param name="CalloutText"></param>
/// <param name="Quote"></param>
public record TopicDraftDto(
    string? Title,
    string? Description,
    string? Body,
    string? CalloutLabel,
    string? CalloutText,
    string? Quote);
