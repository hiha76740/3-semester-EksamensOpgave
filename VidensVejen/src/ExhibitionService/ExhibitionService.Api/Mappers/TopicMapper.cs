using ExhibitionService.FacadeLib.Commands.DTOs;
using ExhibitionService.FacadeLib.Queries.DTOs;
using VidensVejen.ContractsLib.Requests.ExhibitionService.Exhibitions;
using VidensVejen.ContractsLib.Responses.ExhibitionService;

namespace ExhibitionService.Api.Mappers;

internal static class TopicMapper
{
    internal static TopicResponse AsResponse(this TopicDto dto)
    {
        var output = new TopicResponse(
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

    internal static CreateTopicCommand AsCreateCommand(this CreateTopicRequest request)
    {
        var output = new CreateTopicCommand(
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
            request.Qoute,
            request.QouteEn
            );

        return output;
    }
}
