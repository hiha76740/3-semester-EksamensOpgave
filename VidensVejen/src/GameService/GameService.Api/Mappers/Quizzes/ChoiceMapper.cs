using GameService.FacadeLib.Commands.DTOs;
using GameService.FacadeLib.Queries.DTOs;
using VidensVejen.ContractsLib.Requests.GameService.Quizzes;
using VidensVejen.ContractsLib.Responses.GameService;

namespace GameService.Api.Mappers.Quizzes;

internal static class ChoiceMapper
{
    internal static CreateChoiceCommand AsCreateCommand(this CreateChoiceRequest request)
    {
        var output = new CreateChoiceCommand(
            request.Text,
            request.TextEn,
            request.CorrectAnswer,
            request.Feedback,
            request.FeedbackEn
            );

        return output;
    }

    internal static ChoiceResponse AsResponse(this ChoiceDto dto)
    {
        var output = new ChoiceResponse(
            dto.Text,
            dto.TextEn,
            dto.CorrectAnswer,
            dto.Feedback,
            dto.FeedbackEn
            );

        return output;
    }
}
