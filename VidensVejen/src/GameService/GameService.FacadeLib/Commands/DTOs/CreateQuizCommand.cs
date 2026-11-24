namespace GameService.FacadeLib.Commands.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Title"></param>
/// <param name="TitleEn"></param>
/// <param name="ExhibitionId"></param>
/// <param name="QuizLayout"></param>
/// <param name="Questions"></param>
/// <param name="EndMessage"></param>
/// <param name="EndMessageEn"></param>
public record CreateQuizCommand(
    string Title,
    string TitleEn,
    Guid ExhibitionId,
    string QuizLayout,
    List<CreateQuestionCommand> Questions,
    string? EndMessage,
    string? EndMessageEn
    );
