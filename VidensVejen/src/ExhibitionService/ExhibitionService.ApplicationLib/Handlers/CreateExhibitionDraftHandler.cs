using ExhibitionService.ApplicationLib.Repositories;
using ExhibitionService.DomainLib.Entities;
using ExhibitionService.FacadeLib.Commands.Interfaces;

namespace ExhibitionService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
/// <param name="exhibitionDraftRepo"></param>
public class CreateExhibitionDraftHandler(
    IExhibitionDraftRepository exhibitionDraftRepo) : ICreateExhibitionDraftHandler
{
    async Task<Guid> ICreateExhibitionDraftHandler.HandleAsync()
    {
        var exhibitionDraft = ExhibitionDraft.Create();

        await exhibitionDraftRepo.AddAsync(exhibitionDraft);
        await exhibitionDraftRepo.SaveAsync();

        return exhibitionDraft.Id.Value;
    }
}
