using GameService.DomainLib.Entities.Drafts;
using GameService.DomainLib.ValueObjects.Drafts;

namespace GameService.DomainLib.Tests;

public class QuestionDraftTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_GivenIncompleteText_CreatesQuestion(string? text)
    {
        // Act
        var question = new QuestionDraft(text, null, null);

        // Assert
        Assert.NotEqual(Guid.Empty, question.Id.Value);
        Assert.Equal(text, question.Text);
        Assert.Null(question.TextEn);
        Assert.Empty(question.Choices);
    }

    [Fact]
    public void Constructor_GivenChoices_PreservesValuesAndCopiesList()
    {
        // Arrange
        var choice = new ChoiceDraft("Only answer", "Answer EN", false, "Feedback", "Feedback EN");
        List<ChoiceDraft> choices = [choice];

        // Act
        var question = new QuestionDraft("Question", "Question EN", choices);
        choices.Clear();

        // Assert
        Assert.Equal("Question", question.Text);
        Assert.Equal("Question EN", question.TextEn);
        Assert.Equal(choice, Assert.Single(question.Choices));
    }

    [Fact]
    public void Constructor_GivenMultipleCorrectChoices_AllowsIncompleteDraft()
    {
        // Arrange
        List<ChoiceDraft> choices =
        [
            new ChoiceDraft("A", null, true, null, null),
            new ChoiceDraft("B", null, true, null, null)
        ];

        // Act
        var question = new QuestionDraft(null, null, choices);

        // Assert
        Assert.Equal<ChoiceDraft>(choices, question.Choices);
    }
}
