using GameService.FacadeLib.Commands.DTOs;
using GameService.FacadeLib.Queries.DTOs;
using VidensVejen.ContractsLib.Requests.GameService;
using VidensVejen.ContractsLib.Responses.GameService;

namespace GameService.Api.Mappers.Quizzes;

internal static class QuizMapper
{
    internal static CreateQuizCommand AsCreateCommand(this CreateQuizRequest request)
    {
        var output = new CreateQuizCommand(
            request.Title,
            request.ExhibitionId,
            request.QuizLayout,
            request.Questions?
            .Select(
                q => q.AsCreateCommand())
            .ToList(),
             request.EndMessage
        );

        return output;
    }

    internal static QuizResponse AsResponse(this QuizDto dto)
    {
        var output = new QuizResponse(
            dto.Id,
            dto.Title,
            dto.ExhibitionId,
            dto.QuizLayout,
            dto.EndMessage,
            dto.Questions
            .Select(
                q => q.AsResponse())
            .ToList()
            );

        return output;
    }

    internal static UpdateQuizCommand AsUpdateCommand(this UpdateQuizRequest request, Guid id)
    {
        var output = new UpdateQuizCommand(
            id,
            request.Title,
            request.ExhibitionId,
            request.QuizLayout,
            request.Questions
            .Select(
                q => q.AsUpdateCommand())
            .ToList()
            );

        return output;
    }
}
