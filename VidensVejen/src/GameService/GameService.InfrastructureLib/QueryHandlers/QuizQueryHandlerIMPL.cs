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

        return await db.Quizzes
            .AsNoTracking()
            .Where(q => q.Id == quizKey)
            .Select(q => new QuizDto(
            q.Id.Value,
            q.Title,
            q.TitleEn,
            q.ExhibitionId.Value,
            q.QuizLayout,
            q.EndMessage,
            q.EndMessageEn,
            q.Questions
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
            q.Instruction,
            q.InstructionEn
            ))
            .FirstOrDefaultAsync();
    }

    async Task<List<QuizDto>> IQuizQueries.GetQuizzesByExhibitionAsync(Guid exhibitionId)
    {
        var exhibitionKey = new ExhibitionId(exhibitionId);

        return await db.Quizzes
            .AsNoTracking()
            .Where(q => q.ExhibitionId == exhibitionKey)
            .Select(q => new QuizDto(
            q.Id.Value,
            q.Title,
            q.TitleEn,
            q.ExhibitionId.Value,
            q.QuizLayout,
            q.EndMessage,
            q.EndMessageEn,
            q.Questions
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
            q.Instruction,
            q.InstructionEn
            ))
            .ToListAsync();
    }
}
