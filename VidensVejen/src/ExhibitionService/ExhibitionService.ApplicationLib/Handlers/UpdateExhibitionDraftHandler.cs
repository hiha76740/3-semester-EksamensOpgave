using ExhibitionService.ApplicationLib.Helpers;
using ExhibitionService.ApplicationLib.Repositories;
using ExhibitionService.DomainLib.ValueObjects;
using ExhibitionService.FacadeLib.Commands.DTOs;
using ExhibitionService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging;
using VidensVejen.SharedKernelLib.Exceptions;

namespace ExhibitionService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
/// <param name="exhibitionDraftRepo"></param>
/// <param name="logger"></param>
public class UpdateExhibitionDraftHandler(
    IExhibitionDraftRepository exhibitionDraftRepo,
    ILogger<UpdateExhibitionDraftHandler> logger) : IUpdateExhibitionDraftHandler
{
    async Task IUpdateExhibitionDraftHandler.HandleAsync(UpdateExhibitionDraftCommand command)
    {
        try
        {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Beginning to update exhibition draft {Id}", command.Id);

            var id = new ExhibitionDraftId(command.Id);

            var exhibitionDraft = await exhibitionDraftRepo.GetExhibitionDraftByIdAsync(id)
                ?? throw new NotFoundException("Exhibition draft not found");

            HandlerHelper.Apply(exhibitionDraft, command);

            exhibitionDraftRepo.SetOriginalRowVersion(exhibitionDraft, command.RowVersion);

            await exhibitionDraftRepo.SaveAsync();

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Exhibition draft {Id} successfully updated ", exhibitionDraft.Id.Value);
        }
        catch (Exception ex)
        {
            if (logger.IsEnabled(LogLevel.Error))
            {
                logger.LogError("Error {ErrorMessage}", ex.Message);

                if (ex.InnerException != null)
                    logger.LogError("InnerException Error {Error}", ex.InnerException.Message);
            }

            throw;
        }
    }
}
