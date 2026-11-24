using GameService.FacadeLib.Queries.DTOs;
using VidensVejen.ContractsLib.Responses.GameService;

namespace GameService.Api.Mappers.Sessions;

internal static class SessionMapper
{
    internal static StartSessionResponse AsResponse(this StartSessionDto dto)
    {
        var output = new StartSessionResponse(
            dto.SessionId,
            dto.Quiz.AsResponse()
            );

        return output;
    }

    internal static PlayQuizResponse AsResponse(this PlayQuizDto dto)
    {
        var output = new PlayQuizResponse(
            dto.Id,
            dto.Title,
            dto.QuizLayout,
            dto.EndMessage,
            dto.Questions
            .Select(
                q => q.AsResponse())
            .ToList()
            );

        return output;
    }

    internal static PlayQuestionResponse AsResponse(this PlayQuestionDto dto)
    {
        var output = new PlayQuestionResponse(
            dto.Id,
            dto.Text,
            dto.Choices
            .Select(
                c => c.AsResponse())
            .ToList()
            );

        return output;
    }

    internal static PlayChoiceResponse AsResponse(this PlayChoiceDto dto)
    {
        var output = new PlayChoiceResponse(
            dto.Index,
            dto.Text
            );

        return output;
    }
}
