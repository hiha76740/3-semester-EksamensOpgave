using GameService.FacadeLib.Commands.DTOs;
using VidensVejen.ContractsLib.Requests.GameService.Drafts;

namespace GameService.Api.Mappers.Quizzes;

internal static class QuizDraftMapper
{
    internal static UpdateQuizDraftCommand AsUpdateCommand(this UpdateQuizDraftRequest request, Guid draftId)
    {
        var output = new UpdateQuizDraftCommand(
            draftId,
            request.Title,
            request.TitleEn,
            request.ExhibitionId,
            request.QuizLayout,
            request.Questions?.Select(q => q.AsUpdateCommand()).ToList(),
            request.EndMessage,
            request.EndMessageEn,
            request.Instruction,
            request.InstructionEn
            );

        return output;
    }
}
