using ExhibitionService.ApplicationLib.Repositories;
using ExhibitionService.DomainLib.ValueObjects;
using ExhibitionService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging;
using VidensVejen.SharedKernelLib.Exceptions;

namespace ExhibitionService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
public class DeleteExhibitionDraftHandler(
    IExhibitionDraftRepository exhibitionDraftRepo,
    ILogger<DeleteExhibitionDraftHandler> logger) : IDeleteExhibitionDraftHandler
{
    async Task IDeleteExhibitionDraftHandler.HandleAsync(Guid id)
    {
        try
        {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Beginning deletion of exhibition draft {ExhibitionDraftId}", id);


            var exhibitionDraftId = new ExhibitionDraftId(id);

            var exhibition = await exhibitionDraftRepo.GetExhibitionDraftByIdAsync(exhibitionDraftId)
                ?? throw new NotFoundException("Exhibition draft not found");


            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Deleting exhibition draft from exhibition database");


            exhibitionDraftRepo.Delete(exhibition);

            await exhibitionDraftRepo.SaveAsync();

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Exhibition draft {Id} was successfully deleted from exhibition database", exhibition.Id.Value);
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
