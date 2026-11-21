using ExhibitionService.FacadeLib.Commands.DTOs;
using ExhibitionService.FacadeLib.Commands.Interfaces;
using ExhibitionService.FacadeLib.Queries.DTOs;

namespace ExhibitionService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
public class UpdateExhibitionHandler : IUpdateExhibitionHandler
{
    Task<ExhibitionDto> IUpdateExhibitionHandler.HandleAsync(UpdateExhibitionCommand command)
    {
        throw new NotImplementedException();
    }
}
