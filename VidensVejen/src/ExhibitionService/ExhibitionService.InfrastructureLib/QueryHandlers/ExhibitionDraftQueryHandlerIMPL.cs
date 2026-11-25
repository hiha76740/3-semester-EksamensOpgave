
using ExhibitionService.DomainLib.ValueObjects;
using ExhibitionService.FacadeLib.Queries.DTOs;
using ExhibitionService.FacadeLib.Queries.Interfaces;
using ExhibitionService.InfrastructureLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExhibitionService.InfrastructureLib.QueryHandlers;

/// <summary>
/// 
/// </summary>
public class ExhibitionDraftQueryHandlerIMPL(ExhibitionDbContext db) : IExhibitionDraftQueries
{
    Task<IReadOnlyList<ExhibitionDraftDto>> IExhibitionDraftQueries.GetAllExhibitionDrafts()
    {
        throw new NotImplementedException();
    }

    async Task<ExhibitionDraftDto?> IExhibitionDraftQueries.GetExhibitionDraftByIdAsync(Guid id)
    {
        var exhibitionDraftId = new ExhibitionDraftId(id);

        return await db.ExhibitionDrafts
                .AsNoTracking()
                .Where(ed => ed.Id == exhibitionDraftId)
                .Select(ed => new ExhibitionDraftDto(
                    ed.Id.Value,
                    ed.Name,
                    ed.NameEn,
                    ed.SubjectDrafts
                        .Select(sd => new SubjectDraftDto(
                            sd.Id.Value,
                            sd.Title,
                            sd.TitleEn,
                            sd.Slug,
                            sd.Description,
                            sd.DescriptionEn,
                            sd.TopicDrafts
                                .Select(td => new TopicDraftDto(
                                    td.Title,
                                    td.TitleEn,
                                    td.Description,
                                    td.DescriptionEn,
                                    td.Body,
                                    td.BodyEn,
                                    td.CalloutLabel,
                                    td.CalloutLabelEn,
                                    td.CalloutText,
                                    td.CalloutTextEn,
                                    td.Quote,
                                    td.QuoteEn))
                                .ToList()))
                        .ToList(),
                    ed.ThemeImageId == null
                        ? null
                        : ed.ThemeImageId.Value))
                .FirstOrDefaultAsync();
    }
}
