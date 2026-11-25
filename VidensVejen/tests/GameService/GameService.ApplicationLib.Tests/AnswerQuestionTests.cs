using GameService.ApplicationLib.Handlers;
using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities.Quizzes;
using GameService.DomainLib.Entities.Sessions;
using GameService.DomainLib.ValueObjects.Quizzes;
using GameService.FacadeLib.Commands.DTOs;
using GameService.FacadeLib.Commands.Interfaces;
using Moq;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.ApplicationLib.Tests;

public class AnswerQuestionTests
{
    private static Quiz CreateQuiz()
    {
        return Quiz.Create("Quiz", "Quiz EN", Guid.NewGuid(), "vertical", "Done", "Done EN", [new NewQuestion("Question", "Question EN", [new Choice("Yes", "Answer EN", true, "Correct", "Correct EN"), new Choice("No", "Answer EN", false, "Incorrect", "Incorrect EN")])], "Instructions", "Instructions EN");
    }

    [Theory]
    [InlineData(0, true, "Correct", "Correct EN")]
    [InlineData(1, false, "Incorrect", "Incorrect EN")]
    public async Task Handle_GivenValidAnswer_SavesAndReturnsResult(
        int choiceIndex, bool isCorrect, string feedback, string feedbackEn)
    {
        // Arrange
        var quiz = CreateQuiz();
        var session = GameSession.Start(quiz);
        var sessionRepository = new Mock<IGameSessionRepository>();
        sessionRepository.Setup(r => r.GetSessionByIdAsync(It.IsAny<GameSessionId>()))
            .ReturnsAsync(session);
        var quizRepository = new Mock<IQuizRepository>();
        quizRepository.Setup(r => r.GetQuizByIdAsync(It.IsAny<QuizId>()))
            .ReturnsAsync(quiz);
        IAnswerQuestionHandler handler = new AnswerQuestionHandler(sessionRepository.Object, quizRepository.Object);
        var command = new AnswerQuestionCommand(session.Id.Value, quiz.Questions[0].Id.Value, choiceIndex);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.Equal(isCorrect, result.IsCorrect);
        Assert.Equal(feedback, result.Feedback);
        Assert.Equal(feedbackEn, result.FeedbackEn);
        Assert.Equal("Yes", result.CorrectAnswer);
        Assert.Equal("Answer EN", result.CorrectAnswerEn);
        var answer = Assert.Single(session.Answers);
        Assert.Equal(command.QuestionId, answer.QuestionId);
        Assert.Equal(choiceIndex, answer.ChoiceIndex);
        Assert.Equal(isCorrect, answer.IsCorrect);
        sessionRepository.Verify(r => r.GetSessionByIdAsync(session.Id), Times.Once);
        quizRepository.Verify(r => r.GetQuizByIdAsync(session.QuizId), Times.Once);
        sessionRepository.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenUnknownSession_ThrowsNotFoundExceptionWithoutSaving()
    {
        // Arrange
        var sessionRepository = new Mock<IGameSessionRepository>();
        sessionRepository.Setup(r => r.GetSessionByIdAsync(It.IsAny<GameSessionId>()))
            .ReturnsAsync((GameSession?)null);
        var quizRepository = new Mock<IQuizRepository>();
        IAnswerQuestionHandler handler = new AnswerQuestionHandler(sessionRepository.Object, quizRepository.Object);
        var command = new AnswerQuestionCommand(Guid.NewGuid(), Guid.NewGuid(), 0);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command));
        sessionRepository.Verify(r => r.SaveAsync(), Times.Never);
        quizRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_GivenAlreadyAnsweredQuestion_ThrowsDomainExceptionWithoutSaving()
    {
        // Arrange
        var quiz = CreateQuiz();
        var session = GameSession.Start(quiz);
        var questionId = quiz.Questions[0].Id.Value;
        session.Answer(quiz, questionId, 0);
        var sessionRepository = new Mock<IGameSessionRepository>();
        sessionRepository.Setup(r => r.GetSessionByIdAsync(It.IsAny<GameSessionId>()))
            .ReturnsAsync(session);
        var quizRepository = new Mock<IQuizRepository>();
        quizRepository.Setup(r => r.GetQuizByIdAsync(It.IsAny<QuizId>()))
            .ReturnsAsync(quiz);
        IAnswerQuestionHandler handler = new AnswerQuestionHandler(sessionRepository.Object, quizRepository.Object);
        var command = new AnswerQuestionCommand(session.Id.Value, questionId, 1);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(command));
        sessionRepository.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenUnknownQuiz_ThrowsWithoutReturningResult()
    {
        // Arrange
        var quiz = CreateQuiz();
        var session = GameSession.Start(quiz);
        var sessionRepository = new Mock<IGameSessionRepository>();
        sessionRepository.Setup(r => r.GetSessionByIdAsync(session.Id)).ReturnsAsync(session);
        var quizRepository = new Mock<IQuizRepository>();
        quizRepository.Setup(r => r.GetQuizByIdAsync(session.QuizId)).ReturnsAsync((Quiz?)null);
        IAnswerQuestionHandler handler = new AnswerQuestionHandler(sessionRepository.Object, quizRepository.Object);

        // Act
        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(new AnswerQuestionCommand(session.Id.Value, quiz.Questions[0].Id.Value, 0)));

