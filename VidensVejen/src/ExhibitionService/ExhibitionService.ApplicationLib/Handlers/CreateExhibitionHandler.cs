using ExhibitionService.FacadeLib.Commands.DTOs;
using ExhibitionService.FacadeLib.Commands.Interfaces;

namespace ExhibitionService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
public class CreateExhibitionHandler : ICreateExhibitionHandler
{
    Task ICreateExhibitionHandler.HandleAsync(CreateExhibitionCommand command)
    {
        throw new NotImplementedException();
    }
}
