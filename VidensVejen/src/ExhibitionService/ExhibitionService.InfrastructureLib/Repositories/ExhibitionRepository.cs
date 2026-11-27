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
            .FirstOrDefaultAsync(exhibition => exhibition.Id == id);
    }

    async Task IExhibitionRepository.SaveAsync()
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

    void IExhibitionRepository.SetOriginalRowVersion(Exhibition exhibition, byte[] originalRowVersion)
    {
        db.Entry(exhibition)
            .Property(u => u.RowVersion)
            .OriginalValue = originalRowVersion;
    }
}
