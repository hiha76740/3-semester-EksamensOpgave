using ExhibitionService.FacadeLib.Commands.DTOs;
using ExhibitionService.FacadeLib.Queries.DTOs;
using VidensVejen.ContractsLib.Requests.ExhibitionService.Drafts;
using VidensVejen.ContractsLib.Responses.ExhibitionService;

namespace ExhibitionService.Api.Mappers;

internal static class SubjectDraftMapper
{
    internal static SubjectDraftResponse AsResponse(this SubjectDraftDto dto)
    {
        var output = new SubjectDraftResponse(
            dto.Id,
            dto.Title,
            dto.TitleEn,
            dto.Slug,
            dto.Description,
            dto.DescriptionEn,
            dto.Topics
            .Select(
                t => t.AsResponse())
            .ToList()
            );

        return output;
    }

    internal static UpdateSubjectDraftCommand AsUpdateDraftCommand(this UpdateSubjectDraftRequest request)
    {
        var output = new UpdateSubjectDraftCommand(
            request.Id,
            request.Title,
            request.TitleEn,
            request.Slug,
            request.Description,
            request.DescriptionEn,
            request.TopicDrafts
            .Select(
                td => td.AsUpdateDraftCommand())
            .ToList()
            );

        return output;
    }
}
