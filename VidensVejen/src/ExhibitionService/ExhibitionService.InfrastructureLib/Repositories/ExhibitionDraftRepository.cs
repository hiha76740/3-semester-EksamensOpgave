using ExhibitionService.ApplicationLib.Repositories;
using ExhibitionService.DomainLib.Entities;
using ExhibitionService.DomainLib.ValueObjects;
using ExhibitionService.InfrastructureLib.Persistence;
using Microsoft.EntityFrameworkCore;
using VidensVejen.SharedKernelLib.Exceptions;

namespace ExhibitionService.InfrastructureLib.Repositories;

/// <summary>
/// 
/// </summary>
/// <param name="db"></param>
public class ExhibitionDraftRepository(ExhibitionDbContext db) : IExhibitionDraftRepository
{
    async Task IExhibitionDraftRepository.AddAsync(ExhibitionDraft exhibitionDraft)
    {
        await db.ExhibitionDrafts.AddAsync(exhibitionDraft);
    }

    void IExhibitionDraftRepository.Delete(ExhibitionDraft exhibitionDraft)
    {
        db.ExhibitionDrafts.Remove(exhibitionDraft);
    }

    async Task<ExhibitionDraft?> IExhibitionDraftRepository.GetExhibitionDraftByIdAsync(ExhibitionDraftId id)
    {
        return await db.ExhibitionDrafts
            .Include(ed => ed.SubjectDrafts)
            .FirstOrDefaultAsync(ed => ed.Id == id);
    }

    async Task IExhibitionDraftRepository.SaveAsync()
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

    void IExhibitionDraftRepository.SetOriginalRowVersion(ExhibitionDraft exhibitionDraft, byte[] originalRowVersion)
    {
        db.Entry(exhibitionDraft)
            .Property(u => u.RowVersion)
            .OriginalValue = originalRowVersion;
    }
}
