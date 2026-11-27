
using ExhibitionService.DomainLib.ValueObjects;
using ExhibitionService.FacadeLib.Queries.DTOs;
using ExhibitionService.FacadeLib.Queries.Interfaces;
using ExhibitionService.InfrastructureLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExhibitionService.InfrastructureLib.QueryHandlers;

/// <summary>
/// Provides query operations for retrieving exhibition drafts.
/// </summary>
/// <param name="db">The database context used to query exhibition drafts.</param>
public class ExhibitionDraftQueryHandlerIMPL(ExhibitionDbContext db) : IExhibitionDraftQueries
{
    async Task<IReadOnlyList<ExhibitionDraftDto>> IExhibitionDraftQueries.GetAllExhibitionDrafts()
    {
        return await db.ExhibitionDrafts
                .AsNoTracking()
                .Select(ed => new ExhibitionDraftDto(
                    ed.Id.Value,
                    ed.Name,
                    ed.NameEn,
                    ed.SubjectDrafts
                        .OrderBy(sd => sd.Position)
                        .Select(sd => new SubjectDraftDto(
                            sd.Id.Value,
                            sd.Title,
                            sd.TitleEn,
                            sd.Slug,
                            sd.Description,
                            sd.DescriptionEn,
                            sd.QuizId == null ? null : sd.QuizId.Value,
                            sd.QuizDraftId == null ? null : sd.QuizDraftId.Value,
                            sd.IsQuiz,
                            sd.TopicDrafts
                                .OrderBy(td => td.Position)
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
                    ed.ThemeImageId == null ? null : ed.ThemeImageId.Value,
                    ed.SourceExhibitionId == null ? null : ed.SourceExhibitionId.Value
                    ))
                .ToListAsync();
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
                        .OrderBy(sd => sd.Position)
                        .Select(sd => new SubjectDraftDto(
                            sd.Id.Value,
                            sd.Title,
                            sd.TitleEn,
                            sd.Slug,
                            sd.Description,
                            sd.DescriptionEn,
                            sd.QuizId == null ? null : sd.QuizId.Value,
                            sd.QuizDraftId == null ? null : sd.QuizDraftId.Value,
                            sd.IsQuiz,
                            sd.TopicDrafts
                                .OrderBy(td => td.Position)
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
                    ed.ThemeImageId == null ? null : ed.ThemeImageId.Value,
                    ed.SourceExhibitionId == null ? null : ed.SourceExhibitionId.Value
                    ))
                .FirstOrDefaultAsync();
    }
}
