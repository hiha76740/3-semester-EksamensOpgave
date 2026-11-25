using GameService.DomainLib.Entities.Quizzes;
using GameService.DomainLib.ValueObjects.Quizzes;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.DomainLib.Tests;

public class QuestionTests
{
    private static List<Choice> CreateChoices()
    {
        return [new Choice("Yes", "Answer EN", true, "Correct", "Correct EN"), new Choice("No", "Answer EN", false, "Incorrect", "Incorrect EN")];
    }

    [Fact]
    public void Constructor_GivenValidInput_InitializesQuestion()
    {
        // Arrange
        var choices = CreateChoices();

        // Act
        var question = new Question("Question", "Question EN", choices);

        // Assert
        Assert.NotEqual(Guid.Empty, question.Id.Value);
        Assert.Equal("Question", question.Text);
        Assert.Equal("Question EN", question.TextEn);
        Assert.Equal<Choice>(choices, question.Choices);
    }

    [Fact]
    public void Constructor_GivenChoicesWithoutFeedback_PreservesNullValues()
    {
        // Arrange
        List<Choice> choices =
        [
            new Choice("Yes", "Yes EN", true, null, null),
            new Choice("No", "No EN", false, null, null)
        ];

        // Act
        var question = new Question("Question", "Question EN", choices);

        // Assert
        Assert.All(question.Choices, choice =>
        {
            Assert.Null(choice.Feedback);
            Assert.Null(choice.FeedbackEn);
        });
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_GivenBlankText_ThrowsDomainException(string? text)
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => new Question(text, "Question EN", CreateChoices()));
    }

    [Fact]
    public void Constructor_GivenNoChoices_ThrowsDomainException()
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => new Question("Question", "Question EN", []));
        Assert.Throws<DomainException>(() => new Question("Question", "Question EN", null));
    }

    [Fact]
    public void Constructor_GivenOneChoice_ThrowsDomainException()
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => new Question("Question", "Question EN", [new Choice("Only", "Answer EN", true, "Correct", "Correct EN")]));
    }

    [Fact]
    public void Constructor_GivenNoCorrectChoice_ThrowsDomainException()
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => new Question("Question", "Question EN", [new Choice("A", "Answer EN", false, "Incorrect", "Incorrect EN"), new Choice("B", "Answer EN", false, "Incorrect", "Incorrect EN")]));
    }

    [Fact]
    public void Constructor_GivenMultipleCorrectChoices_ThrowsDomainException()
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => new Question("Question", "Question EN", [new Choice("A", "Answer EN", true, "Correct", "Correct EN"), new Choice("B", "Answer EN", true, "Correct", "Correct EN")]));
    }

    [Fact]
    public void Constructor_GivenThreeChoicesWithOneCorrect_AcceptsChoicesInOrder()
    {
        // Arrange
        List<Choice> choices =
        [
            new Choice("A", "Answer EN", false, "Incorrect", "Incorrect EN"),
            new Choice("B", "Answer EN", true, "Correct", "Correct EN"),
            new Choice("C", "Answer EN", false, "Incorrect", "Incorrect EN")
        ];

        // Act
        var question = new Question("Question", "Question EN", choices);

        // Assert
        Assert.Equal<Choice>(choices, question.Choices);
    }

    [Fact]
    public void Constructor_GivenChoices_CopiesCollection()
    {
        // Arrange
        var choices = CreateChoices();
        var question = new Question("Question", "Question EN", choices);

        // Act
        choices.Clear();

        // Assert
        Assert.Equal(2, question.Choices.Count);
    }
}
