using ExhibitionService.FacadeLib.Commands.Interfaces;

namespace ExhibitionService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
public class DeleteExhibitionHandler : IDeleteExbitionHandler
{
    Task IDeleteExbitionHandler.HandleAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
