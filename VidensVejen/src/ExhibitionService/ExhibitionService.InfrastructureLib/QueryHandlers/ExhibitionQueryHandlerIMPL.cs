using ExhibitionService.FacadeLib.Queries.DTOs;
using ExhibitionService.FacadeLib.Queries.Interfaces;

namespace ExhibitionService.InfrastructureLib.QueryHandlers;

/// <summary>
/// 
/// </summary>
public class ExhibitionQueryHandlerIMPL : IExhibitionQueries
{
    Task<IReadOnlyList<ExhibitionNameDto>> IExhibitionQueries.GetAllExhibitionNamesAsync()
    {
        throw new NotImplementedException();
    }

    Task<ExhibitionDto?> IExhibitionQueries.GetExhibitionByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
