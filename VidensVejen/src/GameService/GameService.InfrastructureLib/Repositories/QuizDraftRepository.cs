using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities.Drafts;
using GameService.InfrastructureLib.Persistence;
using Microsoft.EntityFrameworkCore;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.InfrastructureLib.Repositories;

/// <summary>
/// 
/// </summary>
/// <param name="db"></param>
public class QuizDraftRepository(GameDbContext db) : IQuizDraftRepository
{
    async Task IQuizDraftRepository.AddAsync(QuizDraft draft)
    {
        await db.QuizDrafts.AddAsync(draft);
    }

    async Task<QuizDraft?> IQuizDraftRepository.GetDraftByIdAsync(QuizDraftId id)
    {
        return await db.QuizDrafts
            .Include(d => d.Questions)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    void IQuizDraftRepository.Delete(QuizDraft draft)
    {
        db.QuizDrafts.Remove(draft);
    }

    async Task IQuizDraftRepository.SaveAsync()
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

    void IQuizDraftRepository.SetOriginalRowVersion(QuizDraft draft, byte[] originalRowVersion)
    {
        db.Entry(draft)
            .Property(u => u.RowVersion)
            .OriginalValue = originalRowVersion;
    }
}
