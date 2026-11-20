using ExhibitionService.ApplicationLib.Repositories;
using ExhibitionService.DomainLib.Entities;
using ExhibitionService.InfrastructureLib.Persistence;

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

    async Task IExhibitionRepository.SaveAsync()
    {
        await db.SaveChangesAsync();
    }
}
