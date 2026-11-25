using ExhibitionService.FacadeLib.Commands.DTOs;
using ExhibitionService.FacadeLib.Queries.DTOs;
using VidensVejen.ContractsLib.Requests.ExhibitionService.Drafts;
using VidensVejen.ContractsLib.Responses.ExhibitionService;

namespace ExhibitionService.Api.Mappers;

internal static class TopicDraftMapper
{
    internal static TopicDraftResponse AsResponse(this TopicDraftDto dto)
    {
        var output = new TopicDraftResponse(
            dto.Title,
            dto.TitleEn,
            dto.Description,
            dto.DescriptionEn,
            dto.Body,
            dto.BodyEn,
            dto.CalloutLabel,
            dto.CalloutLabelEn,
            dto.CalloutText,
            dto.CalloutTextEn,
            dto.Quote,
            dto.QuoteEn
            );

        return output;
    }

    internal static UpdateTopicDraftCommand AsUpdateDraftCommand(this UpdateTopicDraftRequest request)
    {
        var output = new UpdateTopicDraftCommand(
            request.Title,
            request.TitleEn,
            request.Description,
            request.DescriptionEn,
            request.Body,
            request.BodyEn,
            request.CalloutLabel,
            request.CalloutLabelEn,
            request.CalloutText,
            request.CalloutTextEn,
            request.Quote,
            request.QuoteEn
            );

        return output;
    }
}
