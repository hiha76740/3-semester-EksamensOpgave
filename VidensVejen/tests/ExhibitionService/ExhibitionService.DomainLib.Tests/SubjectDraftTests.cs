using ExhibitionService.DomainLib.Entities;
using ExhibitionService.DomainLib.ValueObjects;

namespace ExhibitionService.DomainLib.Tests;

public class SubjectDraftTests
{
    [Fact]
    public void Update_GivenQuizReferences_PreservesBothReferencesAndQuizFlag()
    {
        // Arrange
        var draft = ExhibitionDraft.Create();
        var quizId = Guid.NewGuid();
        var quizDraftId = Guid.NewGuid();

        // Act
        draft.UpdateExhibitionDraft(null, null, null,
            [new NewSubjectDraft("Quiz", "Quiz EN", "quiz", null, null, quizId, quizDraftId, true, [])]);

        // Assert
        var subject = Assert.Single(draft.SubjectDrafts);
        Assert.NotEqual(Guid.Empty, subject.Id.Value);
        Assert.Equal(quizId, subject.QuizId?.Value);
        Assert.Equal(quizDraftId, subject.QuizDraftId?.Value);
        Assert.True(subject.IsQuiz);
        Assert.Equal("Quiz", subject.Title);
        Assert.Equal("Quiz EN", subject.TitleEn);
        Assert.Equal("quiz", subject.Slug);
    }

    [Fact]
    public void Update_GivenEmptySubject_AcceptsIncompleteDraft()
    {
        // Arrange
        var draft = ExhibitionDraft.Create();

        // Act
        draft.UpdateExhibitionDraft(null, null, null,
            [new NewSubjectDraft(null, null, null, null, null, null, null, false, [])]);

        // Assert
        var subject = Assert.Single(draft.SubjectDrafts);
        Assert.Null(subject.Title);
        Assert.Null(subject.QuizId);
        Assert.Null(subject.QuizDraftId);
        Assert.Empty(subject.TopicDrafts);
    }
}
