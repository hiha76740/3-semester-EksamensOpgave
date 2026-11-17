using ExhibitionService.FacadeLib.Commands.DTOs;
using ExhibitionService.FacadeLib.Queries.DTOs;
using VidensVejen.ContractsLib.Requests.ExhibitionService;
using VidensVejen.ContractsLib.Responses.ExhibitionService;

namespace ExhibitionService.Api.Mappers;

internal static class TopicMapper
{
    internal static TopicResponse AsResponse(this TopicDto dto)
    {
        var output = new TopicResponse(
            dto.Id,
            dto.Title,
            dto.Description,
            dto.Body,
            dto.CalloutLabel,
            dto.CalloutText,
            dto.Quote
            );

        return output;
    }

    internal static UpdateTopicCommand AsUpdateCommand(this UpdateTopicRequest request)
    {
        var output = new UpdateTopicCommand(
            request.Id,
            request.Title,
            request.Description,
            request.Body,
            request.CalloutLabel,
            request.CalloutText,
            request.Qoute
            );

        return output;
    }

    internal static CreateTopicCommand AsCreateCommand(this CreateTopicRequest request)
    {
        var output = new CreateTopicCommand(
            request.Title,
            request.Description,
            request.Body,
            request.CalloutLabel,
            request.CalloutText,
            request.Qoute
            );

        return output;
    }
}
