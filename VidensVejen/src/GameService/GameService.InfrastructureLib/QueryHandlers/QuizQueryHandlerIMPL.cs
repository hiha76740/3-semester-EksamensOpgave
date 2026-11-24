using GameService.DomainLib.Entities;
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

    private static QuizDto AsDto(Quiz quiz)
    {
        return new QuizDto(
            quiz.Id.Value,
            quiz.Title,
            quiz.ExhibitionId.Value,
            quiz.QuizLayout,
            quiz.EndMessage,
            quiz.Questions
            .Select(
                q => new QuestionDto(
                    q.Id.Value,
                    q.Text,
                    q.CorrectFeedback,
                    q.IncorrectFeedback,
                    q.Choices
                    .Select(
                        c => new ChoiceDto(
                            c.Text,
                            c.IsCorrect))
                    .ToList()))
            .ToList());
    }
}
