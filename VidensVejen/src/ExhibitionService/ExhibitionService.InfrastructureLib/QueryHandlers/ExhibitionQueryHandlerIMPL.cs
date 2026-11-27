using ExhibitionService.DomainLib.ValueObjects;
using ExhibitionService.FacadeLib.Queries.DTOs;
using ExhibitionService.FacadeLib.Queries.Interfaces;
using ExhibitionService.InfrastructureLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExhibitionService.InfrastructureLib.QueryHandlers;

/// <summary>
/// Provides database-backed queries for exhibitions.
/// </summary>
/// <param name="db">The exhibition database context.</param>
public class ExhibitionQueryHandlerIMPL(ExhibitionDbContext db) : IExhibitionQueries
{
    async Task<bool> IExhibitionQueries.CheckIfImageInUseAsync(Guid id)
    {
        var imageId = new ImageId(id);

        return await db.Exhibitions
            .AsNoTracking()
            .AnyAsync(e => e.ThemeImageId == imageId);
    }

    async Task<bool> IExhibitionQueries.CheckIfQuizInUseAsync(Guid id)
    {
        var quizId = new QuizId(id);

        return await db.Subjects.AnyAsync(s => s.QuizId == quizId);
    }

    async Task<IReadOnlyList<ExhibitionNameDto>> IExhibitionQueries.GetAllExhibitionNamesAsync()
    {
        return await db.Exhibitions
            .AsNoTracking()
            .Select(e => new ExhibitionNameDto(
                e.Id.Value,
                e.Name))
            .ToListAsync();
    }

    async Task<ExhibitionDto?> IExhibitionQueries.GetExhibitionByIdAsync(Guid id)
    {
        var exhibitionId = new ExhibitionId(id);

        return await db.Exhibitions
            .AsNoTracking()
            .Where(e => e.Id == exhibitionId)
            .Select(e => new ExhibitionDto(
                e.Id.Value,
                e.Name,
                e.NameEn,
                e.Subjects.Select(s => new SubjectDto(
                    s.Id.Value,
                    s.Slug,
                    s.Title,
                    s.TitleEn,
                    s.Description,
                    s.DescriptionEn,
                    s.QuizId == null ? null : s.QuizId.Value,
                    s.Topics.Select(t => new TopicDto(
                        t.Title,
                        t.TitleEn,
                        t.Description,
                        t.DescriptionEn,
                        t.Body,
                        t.BodyEn,
                        t.CalloutLabel,
                        t.CalloutLabelEn,
                        t.CalloutText,
                        t.CalloutLabelEn,
                        t.Quote,
                        t.QuoteEn
                        ))
                    .ToList()))
                .ToList(),
                e.ThemeImageId.Value))
            .FirstOrDefaultAsync();
    }
}
