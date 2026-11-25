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
            request.TextEn,
            request.Choices?
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
            dto.TextEn,
            dto.Choices
            .Select(
                q => q.AsResponse())
            .ToList()
            );

        return output;
    }
}
