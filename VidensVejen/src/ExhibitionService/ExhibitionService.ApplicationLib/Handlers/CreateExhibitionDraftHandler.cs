using ExhibitionService.ApplicationLib.Repositories;
using ExhibitionService.DomainLib.Entities;
using ExhibitionService.FacadeLib.Commands.DTOs;
using ExhibitionService.FacadeLib.Commands.Interfaces;
using ExhibitionService.DomainLib.ValueObjects;
using VidensVejen.SharedKernelLib.Exceptions;
using ExhibitionService.ApplicationLib.Helpers;

namespace ExhibitionService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
/// <param name="exhibitionDraftRepo"></param>
/// <param name="exhibitionRepo"></param>
public class CreateExhibitionDraftHandler(
    IExhibitionDraftRepository exhibitionDraftRepo,
    IExhibitionRepository exhibitionRepo) : ICreateExhibitionDraftHandler
{
    Task<Guid> ICreateExhibitionDraftHandler.HandleAsync()
    {
        return SaveAsync(ExhibitionDraft.Create());
    }

    async Task<Guid> ICreateExhibitionDraftHandler.CreateRevisionAsync(UpdateExhibitionDraftCommand content)
    {
        var id = new ExhibitionId(content.Id);

        var exhibition = await exhibitionRepo.GetExhibitionById(id)
            ?? throw new NotFoundException("Exhibition was not found.");

        var draft = ExhibitionDraft.CreateRevision(exhibition.Id);
        HandlerHelper.Apply(draft, content);
        return await SaveAsync(draft);
    }

    private async Task<Guid> SaveAsync(ExhibitionDraft exhibitionDraft)
    {
        await exhibitionDraftRepo.AddAsync(exhibitionDraft);
        await exhibitionDraftRepo.SaveAsync();

        return exhibitionDraft.Id.Value;
    }
}
