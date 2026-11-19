using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities;
using GameService.InfrastructureLib.Persistence;

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

    async Task IQuizRepository.SaveAsync()
    {
        await db.SaveChangesAsync();
    }
}
