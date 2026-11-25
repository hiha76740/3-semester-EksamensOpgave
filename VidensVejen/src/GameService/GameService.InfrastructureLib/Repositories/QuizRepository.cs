using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities.Quizzes;
using GameService.InfrastructureLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GameService.InfrastructureLib.Repositories;

/// <summary>
/// 
/// </summary>
/// <param name="db"></param>
public class QuizRepository(GameDbContext db) : IQuizRepository
{
    async Task IQuizRepository.AddAsync(Quiz quiz)
    {
        await db.AddAsync(quiz);
    }

    async Task<Quiz?> IQuizRepository.GetQuizByIdAsync(QuizId id)
    {
        return await db.Quizzes
            .Include(q => q.Questions)
            .FirstOrDefaultAsync(q => q.Id == id);
    }

    async Task IQuizRepository.SaveAsync()
    {
        await db.SaveChangesAsync();
    }
}
