using GameService.DomainLib.ValueObjects.Quizzes;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.DomainLib.Tests;

public class ChoiceTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Constructor_GivenValidText_SetsValues(bool isCorrect)
    {
        // Arrange
        const string Text = "Answer";

        // Act
        var choice = new Choice(Text, "Answer EN", isCorrect, "Incorrect", "Incorrect EN");

        // Assert
        Assert.Equal(Text, choice.Text);
        Assert.Equal("Answer EN", choice.TextEn);
        Assert.Equal("Incorrect", choice.Feedback);
        Assert.Equal("Incorrect EN", choice.FeedbackEn);
        Assert.Equal(isCorrect, choice.IsCorrect);
        Assert.Equal(new Choice(Text, "Answer EN", isCorrect, "Incorrect", "Incorrect EN"), choice);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_GivenBlankText_ThrowsDomainException(string? text)
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => new Choice(text!, "Answer EN", false, "Incorrect", "Incorrect EN"));
    }

    [Fact]
    public void Constructor_GivenNoFeedback_PreservesNullValues()
    {
        // Act
        var choice = new Choice("Answer", "Answer EN", false, null, null);

        // Assert
        Assert.Null(choice.Feedback);
        Assert.Null(choice.FeedbackEn);
    }
}
