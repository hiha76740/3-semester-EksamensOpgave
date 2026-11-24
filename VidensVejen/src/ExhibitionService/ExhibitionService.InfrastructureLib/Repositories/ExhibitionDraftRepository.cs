using ExhibitionService.ApplicationLib.Repositories;
using ExhibitionService.DomainLib.Entities;
using ExhibitionService.InfrastructureLib.Persistence;

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

    async Task IExhibitionDraftRepository.SaveAsync()
    {
        await db.SaveChangesAsync();
    }
}
