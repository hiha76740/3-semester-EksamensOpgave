using GameService.ApplicationLib.Handlers;
using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities;
using GameService.DomainLib.ValueObjects;
using GameService.FacadeLib.Commands.DTOs;
using GameService.FacadeLib.Commands.Interfaces;
using Moq;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.ApplicationLib.Tests;

public class AnswerQuestionTests
{
    private static Quiz CreateQuiz()
    {
        return Quiz.Create("Quiz", "Quiz EN", Guid.NewGuid(), "vertical", "Done", "Done EN", [new NewQuestion("Question", "Question EN", [new Choice("Yes", "Answer EN", true, "Correct", "Correct EN"), new Choice("No", "Answer EN", false, "Incorrect", "Incorrect EN")])]);
    }

    [Fact]
    public async Task Handle_GivenValidAnswer_SavesAndReturnsResult()
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
        var command = new AnswerQuestionCommand(session.Id.Value, quiz.Questions[0].Id.Value, 1);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.False(result.IsCorrect);
        Assert.Equal("Incorrect", result.Feedback);
        Assert.Equal("Incorrect EN", result.FeedbackEn);
        Assert.Equal("Yes", result.CorrectAnswer);
        Assert.Single(session.Answers);
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
}
