using GameService.DomainLib.ValueObjects.Drafts;

namespace GameService.DomainLib.Tests;

public class ChoiceDraftTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Constructor_GivenValidText_SetsValues(bool isCorrect)
    {
        // Arrange
        const string Text = "Answer";

        // Act
        var choice = new ChoiceDraft(Text, "Answer EN", isCorrect, "Incorrect", "Incorrect EN");

        // Assert
        Assert.Equal(Text, choice.Text);
        Assert.Equal("Answer EN", choice.TextEn);
        Assert.Equal("Incorrect", choice.Feedback);
        Assert.Equal("Incorrect EN", choice.FeedbackEn);
        Assert.Equal(isCorrect, choice.IsCorrect);
        Assert.Equal(new ChoiceDraft(Text, "Answer EN", isCorrect, "Incorrect", "Incorrect EN"), choice);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_GivenBlankText_PreservesIncompleteDraft(string? text)
    {
        // Act
        var choice = new ChoiceDraft(text, "Answer EN", false, "Incorrect", "Incorrect EN");

        // Assert
        Assert.Equal(text, choice.Text);
        Assert.Equal("Answer EN", choice.TextEn);
        Assert.False(choice.IsCorrect);
        Assert.Equal("Incorrect", choice.Feedback);
        Assert.Equal("Incorrect EN", choice.FeedbackEn);
    }

    [Fact]
    public void Constructor_GivenOptionalNullValues_PreservesNullValues()
    {
        // Act
        var choice = new ChoiceDraft("Answer", null, false, null, null);

        // Assert
        Assert.Null(choice.TextEn);
        Assert.Null(choice.Feedback);
        Assert.Null(choice.FeedbackEn);
    }
}
