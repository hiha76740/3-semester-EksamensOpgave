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

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_GivenBlankText_ThrowsDomainException(string? text)
    {
        // Arrange
        var choices = CreateChoices();

        // Act & Assert
        Assert.Throws<DomainException>(() => new Question(text!, choices, null, null));
    }

    [Fact]
    public void Constructor_GivenNullChoices_ThrowsDomainException()
    {
        // Arrange
        List<Choice> choices = null!;

        // Act & Assert
        Assert.Throws<DomainException>(() => new Question("Question", choices, null, null));
    }

    [Fact]
    public void Constructor_GivenNoChoices_ThrowsDomainException()
    {
        // Arrange
        List<Choice> choices = [];

        // Act & Assert
        Assert.Throws<DomainException>(() => new Question("Question", choices, null, null));
    }

    [Fact]
    public void Constructor_GivenOneChoice_ThrowsDomainException()
    {
        // Arrange
        List<Choice> choices = [new Choice("Only", true)];

        // Act & Assert
        Assert.Throws<DomainException>(() => new Question("Question", choices, null, null));
    }

    [Fact]
    public void Constructor_GivenNoCorrectChoice_ThrowsDomainException()
    {
        // Arrange
        List<Choice> choices = [new Choice("A", false), new Choice("B", false)];

        // Act & Assert
        Assert.Throws<DomainException>(() => new Question("Question", choices, null, null));
    }

    [Fact]
    public void Constructor_GivenMultipleCorrectChoices_ThrowsDomainException()
    {
        // Arrange
        List<Choice> choices = [new Choice("A", true), new Choice("B", true)];

        // Act & Assert
        Assert.Throws<DomainException>(() => new Question("Question", choices, null, null));
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
