using ExhibitionService.FacadeLib.Commands.DTOs;
using ExhibitionService.FacadeLib.Queries.DTOs;
using VidensVejen.ContractsLib.Requests.ExhibitionService.Drafts;
using VidensVejen.ContractsLib.Responses.ExhibitionService;

namespace ExhibitionService.Api.Mappers;

internal static class ExhibitionDraftMapper
{
    internal static ExhibitionDraftResponse AsResponse(this ExhibitionDraftDto dto)
    {
        var output = new ExhibitionDraftResponse(
            dto.Id,
            dto.Name,
            dto.NameEn,
            dto.ThemeImageId,
            dto.SubjectDrafts
            .Select(
                sd => sd.AsResponse())
            .ToList(),
            dto.SourceExhibitionId
            );

        return output;
    }

    internal static UpdateExhibitionDraftCommand AsUpdateDraftCommand(this UpdateExhibitionDraftRequest request, Guid id)
    {
        var output = new UpdateExhibitionDraftCommand(
            id,
            request.Name,
            request.NameEn,
            request.ThemeImageId,
            request.SubjectDrafts
            .Select(
                sd => sd.AsUpdateDraftCommand())
            .ToList()
            );

        return output;
    }

    internal static DeleteExhibitionDraftCommand AsDeleteDraftCommand(this DeleteExhibitionDraftRequest request, Guid exhibitionDraftId)
    {
        var output = new DeleteExhibitionDraftCommand(
            exhibitionDraftId,
            request.RowVersion);

        return output;
    }
}
