using GameService.DomainLib.Entities;
using GameService.DomainLib.ValueObjects;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.DomainLib.Tests;

public class QuestionTests
{
    private static List<Choice> CreateChoices()
    {
        return [new Choice("Yes", true), new Choice("No", false)];
    }

    [Fact]
    public void Constructor_GivenValidInput_InitializesQuestion()
    {
        // Arrange
        var choices = CreateChoices();

        // Act
        var question = new Question("Question", choices, "Correct", "Incorrect");

        // Assert
        Assert.NotEqual(Guid.Empty, question.Id.Value);
        Assert.Equal("Question", question.Text);
        Assert.Equal("Correct", question.CorrectFeedback);
        Assert.Equal("Incorrect", question.IncorrectFeedback);
        Assert.Equal<Choice>(choices, question.Choices);
    }

    [Fact]
    public void Constructor_GivenNullFeedback_PreservesNullValues()
    {
        // Arrange
        var choices = CreateChoices();

        // Act
        var question = new Question("Question", choices, null, null);

        // Assert
        Assert.Null(question.CorrectFeedback);
        Assert.Null(question.IncorrectFeedback);
    }

    [Fact]
    public void Constructor_GivenNoData_CreatesEmptyQuestion()
    {
        // Act
        var question = new Question(null, null, null, null);

        // Assert
        Assert.Null(question.Text);
        Assert.Empty(question.Choices);
    }

    [Fact]
    public void EnsureCanBePublished_GivenValidQuestion_DoesNotThrow()
    {
        // Arrange
        var question = new Question("Question", CreateChoices(), null, null);

        // Act
        var exception = Record.Exception(question.EnsureCanBePublished);

        // Assert
        Assert.Null(exception);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void EnsureCanBePublished_GivenBlankText_ThrowsDomainException(string? text)
    {
        // Arrange
        var question = new Question(text, CreateChoices(), null, null);

        // Act & Assert
        Assert.Throws<DomainException>(question.EnsureCanBePublished);
    }

    [Fact]
    public void EnsureCanBePublished_GivenNoChoices_ThrowsDomainException()
    {
        // Arrange
        var question = new Question("Question", [], null, null);

        // Act & Assert
        Assert.Throws<DomainException>(question.EnsureCanBePublished);
    }

    [Fact]
    public void EnsureCanBePublished_GivenOneChoice_ThrowsDomainException()
    {
        // Arrange
        var question = new Question("Question", [new Choice("Only", true)], null, null);

        // Act & Assert
        Assert.Throws<DomainException>(question.EnsureCanBePublished);
    }

    [Fact]
    public void EnsureCanBePublished_GivenNoCorrectChoice_ThrowsDomainException()
    {
        // Arrange
        var question = new Question("Question", [new Choice("A", false), new Choice("B", false)], null, null);

        // Act & Assert
        Assert.Throws<DomainException>(question.EnsureCanBePublished);
    }

    [Fact]
    public void EnsureCanBePublished_GivenMultipleCorrectChoices_ThrowsDomainException()
    {
        // Arrange
        var question = new Question("Question", [new Choice("A", true), new Choice("B", true)], null, null);

        // Act & Assert
        Assert.Throws<DomainException>(question.EnsureCanBePublished);
    }

    [Fact]
    public void Constructor_GivenThreeChoicesWithOneCorrect_AcceptsChoicesInOrder()
    {
        // Arrange
        List<Choice> choices =
        [
            new Choice("A", false),
            new Choice("B", true),
            new Choice("C", false)
        ];

        // Act
        var question = new Question("Question", choices, null, null);

        // Assert
        Assert.Equal<Choice>(choices, question.Choices);
    }

    [Fact]
    public void Constructor_GivenChoices_CopiesCollection()
    {
        // Arrange
        var choices = CreateChoices();
        var question = new Question("Question", choices, null, null);

        // Act
        choices.Clear();

        // Assert
        Assert.Equal(2, question.Choices.Count);
    }
}
