using GameService.DomainLib.Entities;
using GameService.DomainLib.ValueObjects;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.DomainLib.Tests;

public class GameSessionTests
{
    private static Quiz CreateQuiz()
    {
        return Quiz.Create("Quiz", Guid.NewGuid(), "vertical", "Done",
            [new NewQuestion("Question", [new Choice("Yes", true), new Choice("No", false)], "Correct", "Incorrect")]);
    }

    [Fact]
    public void Start_GivenQuiz_CreatesSession()
    {
        // Arrange
        var quiz = CreateQuiz();

        // Act
        var session = GameSession.Start(quiz);

        // Assert
        Assert.NotEqual(Guid.Empty, session.Id.Value);
        Assert.Equal(quiz.Id, session.QuizId);
        Assert.Empty(session.Answers);
    }

    [Fact]
    public void Answer_GivenCorrectChoice_ReturnsCorrectWithFeedbackAndStoresAnswer()
    {
        // Arrange
        var quiz = CreateQuiz();
        var session = GameSession.Start(quiz);
        var questionId = quiz.Questions[0].Id.Value;

        // Act
        var result = session.Answer(quiz, questionId, 0);

        // Assert
        Assert.True(result.IsCorrect);
        Assert.Equal("Correct", result.Feedback);
        Assert.Equal("Yes", result.CorrectAnswer);
        var answer = Assert.Single(session.Answers);
        Assert.Equal(questionId, answer.QuestionId);
        Assert.Equal(0, answer.ChoiceIndex);
        Assert.True(answer.IsCorrect);
    }

    [Fact]
    public void Answer_GivenWrongChoice_ReturnsIncorrectWithCorrectAnswer()
    {
        // Arrange
        var quiz = CreateQuiz();
        var session = GameSession.Start(quiz);
        var questionId = quiz.Questions[0].Id.Value;

        // Act
        var result = session.Answer(quiz, questionId, 1);

        // Assert
        Assert.False(result.IsCorrect);
        Assert.Equal("Incorrect", result.Feedback);
        Assert.Equal("Yes", result.CorrectAnswer);
        Assert.False(Assert.Single(session.Answers).IsCorrect);
    }

    [Fact]
    public void Answer_GivenAlreadyAnsweredQuestion_ThrowsDomainException()
    {
        // Arrange
        var quiz = CreateQuiz();
        var session = GameSession.Start(quiz);
        var questionId = quiz.Questions[0].Id.Value;
        session.Answer(quiz, questionId, 0);

        // Act & Assert
        Assert.Throws<DomainException>(() => session.Answer(quiz, questionId, 1));
        Assert.Single(session.Answers);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(2)]
    public void Answer_GivenInvalidChoiceIndex_ThrowsDomainException(int choiceIndex)
    {
        // Arrange
        var quiz = CreateQuiz();
        var session = GameSession.Start(quiz);
        var questionId = quiz.Questions[0].Id.Value;

        // Act & Assert
        Assert.Throws<DomainException>(() => session.Answer(quiz, questionId, choiceIndex));
        Assert.Empty(session.Answers);
    }

    [Fact]
    public void Answer_GivenUnknownQuestion_ThrowsNotFoundException()
    {
        // Arrange
        var quiz = CreateQuiz();
        var session = GameSession.Start(quiz);

        // Act & Assert
        Assert.Throws<NotFoundException>(() => session.Answer(quiz, Guid.NewGuid(), 0));
    }

    [Fact]
    public void Answer_GivenOtherQuiz_ThrowsDomainException()
    {
        // Arrange
        var quiz = CreateQuiz();
        var otherQuiz = CreateQuiz();
        var session = GameSession.Start(quiz);
        var questionId = otherQuiz.Questions[0].Id.Value;

        // Act & Assert
        Assert.Throws<DomainException>(() => session.Answer(otherQuiz, questionId, 0));
    }
}
