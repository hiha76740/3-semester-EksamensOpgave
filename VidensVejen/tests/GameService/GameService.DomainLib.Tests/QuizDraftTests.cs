using GameService.DomainLib.Entities;
using GameService.DomainLib.ValueObjects;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.DomainLib.Tests;

public class QuizDraftTests
{
    private static List<NewQuestion> CreateQuestions()
    {
        return [new NewQuestion("Question", [new Choice("Yes", true), new Choice("No", false)], "Correct", "Incorrect")];
    }

    [Fact]
    public void Create_GivenNoData_CreatesEmptyDraft()
    {
        // Act
        var draft = QuizDraft.Create(null, null, null, null, null);

        // Assert
        Assert.NotEqual(Guid.Empty, draft.Id.Value);
        Assert.Null(draft.Title);
        Assert.Null(draft.ExhibitionId);
        Assert.Empty(draft.Questions);
    }

    [Fact]
    public void Create_GivenIncompleteQuestion_KeepsQuestion()
    {
        // Arrange
        List<NewQuestion> questions = [new NewQuestion(null, [new Choice("Only", true)], null, null)];

        // Act
        var draft = QuizDraft.Create("Quiz", null, null, null, questions);

        // Assert
        var question = Assert.Single(draft.Questions);
        Assert.Null(question.Text);
        Assert.Single(question.Choices);
    }

    [Fact]
    public void Publish_GivenValidDraft_ReturnsQuizWithSameContent()
    {
        // Arrange
        var exhibitionId = Guid.NewGuid();
        var draft = QuizDraft.Create("Quiz", exhibitionId, "vertical", "Done", CreateQuestions());

        // Act
        var quiz = draft.Publish();

        // Assert
        Assert.Equal("Quiz", quiz.Title);
        Assert.Equal(exhibitionId, quiz.ExhibitionId.Value);
        Assert.Equal("vertical", quiz.QuizLayout);
        Assert.Equal("Done", quiz.EndMessage);
        var question = Assert.Single(quiz.Questions);
        Assert.Equal("Question", question.Text);
        Assert.Equal(2, question.Choices.Count);
    }

    [Fact]
    public void Publish_GivenEmptyDraft_ThrowsDomainException()
    {
        // Arrange
        var draft = QuizDraft.Create(null, null, null, null, null);

        // Act & Assert
        Assert.Throws<DomainException>(draft.Publish);
    }

    [Fact]
    public void Publish_GivenInvalidQuestion_ThrowsDomainException()
    {
        // Arrange
        List<NewQuestion> questions = [new NewQuestion("Question", [new Choice("A", true), new Choice("B", true)], null, null)];
        var draft = QuizDraft.Create("Quiz", Guid.NewGuid(), "vertical", null, questions);

        // Act & Assert
        Assert.Throws<DomainException>(draft.Publish);
    }
}
