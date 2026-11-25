using GameService.FacadeLib.Queries.DTOs;
using VidensVejen.ContractsLib.Responses.GameService;

namespace GameService.Api.Mappers.Quizzes;

internal static class QuizDraftMapper
{
    internal static QuizDraftResponse AsResponse(this QuizDraftDto dto)
    {
        var output = new QuizDraftResponse(
            dto.Id,
            dto.Title,
            dto.TitleEn,
            dto.ExhibitionId,
            dto.QuizLayout,
            dto.EndMessage,
            dto.EndMessageEn,
            dto.Questions
            .Select(
                q => q.AsResponse())
            .ToList()
            );

        return output;
    }

    internal static DraftQuestionResponse AsResponse(this DraftQuestionDto dto)
    {
        var output = new DraftQuestionResponse(
            dto.Id,
            dto.Text,
            dto.TextEn,
            dto.Choices
            .Select(
                c => c.AsResponse())
            .ToList()
            );

        return output;
    }
}
