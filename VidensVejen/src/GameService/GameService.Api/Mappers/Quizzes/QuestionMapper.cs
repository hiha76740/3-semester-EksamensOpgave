using GameService.FacadeLib.Commands.DTOs;
using GameService.FacadeLib.Queries.DTOs;
using VidensVejen.ContractsLib.Requests.GameService;
using VidensVejen.ContractsLib.Responses.GameService;

namespace GameService.Api.Mappers.Quizzes;

internal static class QuestionMapper
{
    internal static CreateQuestionCommand AsCreateCommand(this CreateQuestionRequest request)
    {
        var output = new CreateQuestionCommand(
            request.Text,
            request.Choices
            .Select(
                q => q.AsCreateCommand())
            .ToList()
        );

        return output;
    }

    internal static QuestionResponse AsResponse(this QuestionDto dto)
    {
        var output = new QuestionResponse(
            dto.Id,
            dto.Text,
            dto.Choices
            .Select(
                q => q.AsResponse())
            .ToList()
            );

        return output;
    }

    internal static UpdateQuestionCommand AsUpdateCommand(this UpdateQuestionRequest request)
    {
        var output = new UpdateQuestionCommand(
            request.Id,
            request.Text,
            request.Choices
            .Select(
                q => q.AsUpdateCommand())
            .ToList()
            );

        return output;
    }
}
