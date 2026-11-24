using GameService.DomainLib.Entities;
using GameService.DomainLib.Enums;
using GameService.DomainLib.ValueObjects;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.DomainLib.Tests;

public class QuizTests
{
    private static List<NewQuestion> CreateQuestions()
    {
        return [new NewQuestion("Question", "Question EN", [new Choice("Yes", "Answer EN", true, "Correct", "Correct EN"), new Choice("No", "Answer EN", false, "Incorrect", "Incorrect EN")])];
    }

    [Fact]
    public void Create_GivenValidData_InitializesQuizAsDraft()
    {
        // Arrange
        var exhibitionId = Guid.NewGuid();
        var questions = CreateQuestions();

        // Act
        var quiz = Quiz.Create("Quiz", "Quiz EN", exhibitionId, "vertical", "Done", "Done EN", questions);

        // Assert
        Assert.NotEqual(Guid.Empty, quiz.Id.Value);
        Assert.Equal(PublicationStatus.Draft, quiz.Status);
        Assert.Equal(exhibitionId, quiz.ExhibitionId?.Value);
        Assert.Equal("Quiz", quiz.Title);
        Assert.Equal("Quiz EN", quiz.TitleEn);
        Assert.Equal("Done EN", quiz.EndMessageEn);
        Assert.Equal("vertical", quiz.QuizLayout);
        Assert.Equal("Done", quiz.EndMessage);
        var question = Assert.Single(quiz.Questions);
        Assert.NotEqual(Guid.Empty, question.Id.Value);
        Assert.Equal("Question", question.Text);
        Assert.Equal(questions[0].Choices, question.Choices);
    }

    [Fact]
    public void Create_GivenNoData_CreatesEmptyDraft()
    {
        // Act
        var quiz = Quiz.Create(null, null!, null, null, null, null, null);

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
        var quiz = Quiz.Create("Quiz", "Quiz EN", Guid.NewGuid(), "vertical", null, "Done EN", CreateQuestions());

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
        var quiz = Quiz.Create(title, "Quiz EN", Guid.NewGuid(), "vertical", null, "Done EN", CreateQuestions());

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
        var quiz = Quiz.Create("Quiz", "Quiz EN", Guid.NewGuid(), layout, null, "Done EN", CreateQuestions());

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

        var quiz = Quiz.Create("Quiz", "Quiz EN", exhibitionId, "vertical", null, "Done EN", CreateQuestions());

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

        var quiz = Quiz.Create("Quiz", "Quiz EN", Guid.NewGuid(), "vertical", null, "Done EN", questions);

        // Act & Assert
        Assert.Throws<DomainException>(quiz.Publish);
        Assert.Equal(PublicationStatus.Draft, quiz.Status);
    }

    [Fact]
    public void Publish_GivenInvalidQuestion_ThrowsDomainException()
    {
        // Arrange
        List<NewQuestion> questions = [new NewQuestion("Question", "Question EN", [new Choice("Yes", "Answer EN", true, "Correct", "Correct EN")])];
        var quiz = Quiz.Create("Quiz", "Quiz EN", Guid.NewGuid(), "vertical", null, "Done EN", questions);

        // Act & Assert
        Assert.Throws<DomainException>(quiz.Publish);
        Assert.Equal(PublicationStatus.Draft, quiz.Status);
    }

    [Fact]
    public void Create_GivenMultipleQuestions_PreservesOrderAndOptionalNullValues()
    {
        // Arrange
        var questions = CreateQuestions();
        questions.Add(new NewQuestion("Second", "Question EN", [new Choice("A", "Answer EN", false, "Incorrect", "Incorrect EN"), new Choice("B", "Answer EN", true, "Correct", "Correct EN")]));

        // Act
        var quiz = Quiz.Create("Quiz", "Quiz EN", Guid.NewGuid(), "vertical", null, "Done EN", questions);

        // Assert
        Assert.Null(quiz.EndMessage);
        Assert.Equal(2, quiz.Questions.Count);
        Assert.Equal("Question", quiz.Questions[0].Text);
        Assert.Equal("Second", quiz.Questions[1].Text);
        Assert.NotEqual(quiz.Questions[0].Id, quiz.Questions[1].Id);
    }
}
