using ImageService.FacadeLib.Commands.DTOs;
using ImageService.FacadeLib.Queries.DTOs;
using VidensVejen.ContractsLib.Requests.ImageService;
using VidensVejen.ContractsLib.Responses.ImageService;

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

    internal static ImageResponse AsResponse(this ImageDto dto, string url)
    {
        var output = new ImageResponse(
            dto.Id,
            dto.StorageKey,
            dto.Extension,
            url
            );

        return output;
    }
}
