using GameService.DomainLib.Entities;
using GameService.DomainLib.ValueObjects;

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
    public void Update_GivenNewData_ReplacesFieldsAndQuestions()
    {
        // Arrange
        var draft = QuizDraft.Create("Quiz", "Quiz EN", Guid.NewGuid(), "vertical", "Done", "Done EN", CreateQuestions());
        var exhibitionId = Guid.NewGuid();
        List<NewQuestion> questions = [new NewQuestion("New", "New EN", [new Choice("A", "A EN", true, null, null)])];

        // Act
        draft.Update("New quiz", "New quiz EN", exhibitionId, "horizontal", "Bye", "Bye EN", questions);

        // Assert
        Assert.Equal("New quiz", draft.Title);
        Assert.Equal("New quiz EN", draft.TitleEn);
        Assert.Equal(exhibitionId, draft.ExhibitionId?.Value);
        Assert.Equal("horizontal", draft.QuizLayout);
        Assert.Equal("Bye", draft.EndMessage);
        Assert.Equal("Bye EN", draft.EndMessageEn);
        var question = Assert.Single(draft.Questions);
        Assert.Equal("New", question.Text);
        Assert.Single(question.Choices);
    }

    [Fact]
    public void Update_GivenNoData_ClearsDraft()
    {
        // Arrange
        var draft = QuizDraft.Create("Quiz", "Quiz EN", Guid.NewGuid(), "vertical", "Done", "Done EN", CreateQuestions());

        // Act
        draft.Update(null, null, null, null, null, null, null);

        // Assert
        Assert.Null(draft.Title);
        Assert.Null(draft.ExhibitionId);
        Assert.Null(draft.QuizLayout);
        Assert.Empty(draft.Questions);
    }

    [Fact]
    public void Update_GivenEmptyExhibition_SetsExhibitionToNull()
    {
        // Arrange
        var draft = QuizDraft.Create("Quiz", null, Guid.NewGuid(), "vertical", null, null, null);

        // Act
        draft.Update("Quiz", null, Guid.Empty, "vertical", null, null, null);

        // Assert
        Assert.Null(draft.ExhibitionId);
    }
}
