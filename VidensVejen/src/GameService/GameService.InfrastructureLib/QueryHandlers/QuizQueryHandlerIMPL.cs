using GameService.DomainLib.Entities.Drafts;
using GameService.DomainLib.Entities.Quizzes;
using GameService.DomainLib.ValueObjects;
using GameService.FacadeLib.Queries.DTOs;
using GameService.FacadeLib.Queries.Interfaces;
using GameService.InfrastructureLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GameService.InfrastructureLib.QueryHandlers;

/// <summary>
/// 
/// </summary>
/// <param name="db"></param>
public class QuizQueryHandlerIMPL(GameDbContext db) : IQuizQueries
{
    async Task<QuizDto?> IQuizQueries.GetQuizByIdAsync(Guid id)
    {
        var quizKey = new QuizId(id);

        var quiz = await db.Quizzes
            .AsNoTracking()
            .Include(q => q.Questions)
            .FirstOrDefaultAsync(q => q.Id == quizKey);

        if (quiz == null)
            return null;

        return AsDto(quiz);
    }

    async Task<List<QuizDto>> IQuizQueries.GetQuizzesByExhibitionAsync(Guid exhibitionId)
    {
        var exhibitionKey = new ExhibitionId(exhibitionId);

        var quizzes = await db.Quizzes
            .AsNoTracking()
            .Include(q => q.Questions)
            .Where(q => q.ExhibitionId == exhibitionKey)
            .ToListAsync();

        return quizzes.Select(AsDto).ToList();
    }

    async Task<QuizDraftDto?> IQuizQueries.GetQuizDraftByIdAsync(Guid id)
    {
        var draftKey = new QuizDraftId(id);

        var draft = await db.QuizDrafts
            .AsNoTracking()
            .Include(d => d.Questions)
            .FirstOrDefaultAsync(d => d.Id == draftKey);

        if (draft == null)
            return null;

        return AsDraftDto(draft);
    }

    async Task<List<QuizDraftDto>> IQuizQueries.GetQuizDraftsAsync()
    {
        var drafts = await db.QuizDrafts
            .AsNoTracking()
            .Include(d => d.Questions)
            .ToListAsync();

        return drafts.Select(AsDraftDto).ToList();
    }

    private static QuizDto AsDto(Quiz quiz)
    {
        return new QuizDto(
            quiz.Id.Value,
            quiz.Title,
            quiz.TitleEn,
            quiz.ExhibitionId.Value,
            quiz.QuizLayout,
            quiz.EndMessage,
            quiz.EndMessageEn,
            quiz.Questions
            .Select(
                q => new QuestionDto(
                    q.Id.Value,
                    q.Text,
                    q.TextEn,
                    q.Choices
                    .Select(
                        c => new ChoiceDto(
                            c.Text,
                            c.TextEn,
                            c.IsCorrect,
                            c.Feedback,
                            c.FeedbackEn))
                    .ToList()))
            .ToList(),
            quiz.Instruction,
            quiz.InstructionEn
            );
    }

    private static QuizDraftDto AsDraftDto(QuizDraft draft)
    {
        return new QuizDraftDto(
            draft.Id.Value,
            draft.Title,
            draft.TitleEn,
            draft.ExhibitionId?.Value,
            draft.QuizLayout,
            draft.EndMessage,
            draft.EndMessageEn,
            draft.Questions
            .Select(
                q => new DraftQuestionDto(
                    q.Id.Value,
                    q.Text,
                    q.TextEn,
                    q.Choices
                    .Select(
                        c => new ChoiceDraftDto(
                            c.Text,
                            c.TextEn,
                            c.IsCorrect,
                            c.Feedback,
                            c.FeedbackEn))
                    .ToList()))
            .ToList());
    }
}
