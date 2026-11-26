using ExhibitionService.DomainLib.ValueObjects;
using ExhibitionService.FacadeLib.Queries.DTOs;
using ExhibitionService.FacadeLib.Queries.Interfaces;
using ExhibitionService.InfrastructureLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ExhibitionService.InfrastructureLib.QueryHandlers;

/// <summary>
/// 
/// </summary>
public class ExhibitionQueryHandlerIMPL(ExhibitionDbContext db) : IExhibitionQueries
{
    async Task<bool> IExhibitionQueries.CheckIfImageInUseAsync(Guid id)
    {
        var imageId = new ImageId(id);

        return await db.Exhibitions
            .AsNoTracking()
            .AnyAsync(e => e.ThemeImageId == imageId);
    }

    Task<bool> IExhibitionQueries.CheckIfQuizInUseAsync(Guid id)
    {
        // TODO: Fix dette når vi ved hvordan vi skal tilknytte Exhibition til en quiz
        throw new NotImplementedException();
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
                e.Subjects.Select(s => new SubjectDto(
                    s.Id.Value,
                    s.Slug,
                    s.Title,
                    s.Description,
                    s.Topics.Select(t => new TopicDto(
                        t.Title,
                        t.Description,
                        t.Body,
                        t.CalloutLabel,
                        t.CalloutText,
                        t.Quote))
                    .ToList()))
                .ToList(),
                e.ThemeImageId.Value))
            .FirstOrDefaultAsync();
    }
}
