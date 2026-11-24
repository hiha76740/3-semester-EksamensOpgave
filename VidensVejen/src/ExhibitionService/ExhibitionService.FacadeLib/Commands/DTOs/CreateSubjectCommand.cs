namespace ExhibitionService.FacadeLib.Commands.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Title"></param>
/// <param name="Slug"></param>
/// <param name="Description"></param>
/// <param name="Topics"></param>
public record CreateSubjectCommand(
    string Title,
    string Slug,
    string Description,
    List<CreateTopicCommand> Topics
    );
