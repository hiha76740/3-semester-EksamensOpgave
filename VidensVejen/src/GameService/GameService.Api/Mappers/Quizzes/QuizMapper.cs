using GameService.FacadeLib.Commands.DTOs;
using GameService.FacadeLib.Queries.DTOs;
using VidensVejen.ContractsLib.Requests.GameService.Quizzes;
using VidensVejen.ContractsLib.Responses.GameService;

namespace GameService.Api.Mappers.Quizzes;

internal static class QuizMapper
{
    internal static CreateQuizCommand AsCreateCommand(this CreateQuizRequest request, Guid draftId)
    {
        var output = new CreateQuizCommand(
            draftId,
            request.Title,
            request.TitleEn,
            request.ExhibitionId,
            request.QuizLayout,
            request.Questions
            .Select(
                q => q.AsCreateCommand())
            .ToList(),
             request.EndMessage,
             request.EndMessageEn,
             request.Instruction,
             request.InstructionEn
        );

        return output;
    }

    internal static QuizResponse AsResponse(this QuizDto dto)
    {
        var output = new QuizResponse(
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
            .ToList(),
            dto.Instruction,
            dto.InstructionEn
            );

        return output;
    }
}
