using ExhibitionService.ApplicationLib.Repositories;
using ExhibitionService.ApplicationLib.Services;
using ExhibitionService.DomainLib.Entities;
using ExhibitionService.DomainLib.ValueObjects;
using ExhibitionService.FacadeLib.Commands.DTOs;
using ExhibitionService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging;

namespace ExhibitionService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
/// <param name="exhibitionRepo"></param>
/// <param name="imageService"></param>
/// <param name="logger"></param>
public class CreateExhibitionHandler(
    IExhibitionRepository exhibitionRepo,
    IImageService imageService,
    ILogger<CreateExhibitionHandler> logger) : ICreateExhibitionHandler
{
    async Task ICreateExhibitionHandler.HandleAsync(CreateExhibitionCommand command)
    {
        try
        {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Beginning to create exhibition with name {ExhibitionName}", command.ExhibtionName);

            var imageId = new ImageId(command.ThemeImageId);

            var imageExists = await imageService.ExistsAsync(imageId);

            if (imageExists == false)
                throw new InvalidOperationException("Creation aborted, Theme image not found");

            var subjects = new List<NewSubject>();

            foreach (var subjectCommand in command.Subjects)
            {
                var topics = new List<NewTopic>();

                foreach (var topicCommand in subjectCommand.Topics)
                {
                    topics.Add(new NewTopic(
                        topicCommand.Title,
                        topicCommand.Description,
                        topicCommand.Body,
                        topicCommand.CalloutLabel,
                        topicCommand.CalloutText,
                        topicCommand.Qoute
                        ));
                }

                subjects.Add(new NewSubject(
                    subjectCommand.Title,
                    subjectCommand.Slug,
                    subjectCommand.Description,
                    topics));
            }

            var exhibition = Exhibition.Create(
                command.ExhibtionName,
                imageId,
                subjects);

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Exhibition created with id: {ExhibitionId}", exhibition.Id.Value);

            await exhibitionRepo.AddAsync(exhibition);
            await exhibitionRepo.SaveAsync();

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Exhibition {Id} saved in database successfully", exhibition.Id.Value);
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
