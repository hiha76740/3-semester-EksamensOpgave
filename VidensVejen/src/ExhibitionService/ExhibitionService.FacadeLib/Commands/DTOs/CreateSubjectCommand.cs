namespace ExhibitionService.FacadeLib.Commands.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Title"></param>
/// <param name="TitleEn"></param>
/// <param name="Slug"></param>
/// <param name="Description"></param>
/// <param name="DescriptionEn"></param>
/// <param name="QuizId"></param>
/// <param name="Topics"></param>
public record CreateSubjectCommand(
    string Title,
    string TitleEn,
    string Slug,
    string Description,
    string DescriptionEn,
    Guid? QuizId,
    List<CreateTopicCommand> Topics
    );
