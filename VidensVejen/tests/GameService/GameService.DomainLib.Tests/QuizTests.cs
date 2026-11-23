using GameService.DomainLib.Entities;
using GameService.DomainLib.Enums;
using GameService.DomainLib.ValueObjects;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.DomainLib.Tests;

public class QuizTests
{
    private static List<NewQuestion> CreateQuestions()
    {
        return [new NewQuestion("Question", [new Choice("Yes", true), new Choice("No", false)], "Correct", "Incorrect")];
    }

    [Fact]
    public void Create_GivenValidData_InitializesQuizAsDraft()
    {
        // Arrange
        var exhibitionId = Guid.NewGuid();
        var questions = CreateQuestions();

        // Act
        var quiz = Quiz.Create("Quiz", exhibitionId, "vertical", "Done", questions);

        // Assert
        Assert.NotEqual(Guid.Empty, quiz.Id.Value);
        Assert.Equal(PublicationStatus.Draft, quiz.Status);
        Assert.Equal(exhibitionId, quiz.ExhibitionId?.Value);
        Assert.Equal("Quiz", quiz.Title);
        Assert.Equal("vertical", quiz.QuizLayout);
        Assert.Equal("Done", quiz.EndMessage);
        var question = Assert.Single(quiz.Questions);
        Assert.NotEqual(Guid.Empty, question.Id.Value);
        Assert.Equal("Question", question.Text);
        Assert.Equal("Correct", question.CorrectFeedback);
        Assert.Equal("Incorrect", question.IncorrectFeedback);
        Assert.Equal(questions[0].Choices, question.Choices);
    }

    [Fact]
    public void Create_GivenNoData_CreatesEmptyDraft()
    {
        // Act
        var quiz = Quiz.Create(null, null, null, null, null);

        // Assert
        Assert.Equal(PublicationStatus.Draft, quiz.Status);
        Assert.Null(quiz.Title);
        Assert.Null(quiz.ExhibitionId);
        Assert.Null(quiz.QuizLayout);
        Assert.Empty(quiz.Questions);
    }

    [Fact]
    public void Publish_GivenValidDraft_SetsStatusToPublished()
    {
        // Arrange
        var quiz = Quiz.Create("Quiz", Guid.NewGuid(), "vertical", null, CreateQuestions());

        // Act
        quiz.Publish();

        // Assert
        Assert.Equal(PublicationStatus.Published, quiz.Status);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Publish_GivenBlankTitle_ThrowsDomainException(string? title)
    {
        // Arrange
        var quiz = Quiz.Create(title, Guid.NewGuid(), "vertical", null, CreateQuestions());

        // Act & Assert
        Assert.Throws<DomainException>(quiz.Publish);
        Assert.Equal(PublicationStatus.Draft, quiz.Status);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Publish_GivenBlankLayout_ThrowsDomainException(string? layout)
    {
        // Arrange
        var quiz = Quiz.Create("Quiz", Guid.NewGuid(), layout, null, CreateQuestions());

        // Act & Assert
        Assert.Throws<DomainException>(quiz.Publish);
        Assert.Equal(PublicationStatus.Draft, quiz.Status);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Publish_GivenMissingExhibition_ThrowsDomainException(bool useEmptyGuid)
    {
        // Arrange
        Guid? exhibitionId = null;
        if (useEmptyGuid)
        {
            exhibitionId = Guid.Empty;
        }

        var quiz = Quiz.Create("Quiz", exhibitionId, "vertical", null, CreateQuestions());

        // Act & Assert
        Assert.Throws<DomainException>(quiz.Publish);
        Assert.Equal(PublicationStatus.Draft, quiz.Status);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Publish_GivenMissingQuestions_ThrowsDomainException(bool useNull)
    {
        // Arrange
        List<NewQuestion>? questions = [];
        if (useNull)
        {
            questions = null;
        }

        var quiz = Quiz.Create("Quiz", Guid.NewGuid(), "vertical", null, questions);

        // Act & Assert
        Assert.Throws<DomainException>(quiz.Publish);
        Assert.Equal(PublicationStatus.Draft, quiz.Status);
    }

    [Fact]
    public void Publish_GivenInvalidQuestion_ThrowsDomainException()
    {
        // Arrange
        List<NewQuestion> questions = [new NewQuestion("Question", [new Choice("Yes", true)], null, null)];
        var quiz = Quiz.Create("Quiz", Guid.NewGuid(), "vertical", null, questions);

        // Act & Assert
        Assert.Throws<DomainException>(quiz.Publish);
        Assert.Equal(PublicationStatus.Draft, quiz.Status);
    }

    [Fact]
    public void Create_GivenMultipleQuestions_PreservesOrderAndOptionalNullValues()
    {
        // Arrange
        var questions = CreateQuestions();
        questions.Add(new NewQuestion("Second", [new Choice("A", false), new Choice("B", true)], null, null));

        // Act
        var quiz = Quiz.Create("Quiz", Guid.NewGuid(), "vertical", null, questions);

        // Assert
        Assert.Null(quiz.EndMessage);
        Assert.Equal(2, quiz.Questions.Count);
        Assert.Equal("Question", quiz.Questions[0].Text);
        Assert.Equal("Second", quiz.Questions[1].Text);
        Assert.Null(quiz.Questions[1].CorrectFeedback);
        Assert.Null(quiz.Questions[1].IncorrectFeedback);
        Assert.NotEqual(quiz.Questions[0].Id, quiz.Questions[1].Id);
    }
}
