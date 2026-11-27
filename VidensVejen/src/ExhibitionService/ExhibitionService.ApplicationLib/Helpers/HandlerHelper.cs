using ExhibitionService.DomainLib.Entities;
using ExhibitionService.DomainLib.ValueObjects;
using ExhibitionService.FacadeLib.Commands.DTOs;

namespace ExhibitionService.ApplicationLib.Helpers;

internal static class HandlerHelper
{
    internal static void Apply(ExhibitionDraft exhibitionDraft, UpdateExhibitionDraftCommand command)
    {
        ImageId? themeImageId = null;

        if (command.ThemeImageId.HasValue)
        {
            themeImageId = new ImageId(command.ThemeImageId.Value);
        }

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
    }
}
