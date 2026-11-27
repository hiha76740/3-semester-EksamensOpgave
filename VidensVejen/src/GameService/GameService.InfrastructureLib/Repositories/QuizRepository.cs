using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities.Quizzes;
using GameService.DomainLib.ValueObjects;
using GameService.InfrastructureLib.Persistence;
using Microsoft.EntityFrameworkCore;
using VidensVejen.SharedKernelLib.Exceptions;

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

    void IQuizRepository.Delete(Quiz quiz)
    {
        db.Remove(quiz);
    }

    async Task<IReadOnlyList<Quiz>> IQuizRepository.GetAllForExhibitionAsync(ExhibitionId id)
    {
        return await db.Quizzes
            .Include(q => q.Questions)
            .Where(q => q.ExhibitionId == id)
            .ToListAsync();
    }

    async Task<Quiz?> IQuizRepository.GetQuizByIdAsync(QuizId id)
    {
        return await db.Quizzes
            .Include(q => q.Questions)
            .FirstOrDefaultAsync(q => q.Id == id);
    }

    async Task IQuizRepository.SaveAsync()
    {
        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyConflictException("The data was changed or deleted by another request", ex);
        }
    }

    void IQuizRepository.SetOriginalRowVersion(Quiz quiz, byte[] originalRowVersion)
    {
        db.Entry(quiz)
            .Property(u => u.RowVersion)
            .OriginalValue = originalRowVersion;
    }
}
