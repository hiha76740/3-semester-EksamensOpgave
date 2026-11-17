using ExhibitionService.FacadeLib.Commands.DTOs;
using ExhibitionService.FacadeLib.Queries.DTOs;
using VidensVejen.ContractsLib.Requests.ExhibitionService;
using VidensVejen.ContractsLib.Responses.ExhibitionService;

namespace ExhibitionService.Api.Mappers;

internal static class ExhibitionMapper
{
    internal static ExhibitionResponse AsResponse(this ExhibitionDto dto)
    {
        var output = new ExhibitionResponse(
            dto.Id,
            dto.Title,
            // TODO: Insert correct url when made
            "",
            dto.Subjects
            .Select(
                s => s.AsResponse())
            .ToList()
            );

        return output;
    }

    internal static UpdateExhibitionCommand AsUpdateCommand(this UpdateExhibitionRequest request, Guid id)
    {
        var output = new UpdateExhibitionCommand(
            id,
            request.ExhibitionName,
            request.Subjects
            .Select(
                s => s.AsUpdateCommand())
            .ToList()
            );

        return output;
    }

    internal static CreateExhibitionCommand AsCreateCommand(this CreateExhibitionRequest request, Stream imageStream, string extension)
    {
        var output = new CreateExhibitionCommand(
            request.ExhibitionName,
            imageStream,
            extension,
            request.Subjects
            .Select(
                s => s.AsCreateCommand())
            .ToList()
            );

        return output;
    }

    internal static ExhibitionNameResponse AsNameResponse(this ExhibitionNameDto dto)
    {
        var output = new ExhibitionNameResponse(
            dto.Name
            );

        return output;
    }
}
