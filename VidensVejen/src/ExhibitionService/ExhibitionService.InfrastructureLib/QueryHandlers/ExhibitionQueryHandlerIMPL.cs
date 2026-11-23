using ExhibitionService.DomainLib.ValueObjects;
using ExhibitionService.FacadeLib.Queries.DTOs;
using ExhibitionService.FacadeLib.Queries.Interfaces;
using ExhibitionService.InfrastructureLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExhibitionService.InfrastructureLib.QueryHandlers;

/// <summary>
/// 
/// </summary>
public class ExhibitionQueryHandlerIMPL(ExhibitionDbContext db) : IExhibitionQueries
{
    Task<bool> IExhibitionQueries.CheckIfInUseAsync(Guid id)
    {
        var imageId = new ImageId(id);

        return db.Exhibitions
            .AsNoTracking()
            .AnyAsync(e => e.Theme == imageId);
    }

    Task<IReadOnlyList<ExhibitionNameDto>> IExhibitionQueries.GetAllExhibitionNamesAsync()
    {
        throw new NotImplementedException();
    }

    Task<ExhibitionDto?> IExhibitionQueries.GetExhibitionByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
