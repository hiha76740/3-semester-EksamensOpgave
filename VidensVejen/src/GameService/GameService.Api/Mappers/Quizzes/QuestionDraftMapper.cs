using GameService.FacadeLib.Commands.DTOs;
using VidensVejen.ContractsLib.Requests.GameService.Drafts;

namespace GameService.Api.Mappers.Quizzes;

internal static class QuestionDraftMapper
{
    internal static UpdateQuestionDraftCommand AsUpdateCommand(this UpdateQuestionDraftRequest request)
    {
        var output = new UpdateQuestionDraftCommand(
            request.Text,
            request.TextEn,
            request.Choices?.Select(
                c => c.AsUpdateCommand())
            .ToList()
            );

        return output;
    }
}
