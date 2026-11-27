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

            ImageId? themeImageId = null;

            if (command.ThemeImageId.HasValue)
            {
                themeImageId = new ImageId(command.ThemeImageId.Value);
            }


            var exhibitionDraft = await exhibitionDraftRepo.GetExhibitionDraftByIdAsync(id)
                ?? throw new NotFoundException("Exhibition draft not found");


            var subjectDrafts = new List<NewSubjectDraft>();

            foreach (var subjectCommand in command.SubjectDrafts ?? [])
            {
                var topicDrafts = new List<NewTopicDraft>();

                foreach (var topicCommand in subjectCommand.TopicDrafts ?? [])
                {
                    topicDrafts.Add(new NewTopicDraft(
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
                        topicCommand.Quote,
                        topicCommand.QuoteEn
                        ));
                }
                subjectDrafts.Add(new NewSubjectDraft(
                    subjectCommand.Title,
                    subjectCommand.TitleEn,
                    subjectCommand.Slug,
                    subjectCommand.Description,
                    subjectCommand.DescriptionEn,
                    subjectCommand.QuizId,
                    subjectCommand.QuizDraftId,
                    subjectCommand.IsQuiz,
                    topicDrafts));
            }

            exhibitionDraft.UpdateExhibitionDraft(
                command.ExhibtionName,
                command.ExhibitionNameEn,
                themeImageId,
                subjectDrafts);

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
