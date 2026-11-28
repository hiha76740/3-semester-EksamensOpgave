using VidensVejen.ContractsLib.Requests.ExhibitionService.Drafts;
using VidensVejen.ContractsLib.Requests.ExhibitionService.Exhibitions;

namespace VidensVejen.Web.Models.Exhibitions;

internal static class ExhibitionEditorMapper
{
    internal static UpdateExhibitionDraftRequest BuildDraftRequest(ExhibitionInputModel input, Guid? imageId, bool preserveSlug)
    {
        return new UpdateExhibitionDraftRequest(
            input.ExhibitionName,
            input.ExhibitionNameEn,
            imageId,
            input.Subjects
            .Select(subject => new UpdateSubjectDraftRequest(
                Guid.Empty,
                subject.Title,
                subject.TitleEn,
                GetSubjectSlug(subject, preserveSlug),
                subject.Description,
                subject.DescriptionEn,
                subject.QuizId,
                subject.QuizDraftId,
                subject.IsQuiz,
                subject.Topics.Select(topic => new UpdateTopicDraftRequest(
                    topic.Title,
                    topic.TitleEn,
                    topic.Description,
                    topic.DescriptionEn,
                    topic.Body,
                    topic.BodyEn,
                    topic.CalloutLabel,
                    topic.CalloutLabelEn,
                    topic.CalloutText,
                    topic.CalloutTextEn,
                    topic.Quote,
                    topic.QuoteEn
                )).ToList()
            )).ToList(),
            input.RowVersion
        );
    }

    internal static CreateExhibitionRequest BuildPublicationRequest(
        Guid draftId, ExhibitionInputModel input, Guid imageId, bool preserveSlug)
    {
        return new CreateExhibitionRequest(
            draftId,
            input.ExhibitionName,
            input.ExhibitionNameEn,
            imageId,
            input.Subjects.Select(s => new CreateSubjectRequest(
                s.Title,
                s.TitleEn,
                GetSubjectSlug(s, preserveSlug),
                s.Description,
                s.DescriptionEn,
                s.IsQuiz ? s.QuizId : null,
                s.IsQuiz ? [] : s.Topics.Select(t => new CreateTopicRequest(
                    t.Title,
                    t.TitleEn,
                    t.Description,
                    t.DescriptionEn,
                    t.Body,
                    t.BodyEn,
                    t.CalloutLabel,
                    t.CalloutLabelEn,
                    t.CalloutText,
                    t.CalloutTextEn,
                    t.Quote,
                    t.QuoteEn
                )).ToList()
            )).ToList(),
            input.RowVersion
        );
    }

    private static string GetSubjectSlug(ExhibitionSubjectInputModel subject, bool preserveSlug)
    {
        if (preserveSlug && !string.IsNullOrWhiteSpace(subject.Slug))
        {
            return subject.Slug;
        }

        return subject.Title.ToLowerInvariant();
    }
}
