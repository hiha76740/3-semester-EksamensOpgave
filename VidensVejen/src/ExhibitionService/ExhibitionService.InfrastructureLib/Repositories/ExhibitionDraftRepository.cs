using ExhibitionService.ApplicationLib.Repositories;
using ExhibitionService.DomainLib.Entities;
using ExhibitionService.DomainLib.ValueObjects;
using ExhibitionService.InfrastructureLib.Persistence;
using Microsoft.EntityFrameworkCore;

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
        await db.SaveChangesAsync();
    }
}
