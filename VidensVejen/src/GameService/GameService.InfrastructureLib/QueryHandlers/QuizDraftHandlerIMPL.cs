using GameService.DomainLib.Entities.Drafts;
using GameService.FacadeLib.Queries.DTOs;
using GameService.FacadeLib.Queries.Interfaces;
using GameService.InfrastructureLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GameService.InfrastructureLib.QueryHandlers;

internal class QuizDraftHandlerIMPL(GameDbContext db) : IQuizDraftQueries
{
    async Task<QuizDraftDto?> IQuizDraftQueries.GetQuizDraftByIdAsync(Guid id)
    {
        var draftKey = new QuizDraftId(id);

        return await db.QuizDrafts
            .AsNoTracking()
            .Where(d => d.Id == draftKey)
            .Select(d => new QuizDraftDto(
                d.Id.Value,
                d.Title,
                d.TitleEn,
                d.ExhibitionId!.Value,
                d.QuizLayout,
                d.EndMessage,
                d.EndMessageEn,
                d.Questions
                    .Select(q => new DraftQuestionDto(
                        q.Id.Value,
                        q.Text,
                        q.TextEn,
                        q.Choices
                            .Select(c => new ChoiceDraftDto(
                                c.Text,
                                c.TextEn,
                                c.IsCorrect,
                                c.Feedback,
                                c.FeedbackEn
                                ))
                            .ToList()))
                    .ToList(),
                d.Instruction,
                d.InstructionEn
                ))
            .FirstOrDefaultAsync();
    }

    async Task<List<QuizDraftDto>> IQuizDraftQueries.GetQuizDraftsAsync()
    {
        return await db.QuizDrafts
            .AsNoTracking()
            .Select(d => new QuizDraftDto(
                d.Id.Value,
                d.Title,
                d.TitleEn,
                d.ExhibitionId!.Value,
                d.QuizLayout,
                d.EndMessage,
                d.EndMessageEn,
                d.Questions
                    .Select(q => new DraftQuestionDto(
                        q.Id.Value,
                        q.Text,
                        q.TextEn,
                        q.Choices
                            .Select(c => new ChoiceDraftDto(
                                c.Text,
                                c.TextEn,
                                c.IsCorrect,
                                c.Feedback,
                                c.FeedbackEn
                                ))
                            .ToList()))
                    .ToList(),
                d.Instruction,
                d.InstructionEn
                ))
            .ToListAsync();
    }
}
