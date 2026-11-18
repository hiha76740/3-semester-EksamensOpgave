using ImageService.FacadeLib.Commands.Interfaces;

namespace ImageService.ApplicationLib.Handlers;


/// <summary>
/// 
/// </summary>
public class DeleteImageHandler() : IDeleteImageHandler
{
    Task IDeleteImageHandler.HandleAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
