using ExhibitionService.FacadeLib.Commands.DTOs;
using ExhibitionService.FacadeLib.Queries.DTOs;
using VidensVejen.ContractsLib.Requests.ExhibitionService.Exhibitions;
using VidensVejen.ContractsLib.Responses.ExhibitionService;

namespace ExhibitionService.Api.Mappers;

internal static class SubjectMapper
{
    internal static SubjectResponse AsResponse(this SubjectDto dto)
    {
        var output = new SubjectResponse(
            dto.Id,
            dto.Slug,
            dto.Title,
            dto.Description,
            dto.QuizId,
            dto.Topics
            .Select(
                t => t.AsResponse())
            .ToList()
            );

        return output;
    }

    internal static UpdateSubjectCommand AsUpdateCommand(this UpdateSubjectRequest request)
    {
        var output = new UpdateSubjectCommand(
            request.Id,
            request.Title,
            request.Description,
            request.Topics
            .Select(
                t => t.AsUpdateCommand())
            .ToList()
            );

        return output;
    }

    internal static CreateSubjectCommand AsCreateCommand(this CreateSubjectRequest request)
    {
        var output = new CreateSubjectCommand(
            request.Title,
            request.TitleEn,
            request.Slug,
            request.Description,
            request.DescriptionEn,
            request.QuizId,
            request.Topics
            .Select(
                t => t.AsCreateCommand())
            .ToList()
            );

        return output;
    }
}
