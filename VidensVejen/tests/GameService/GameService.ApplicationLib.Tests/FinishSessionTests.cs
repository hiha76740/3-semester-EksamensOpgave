using GameService.ApplicationLib.Handlers;
using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities;
using GameService.DomainLib.ValueObjects;
using GameService.FacadeLib.Commands.Interfaces;
using Moq;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.ApplicationLib.Tests;

public class FinishSessionTests
{
    private static Quiz CreateQuiz()
    {
        return Quiz.Create("Quiz", "Quiz EN", Guid.NewGuid(), "vertical", "Done", "Done EN", [new NewQuestion("Question", "Question EN", [new Choice("Yes", "Yes EN", true, "Correct", "Correct EN"), new Choice("No", "No EN", false, "Incorrect", "Incorrect EN")])]);
    }

    [Fact]
    public async Task Handle_GivenAnsweredSession_SavesAndReturnsScore()
    {
        // Arrange
        var quiz = CreateQuiz();
        var session = GameSession.Start(quiz);
        session.Answer(quiz, quiz.Questions[0].Id.Value, 0);
        var sessionRepository = new Mock<IGameSessionRepository>();
        sessionRepository.Setup(r => r.GetSessionByIdAsync(It.IsAny<GameSessionId>()))
            .ReturnsAsync(session);
        var quizRepository = new Mock<IQuizRepository>();
        quizRepository.Setup(r => r.GetQuizByIdAsync(It.IsAny<QuizId>()))
            .ReturnsAsync(quiz);
        IFinishSessionHandler handler = new FinishSessionHandler(sessionRepository.Object, quizRepository.Object);

        // Act
        var result = await handler.HandleAsync(session.Id.Value);

        // Assert
        Assert.Equal(1, result.CorrectCount);
        Assert.Equal(1, result.TotalCount);
        Assert.Equal(100, result.Percentage);
        Assert.NotNull(session.CompletedAt);
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
        IFinishSessionHandler handler = new FinishSessionHandler(sessionRepository.Object, quizRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(Guid.NewGuid()));
        sessionRepository.Verify(r => r.SaveAsync(), Times.Never);
        quizRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_GivenUnansweredQuestions_ThrowsDomainExceptionWithoutSaving()
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
        IFinishSessionHandler handler = new FinishSessionHandler(sessionRepository.Object, quizRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(session.Id.Value));
        sessionRepository.Verify(r => r.SaveAsync(), Times.Never);
    }
}
