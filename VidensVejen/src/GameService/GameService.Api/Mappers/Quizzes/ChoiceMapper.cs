using GameService.FacadeLib.Commands.DTOs;
using GameService.FacadeLib.Queries.DTOs;
using VidensVejen.ContractsLib.Requests.GameService;
using VidensVejen.ContractsLib.Responses.GameService;

namespace GameService.Api.Mappers.Quizzes;

internal static class ChoiceMapper
{
    internal static CreateChoiceCommand AsCreateCommand(this CreateChoiceRequest request)
    {
        var output = new CreateChoiceCommand(
            request.Text,
            request.CorrectAnswer
            );

        return output;
    }

    internal static ChoiceResponse AsResponse(this ChoiceDto dto)
    {
        var output = new ChoiceResponse(
            dto.Text,
            dto.CorrectAnswer
            );

        return output;
    }

    internal static UpdateChoiceCommand AsUpdateCommand(this UpdateChoiceRequest request)
    {
        var output = new UpdateChoiceCommand(
            request.Id,
            request.Text,
            request.CorrectAnswer
            );

        return output;
    }
}
