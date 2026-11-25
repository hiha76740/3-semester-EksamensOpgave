using ExhibitionService.FacadeLib.Commands.DTOs;

namespace ExhibitionService.FacadeLib.Commands.Interfaces;

public interface IUpdateExhibitionDraftHandler
{
    Task HandleAsync(UpdateExhibitionDraftCommand command);
}
