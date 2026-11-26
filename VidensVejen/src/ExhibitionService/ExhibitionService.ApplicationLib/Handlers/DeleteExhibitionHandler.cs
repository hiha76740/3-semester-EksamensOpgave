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
public class DeleteExhibitionHandler(
    IExhibitionRepository exhibitionRepo,
    ILogger<DeleteExhibitionHandler> logger) : IDeleteExhibitionHandler
{
    async Task IDeleteExhibitionHandler.HandleAsync(DeleteExhibitionCommand command)
    {
        try
        {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Beginning deletion of exhibition {ExhibitionId}", command.ExhibitionId);


            var exhibitionId = new ExhibitionId(command.ExhibitionId);

            var exhibition = await exhibitionRepo.GetExhibitionById(exhibitionId)
                ?? throw new NotFoundException("Exhibition not found");


            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Deleting exhibition from exhibition database");


            exhibitionRepo.Delete(exhibition);

            await exhibitionRepo.SaveAsync();

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Exhibition {Id} was successfully deleted from exhibition database", exhibition.Id.Value);
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
