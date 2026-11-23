using ImageService.FacadeLib.Commands.DTOs;
using VidensVejen.ContractsLib.Requests.ImageService;

namespace ImageService.Api.Mappers;


internal static class ImageMapper
{
    internal static DeleteImageCommand AsDeleteCommand(this DeleteImageRequest request, Guid id)
    {
        var output = new DeleteImageCommand(
            id,
            request.RowVersion
            );

        return output;
    }
}
