using GameService.FacadeLib.Commands.DTOs;
using GameService.FacadeLib.Queries.DTOs;
using VidensVejen.ContractsLib.Requests.GameService.Drafts;
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
            .ToList(),
            dto.Instruction,
            dto.InstructionEn
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

    internal static ChoiceDraftResponse AsResponse(this ChoiceDraftDto dto)
    {
        var output = new ChoiceDraftResponse(
            dto.Text,
            dto.TextEn,
            dto.CorrectAnswer,
            dto.Feedback,
            dto.FeedbackEn
            );

        return output;
    }

    internal static UpdateQuizDraftCommand AsUpdateCommand(this UpdateQuizDraftRequest request, Guid draftId)
    {
        var output = new UpdateQuizDraftCommand(
            draftId,
            request.Title,
            request.TitleEn,
            request.ExhibitionId,
            request.QuizLayout,
            request.Questions?.Select(q => q.AsUpdateCommand()).ToList(),
            request.EndMessage,
            request.EndMessageEn,
            request.Instruction,
            request.InstructionEn
            );

        return output;
    }

    internal static DeleteDraftCommand AsDeleteCommand(this DeleteQuizDraftRequest request, Guid draftId)
    {
        var output = new DeleteDraftCommand(
            draftId,
            request.RowVersion
            );

        return output;
    }
}
