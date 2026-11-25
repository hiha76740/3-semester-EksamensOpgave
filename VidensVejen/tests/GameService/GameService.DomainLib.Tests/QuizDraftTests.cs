using GameService.DomainLib.Entities.Drafts;
using GameService.DomainLib.ValueObjects.Drafts;

namespace GameService.DomainLib.Tests;

public class QuizDraftTests
{
    private static List<NewQuestionDraft> CreateQuestions()
    {
        return [new NewQuestionDraft("Question", "Question EN", [new ChoiceDraft("Yes", "Answer EN", true, "Correct", "Correct EN"), new ChoiceDraft("No", "Answer EN", false, "Incorrect", "Incorrect EN")])];
    }

    [Fact]
    public void Update_GivenValidData_InitializesDraft()
    {
        // Arrange
        var exhibitionId = Guid.NewGuid();

        // Act
        var draft = QuizDraft.Create();
        draft.Update("Quiz", "Quiz EN", exhibitionId, "vertical", "Done", "Done EN", CreateQuestions(), "Instructions", "Instructions EN");

        // Assert
        Assert.NotEqual(Guid.Empty, draft.Id.Value);
        Assert.Equal(exhibitionId, draft.ExhibitionId?.Value);
        Assert.Equal("Quiz", draft.Title);
        Assert.Equal("Quiz EN", draft.TitleEn);
        Assert.Equal("Instructions", draft.Instruction);
        Assert.Equal("Instructions EN", draft.InstructionEn);
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
        var draft = QuizDraft.Create();

        // Assert
        Assert.Null(draft.Instruction);
        Assert.Null(draft.InstructionEn);
        Assert.Null(draft.Title);
        Assert.Null(draft.ExhibitionId);
        Assert.Null(draft.QuizLayout);
        Assert.Empty(draft.Questions);
    }

    [Fact]
    public void Update_GivenIncompleteQuestion_KeepsQuestionInDraft()
    {
        // Arrange
        List<NewQuestionDraft> questions = [new NewQuestionDraft(null, null, [new ChoiceDraft("Only", "Only EN", true, null, null)])];

        // Act
        var draft = QuizDraft.Create();
        draft.Update("Quiz", null, Guid.NewGuid(), "vertical", null, null, questions, "Instructions", "Instructions EN");

        // Assert
        var question = Assert.Single(draft.Questions);
        Assert.Null(question.Text);
        Assert.Single(question.Choices);
    }

    [Fact]
    public void Update_GivenNewData_ReplacesFieldsAndQuestions()
    {
        // Arrange
        var draft = QuizDraft.Create();
        draft.Update("Quiz", "Quiz EN", Guid.NewGuid(), "vertical", "Done", "Done EN", CreateQuestions(), "Instructions", "Instructions EN");
        var exhibitionId = Guid.NewGuid();
        List<NewQuestionDraft> questions = [new NewQuestionDraft("New", "New EN", [new ChoiceDraft("A", "A EN", true, null, null)])];

        // Act
        draft.Update("New quiz", "New quiz EN", exhibitionId, "horizontal", "Bye", "Bye EN", questions, "Instructions", "Instructions EN");

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
        var draft = QuizDraft.Create();
        draft.Update("Quiz", "Quiz EN", Guid.NewGuid(), "vertical", "Done", "Done EN", CreateQuestions(), "Instructions", "Instructions EN");

        // Act
        draft.Update(null, null, null, null, null, null, null, null, null);

        // Assert
        Assert.Null(draft.Instruction);
        Assert.Null(draft.InstructionEn);
        Assert.Null(draft.Title);
        Assert.Null(draft.ExhibitionId);
        Assert.Null(draft.QuizLayout);
        Assert.Empty(draft.Questions);
    }

    [Fact]
    public void Update_GivenEmptyExhibition_SetsExhibitionToNull()
    {
        // Arrange
        var draft = QuizDraft.Create();
        draft.Update("Quiz", null, Guid.NewGuid(), "vertical", null, null, null, "Instructions", "Instructions EN");

        // Act
        draft.Update("Quiz", null, Guid.Empty, "vertical", null, null, null, "Instructions", "Instructions EN");

        // Assert
        Assert.Null(draft.ExhibitionId);
    }

    [Fact]
    public void Create_CalledTwice_CreatesDifferentDraftIds()
    {
        // Act
        var first = QuizDraft.Create();
        var second = QuizDraft.Create();

        // Assert
        Assert.NotEqual(Guid.Empty, first.Id.Value);
        Assert.NotEqual(first.Id, second.Id);
    }

    [Fact]
    public void Update_GivenNewData_PreservesDraftId()
    {
        // Arrange
        var draft = QuizDraft.Create();
        var originalId = draft.Id;

        // Act
        draft.Update("New title", null, null, null, null, null, null, "Instructions", "Instructions EN");

        // Assert
        Assert.Equal(originalId, draft.Id);
    }

    [Theory]
    [InlineData(null, null)]
    [InlineData("", "")]
    [InlineData("   ", "   ")]
    public void Update_GivenIncompleteInstructions_PreservesValues(string? instruction, string? instructionEn)
    {
        // Arrange
        var draft = QuizDraft.Create();

        // Act
        draft.Update(null, null, null, null, null, null, null, instruction, instructionEn);

        // Assert
        Assert.Equal(instruction, draft.Instruction);
        Assert.Equal(instructionEn, draft.InstructionEn);
    }

    [Fact]
    public void Update_GivenChangedInstructions_ReplacesExistingInstructions()
    {
        // Arrange
        var draft = QuizDraft.Create();
        draft.Update(null, null, null, null, null, null, null, "Old", "Old EN");

        // Act
        draft.Update(null, null, null, null, null, null, null, "New", "New EN");

        // Assert
        Assert.Equal("New", draft.Instruction);
        Assert.Equal("New EN", draft.InstructionEn);
    }
}
