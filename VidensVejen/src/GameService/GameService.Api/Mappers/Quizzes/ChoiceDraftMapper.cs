using GameService.FacadeLib.Commands.DTOs;
using VidensVejen.ContractsLib.Requests.GameService.Drafts;

namespace GameService.Api.Mappers.Quizzes;

internal static class ChoiceDraftMapper
{
    internal static UpdateChoiceDraftCommand AsUpdateCommand(this UpdateChoiceDraftRequest request)
    {
        var output = new UpdateChoiceDraftCommand(
            request.Text,
            request.TextEn,
            request.CorrectAnswer,
            request.Feedback,
            request.FeedbackEn
            );

        return output;
    }
}
