using GameService.FacadeLib.Queries.DTOs;
using VidensVejen.ContractsLib.Responses.GameService;
using GameService.FacadeLib.Commands.DTOs;
using VidensVejen.ContractsLib.Requests.GameService;

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

    internal static AnswerQuestionCommand AsCommand(this AnswerQuestionRequest request, Guid sessionId)
    {
        var output = new AnswerQuestionCommand(
            sessionId,
            request.QuestionId,
            request.ChoiceIndex
            );

        return output;
    }

    internal static AnswerResultResponse AsResponse(this AnswerResultDto dto)
    {
        var output = new AnswerResultResponse(
            dto.IsCorrect,
            dto.Feedback,
            dto.CorrectAnswer
            );

        return output;
    }
}
