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
public class ExhibitionRepository(ExhibitionDbContext db) : IExhibitionRepository
{

    async Task IExhibitionRepository.AddAsync(Exhibition exhibition)
    {
        await db.Exhibitions.AddAsync(exhibition);
    }

    void IExhibitionRepository.Delete(Exhibition exhibition)
    {
        db.Exhibitions.Remove(exhibition);
    }

    async Task<Exhibition?> IExhibitionRepository.GetExhibitionById(ExhibitionId id)
    {
        return await db.Exhibitions
            .Include(e => e.Subjects)
            .FirstOrDefaultAsync();
    }

    async Task IExhibitionRepository.SaveAsync()
    {
        await db.SaveChangesAsync();
    }
}
