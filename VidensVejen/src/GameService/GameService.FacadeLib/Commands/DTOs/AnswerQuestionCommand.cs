namespace GameService.FacadeLib.Commands.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="SessionId"></param>
/// <param name="QuestionId"></param>
/// <param name="ChoiceIndex"></param>
public record AnswerQuestionCommand(
    Guid SessionId,
    Guid QuestionId,
    int ChoiceIndex
    );
