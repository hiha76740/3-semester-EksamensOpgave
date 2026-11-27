using ExhibitionService.FacadeLib.Commands.DTOs;
using ExhibitionService.FacadeLib.Queries.DTOs;
using VidensVejen.ContractsLib.Requests.ExhibitionService.Exhibitions;
using VidensVejen.ContractsLib.Responses.ExhibitionService;

namespace ExhibitionService.Api.Mappers;

internal static class ExhibitionMapper
{
    internal static ExhibitionResponse AsResponse(this ExhibitionDto dto)
    {
        var output = new ExhibitionResponse(
            dto.Id,
            dto.Name,
            dto.NameEn,
            dto.ThemeImageId,
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

    internal static CreateExhibitionCommand AsCreateCommand(this CreateExhibitionRequest request)
    {
        var output = new CreateExhibitionCommand(
            request.Id,
            request.ExhibitionName,
            request.ExhibitionNameEn,
            request.ThemeImageId,
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
            dto.Id,
            dto.Name
            );

        return output;
    }

    internal static DeleteExhibitionCommand AsDeleteCommand(this DeleteExhibitionRequest request, Guid exhibitionId)
    {
        var output = new DeleteExhibitionCommand(
            exhibitionId,
            request.RowVersion);

        return output;
    }
}
