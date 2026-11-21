using ExhibitionService.ApplicationLib.Repositories;
using ExhibitionService.ApplicationLib.Services;
using ExhibitionService.DomainLib.Entities;
using ExhibitionService.DomainLib.ValueObjects;
using ExhibitionService.FacadeLib.Commands.DTOs;
using ExhibitionService.FacadeLib.Commands.Interfaces;

namespace ExhibitionService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
public class CreateExhibitionHandler(
    IExhibitionRepository exhibitionRepo,
    IImageService imageService) : ICreateExhibitionHandler
{
    async Task ICreateExhibitionHandler.HandleAsync(CreateExhibitionCommand command)
    {
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
                subjectCommand.Description,
                topics));
        }

        Guid? theme = null;

        if (command.ImageStream != null && command.Extension != null)
            theme = await imageService.SaveAsync(command.ImageStream, command.Extension);

        var exhibition = Exhibition.Create(
            command.ExhibtionName,
            theme,
            subjects);

        await exhibitionRepo.AddAsync(exhibition);
        await exhibitionRepo.SaveAsync();
    }
}
