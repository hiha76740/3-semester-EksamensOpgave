using GameService.DomainLib.Entities;
using GameService.DomainLib.ValueObjects;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.DomainLib.Tests;

public class QuizDraftTests
{
    private static List<NewQuestion> CreateQuestions()
    {
        return [new NewQuestion("Question", "Question EN", [new Choice("Yes", "Answer EN", true, "Correct", "Correct EN"), new Choice("No", "Answer EN", false, "Incorrect", "Incorrect EN")])];
    }

    [Fact]
    public void Create_GivenValidData_InitializesDraft()
    {
        // Arrange
        var exhibitionId = Guid.NewGuid();

        // Act
        var draft = QuizDraft.Create("Quiz", "Quiz EN", exhibitionId, "vertical", "Done", "Done EN", CreateQuestions());

        // Assert
        Assert.NotEqual(Guid.Empty, draft.Id.Value);
        Assert.Equal(exhibitionId, draft.ExhibitionId?.Value);
        Assert.Equal("Quiz", draft.Title);
        Assert.Equal("Quiz EN", draft.TitleEn);
        Assert.Equal("vertical", draft.QuizLayout);
        Assert.Equal("Done", draft.EndMessage);
        Assert.Equal("Done EN", draft.EndMessageEn);
        var question = Assert.Single(draft.Questions);
        Assert.Equal("Question", question.Text);
        Assert.Equal("Question EN", question.TextEn);
        Assert.Equal(2, question.Choices.Count);
    }

    [Fact]
    public void Create_GivenNoData_CreatesEmptyDraft()
    {
        // Act
        var draft = QuizDraft.Create(null, null, null, null, null, null, null);

        // Assert
        Assert.Null(draft.Title);
        Assert.Null(draft.ExhibitionId);
        Assert.Null(draft.QuizLayout);
        Assert.Empty(draft.Questions);
    }

    [Fact]
    public void Create_GivenInvalidQuestion_KeepsQuestionInDraft()
    {
        // Arrange
        List<NewQuestion> questions = [new NewQuestion(null, null, [new Choice("Only", "Only EN", true, null, null)])];

        // Act
        var draft = QuizDraft.Create("Quiz", null, Guid.NewGuid(), "vertical", null, null, questions);

        // Assert
        var question = Assert.Single(draft.Questions);
        Assert.Null(question.Text);
        Assert.Single(question.Choices);
    }

    [Fact]
    public void Publish_GivenValidDraft_ReturnsQuizWithSameContent()
    {
        // Arrange
        var draft = QuizDraft.Create("Quiz", "Quiz EN", Guid.NewGuid(), "vertical", "Done", "Done EN", CreateQuestions());

        // Act
        var quiz = draft.Publish();

        // Assert
        Assert.NotEqual(Guid.Empty, quiz.Id.Value);
        Assert.Equal(draft.Title, quiz.Title);
        Assert.Equal(draft.TitleEn, quiz.TitleEn);
        Assert.Equal(draft.ExhibitionId?.Value, quiz.ExhibitionId.Value);
        Assert.Equal(draft.QuizLayout, quiz.QuizLayout);
        Assert.Equal(draft.EndMessage, quiz.EndMessage);
        Assert.Equal(draft.EndMessageEn, quiz.EndMessageEn);
        var question = Assert.Single(quiz.Questions);
        Assert.Equal("Question", question.Text);
        Assert.Equal("Question EN", question.TextEn);
        Assert.Equal(draft.Questions[0].Choices, question.Choices);
    }

    [Fact]
    public void Publish_GivenEmptyDraft_ThrowsDomainException()
    {
        // Arrange
        var draft = QuizDraft.Create(null, null, null, null, null, null, null);

        // Act & Assert
        Assert.Throws<DomainException>(() => draft.Publish());
    }

    [Fact]
    public void Publish_GivenInvalidQuestion_ThrowsDomainException()
    {
        // Arrange
        List<NewQuestion> questions = [new NewQuestion("Question", "Question EN", [new Choice("Only", "Only EN", true, null, null)])];
        var draft = QuizDraft.Create("Quiz", null, Guid.NewGuid(), "vertical", null, null, questions);

        // Act & Assert
        Assert.Throws<DomainException>(() => draft.Publish());
    }
}
