using GameService.DomainLib.Entities.Quizzes;
using GameService.DomainLib.Entities.Sessions;
using GameService.DomainLib.ValueObjects.Quizzes;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.DomainLib.Tests;

public class GameSessionTests
{
    private static Quiz CreateQuiz()
    {
        List<Choice> choices =
        [
            new Choice("Yes", "Yes EN", true, "Correct", "Correct EN"),
            new Choice("No", "No EN", false, "Incorrect", "Incorrect EN")
        ];
        List<NewQuestion> questions = [new NewQuestion("Question", "Question EN", choices)];

        return Quiz.Create(
            "Quiz", "Quiz EN", Guid.NewGuid(), "vertical", "Done", "Done EN", questions, "Instructions", "Instructions EN");
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
        Assert.Null(session.CompletedAt);
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
        Assert.Equal("Correct EN", result.FeedbackEn);
        Assert.Equal("Yes", result.CorrectAnswer);
        Assert.Equal("Yes EN", result.CorrectAnswerEn);
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
        Assert.Equal("Incorrect EN", result.FeedbackEn);
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
    public void Finish_GivenAllQuestionsAnswered_ReturnsScoreAndCompletesSession()
    {
        // Arrange
        var quiz = CreateQuiz();
        var session = GameSession.Start(quiz);
        session.Answer(quiz, quiz.Questions[0].Id.Value, 0);

        // Act
        var score = session.Finish(quiz);

        // Assert
        Assert.Equal(1, score.CorrectCount);
        Assert.Equal(1, score.TotalCount);
        Assert.Equal(100, score.Percentage);
        Assert.NotNull(session.CompletedAt);
    }

    [Fact]
    public void Finish_GivenWrongAnswer_ReturnsZeroCorrect()
    {
        // Arrange
        var quiz = CreateQuiz();
        var session = GameSession.Start(quiz);
        session.Answer(quiz, quiz.Questions[0].Id.Value, 1);

        // Act
        var score = session.Finish(quiz);

        // Assert
        Assert.Equal(0, score.CorrectCount);
        Assert.Equal(0, score.Percentage);
    }

    [Fact]
    public void Finish_GivenUnansweredQuestions_ThrowsDomainException()
    {
        // Arrange
        var quiz = CreateQuiz();
        var session = GameSession.Start(quiz);

        // Act & Assert
        Assert.Throws<DomainException>(() => session.Finish(quiz));
        Assert.Null(session.CompletedAt);
    }


    [Fact]
    public void Finish_GivenMixedAnswers_ReturnsScoreAcrossAllQuestions()
    {
        // Arrange
        List<Choice> choices =
        [
            new Choice("Yes", "Yes EN", true, null, null),
            new Choice("No", "No EN", false, null, null)
        ];
        var quiz = Quiz.Create("Quiz", "Quiz EN", Guid.NewGuid(), "vertical", null, null,
            [new NewQuestion("First", "First EN", choices), new NewQuestion("Second", "Second EN", choices)], "Instructions", "Instructions EN");
        var session = GameSession.Start(quiz);
        session.Answer(quiz, quiz.Questions[0].Id.Value, 0);
        session.Answer(quiz, quiz.Questions[1].Id.Value, 1);
        var before = DateTime.UtcNow;

        // Act
        var score = session.Finish(quiz);

        // Assert
        Assert.Equal(1, score.CorrectCount);
        Assert.Equal(2, score.TotalCount);
        Assert.Equal(50, score.Percentage);
        Assert.NotNull(session.CompletedAt);
        Assert.InRange(session.CompletedAt.Value, before, DateTime.UtcNow);
    }
}
