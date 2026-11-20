using GameService.DomainLib.Entities;
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
    public void Create_GivenValidData_InitializesQuizAndQuestion()
    {
        // Arrange
        var exhibitionId = Guid.NewGuid();
        var questions = CreateQuestions();

        // Act
        var quiz = Quiz.Create("Quiz", exhibitionId, "vertical", "Done", questions);

        // Assert
        Assert.NotEqual(Guid.Empty, quiz.Id.Value);
        Assert.Equal(exhibitionId, quiz.ExhibitionId.Value);
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

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_GivenBlankTitle_ThrowsDomainException(string? title)
    {
        // Arrange
        var questions = CreateQuestions();

        // Act & Assert
        Assert.Throws<DomainException>(() => Quiz.Create(title!, Guid.NewGuid(), "vertical", null, questions));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_GivenBlankLayout_ThrowsDomainException(string? layout)
    {
        // Arrange
        var questions = CreateQuestions();

        // Act & Assert
        Assert.Throws<DomainException>(() => Quiz.Create("Quiz", Guid.NewGuid(), layout!, null, questions));
    }

    [Fact]
    public void Create_GivenEmptyExhibitionId_ThrowsDomainException()
    {
        // Arrange
        var questions = CreateQuestions();

        // Act & Assert
        Assert.Throws<DomainException>(() => Quiz.Create("Quiz", Guid.Empty, "vertical", null, questions));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void Create_GivenMissingQuestions_ThrowsDomainException(bool useNull)
    {
        // Arrange
        List<NewQuestion> questions = [];
        if (useNull)
        {
            questions = null!;
        }

        // Act & Assert
        Assert.Throws<DomainException>(() => Quiz.Create("Quiz", Guid.NewGuid(), "vertical", null, questions));
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
