namespace GameService.FacadeLib.Commands.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="DraftId"></param>
/// <param name="Title"></param>
/// <param name="TitleEn"></param>
/// <param name="ExhibitionId"></param>
/// <param name="QuizLayout"></param>
/// <param name="Questions"></param>
/// <param name="EndMessage"></param>
/// <param name="EndMessageEn"></param>
/// <param name="Instruction"></param>
/// <param name="InstructionEn"></param>
/// <param name="DraftRowVersion"></param>
public record CreateQuizCommand(
    Guid DraftId,
    string Title,
    string TitleEn,
    Guid ExhibitionId,
    string QuizLayout,
    List<CreateQuestionCommand> Questions,
    string? EndMessage,
    string? EndMessageEn,
    string Instruction,
    string InstructionEn,
    byte[] DraftRowVersion
    );