        // Assert
        sessionRepository.Verify(r => r.GetSessionByIdAsync(session.Id), Times.Once);
        quizRepository.Verify(r => r.GetQuizByIdAsync(session.QuizId), Times.Once);
        sessionRepository.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenSaveFailure_ThrowsWithoutReturningResult()
    {
        // Arrange
        var quiz = CreateQuiz();
        var session = GameSession.Start(quiz);
        var sessionRepository = new Mock<IGameSessionRepository>();
        sessionRepository.Setup(r => r.GetSessionByIdAsync(session.Id)).ReturnsAsync(session);
        var quizRepository = new Mock<IQuizRepository>();
        quizRepository.Setup(r => r.GetQuizByIdAsync(session.QuizId)).ReturnsAsync(quiz);
        var expected = new InvalidOperationException("Save failed");
        sessionRepository.Setup(r => r.SaveAsync()).ThrowsAsync(expected);
        IAnswerQuestionHandler handler = new AnswerQuestionHandler(sessionRepository.Object, quizRepository.Object);

        // Act
        var actual = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(new AnswerQuestionCommand(session.Id.Value, quiz.Questions[0].Id.Value, 0)));

        // Assert
        Assert.Same(expected, actual);
        sessionRepository.Verify(r => r.GetSessionByIdAsync(session.Id), Times.Once);
        quizRepository.Verify(r => r.GetQuizByIdAsync(session.QuizId), Times.Once);
        sessionRepository.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(2)]
    public async Task Handle_GivenInvalidChoiceIndex_ThrowsWithoutSaving(int choiceIndex)
    {
        // Arrange
        var quiz = CreateQuiz();
        var session = GameSession.Start(quiz);
        var sessions = new Mock<IGameSessionRepository>();
        sessions.Setup(r => r.GetSessionByIdAsync(session.Id)).ReturnsAsync(session);
        var quizzes = new Mock<IQuizRepository>();
        quizzes.Setup(r => r.GetQuizByIdAsync(quiz.Id)).ReturnsAsync(quiz);
        IAnswerQuestionHandler handler = new AnswerQuestionHandler(sessions.Object, quizzes.Object);
        var command = new AnswerQuestionCommand(session.Id.Value, quiz.Questions[0].Id.Value, choiceIndex);

        // Act
        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(command));

        // Assert
        Assert.Empty(session.Answers);
        sessions.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenUnknownQuestion_ThrowsWithoutSaving()
    {
        // Arrange
        var quiz = CreateQuiz();
        var session = GameSession.Start(quiz);
        var sessions = new Mock<IGameSessionRepository>();
        sessions.Setup(r => r.GetSessionByIdAsync(session.Id)).ReturnsAsync(session);
        var quizzes = new Mock<IQuizRepository>();
        quizzes.Setup(r => r.GetQuizByIdAsync(quiz.Id)).ReturnsAsync(quiz);
        IAnswerQuestionHandler handler = new AnswerQuestionHandler(sessions.Object, quizzes.Object);
        var command = new AnswerQuestionCommand(session.Id.Value, Guid.NewGuid(), 0);

        // Act
        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command));

        // Assert
        Assert.Empty(session.Answers);
        sessions.Verify(r => r.SaveAsync(), Times.Never);
    }
}
