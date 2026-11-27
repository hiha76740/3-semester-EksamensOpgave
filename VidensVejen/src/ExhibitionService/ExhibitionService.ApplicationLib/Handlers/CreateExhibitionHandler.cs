using ExhibitionService.ApplicationLib.Repositories;
using ExhibitionService.ApplicationLib.Services;
using ExhibitionService.DomainLib.Entities;
using ExhibitionService.DomainLib.ValueObjects;
using ExhibitionService.FacadeLib.Commands.DTOs;
using ExhibitionService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging;
using VidensVejen.SharedKernelLib.Exceptions;

namespace ExhibitionService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
/// <param name="exhibitionRepo"></param>
/// <param name="exhibitionDraftRepo"></param>
/// <param name="imageService"></param>
/// <param name="logger"></param>
public class CreateExhibitionHandler(
    IExhibitionRepository exhibitionRepo,
    IExhibitionDraftRepository exhibitionDraftRepo,
    IImageService imageService,
    ILogger<CreateExhibitionHandler> logger) : ICreateExhibitionHandler
{
    Task ICreateExhibitionHandler.HandleAsync(CreateExhibitionCommand command)
    {
        return SavePublicationAsync(command, null, null);
    }

    async Task ICreateExhibitionHandler.PublishRevisionAsync(Guid draftId, CreateExhibitionCommand command)
    {
        var draft = await exhibitionDraftRepo.GetExhibitionDraftByIdAsync(new ExhibitionDraftId(draftId))
            ?? throw new NotFoundException("Exhibition draft was not found.");

        if (draft.SourceExhibitionId == null)
        {
            throw new DomainException("This draft does not revise a published exhibition.");
        }

        var sourceId = new ExhibitionId(draft.SourceExhibitionId.Value);
        var exhibition = await exhibitionRepo.GetExhibitionById(sourceId)
            ?? throw new NotFoundException("The original exhibition was not found.");

        await SavePublicationAsync(command, exhibition, draft);
    }

    private async Task SavePublicationAsync(CreateExhibitionCommand command, Exhibition? original, ExhibitionDraft? revisionDraft)
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
                        topicCommand.TitleEn,
                        topicCommand.Description,
                        topicCommand.DescriptionEn,
                        topicCommand.Body,
                        topicCommand.BodyEn,
                        topicCommand.CalloutLabel,
                        topicCommand.CalloutLabelEn,
                        topicCommand.CalloutText,
                        topicCommand.CalloutTextEn,
                        topicCommand.Qoute,
                        topicCommand.QouteEn
                        ));
                }

                subjects.Add(new NewSubject(
                    subjectCommand.Title,
                    subjectCommand.TitleEn,
                    subjectCommand.Slug,
                    subjectCommand.Description,
                    subjectCommand.DescriptionEn,
                    subjectCommand.QuizId,
                    topics));
            }

            var exhibitionId = original?.Id ?? new ExhibitionId(command.Id);

            var exhibition = Exhibition.Create(
                exhibitionId,
                command.ExhibtionName,
                command.ExhibitionNameEn,
                imageId,
                subjects);

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Exhibition created with id: {ExhibitionId}", exhibition.Id.Value);

            if (original != null && revisionDraft != null)
            {
                original.ApplyRevision(exhibition);
                exhibitionDraftRepo.Delete(revisionDraft);
            }
            else
            {
                await exhibitionRepo.AddAsync(exhibition);
            }

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
