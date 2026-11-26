using System.Text.Json;
using GameService.ApplicationLib.Handlers;
using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities.Quizzes;
using GameService.DomainLib.Entities.Sessions;
using GameService.DomainLib.ValueObjects.Quizzes;
using GameService.FacadeLib.Commands.Interfaces;
using Moq;
using VidensVejen.BuildingBlocksLib.Messaging.Outbox;
using VidensVejen.ContractsLib.Events;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.ApplicationLib.Tests;

public class FinishSessionTests
{
    private static Quiz CreateQuiz()
    {
        return Quiz.Create("Quiz", "Quiz EN", Guid.NewGuid(), "vertical", "Done", "Done EN", [new NewQuestion("Question", "Question EN", [new Choice("Yes", "Yes EN", true, "Correct", "Correct EN"), new Choice("No", "No EN", false, "Incorrect", "Incorrect EN")])], "Instructions", "Instructions EN");
    }

    [Theory]
    [InlineData(0, 1, 100)]
    [InlineData(1, 0, 0)]
    public async Task Handle_GivenAnsweredSession_SavesEventAndReturnsScore(int choiceIndex, int correctCount, int percentage)
    {
        // Arrange
        var quiz = CreateQuiz();
        var session = GameSession.Start(quiz);
        session.Answer(quiz, quiz.Questions[0].Id.Value, choiceIndex);
        var sessionRepository = new Mock<IGameSessionRepository>();
        sessionRepository.Setup(r => r.GetSessionByIdAsync(It.IsAny<GameSessionId>()))
            .ReturnsAsync(session);
        var quizRepository = new Mock<IQuizRepository>();
        quizRepository.Setup(r => r.GetQuizByIdAsync(It.IsAny<QuizId>()))
            .ReturnsAsync(quiz);
        var outbox = new Mock<IOutboxRepository>();
        OutboxMessage? queuedMessage = null;
        outbox.Setup(o => o.AddAsync(It.IsAny<OutboxMessage>()))
            .Callback<OutboxMessage>(message => queuedMessage = message)
            .Returns(Task.CompletedTask);
        IFinishSessionHandler handler = new FinishSessionHandler(sessionRepository.Object, quizRepository.Object, outbox.Object);

        // Act
        var result = await handler.HandleAsync(session.Id.Value);

        // Assert
        Assert.Equal(correctCount, result.CorrectCount);
        Assert.Equal(1, result.TotalCount);
        Assert.Equal(percentage, result.Percentage);
        Assert.NotNull(session.CompletedAt);
        outbox.Verify(o => o.AddAsync(It.Is<OutboxMessage>(m =>
            m.Topic == "game-completed" &&
            m.Type == typeof(GameCompletedIntegrationEvent).FullName)), Times.Once);
        Assert.NotNull(queuedMessage);
        Assert.NotEqual(Guid.Empty, queuedMessage.Id);
        Assert.Null(queuedMessage.PublishedAtUtc);
        var integrationEvent = JsonSerializer.Deserialize<GameCompletedIntegrationEvent>(queuedMessage.Content);
        Assert.NotNull(integrationEvent);
        Assert.Equal(queuedMessage.Id, integrationEvent.EventId);
        Assert.Equal("game-completed", integrationEvent.EventTopic);
        Assert.Equal(session.Id.Value, integrationEvent.SessionId);
        Assert.Equal(quiz.Id.Value, integrationEvent.QuizId);
        Assert.Equal(quiz.Title, integrationEvent.QuizTitle);
        Assert.Equal(quiz.ExhibitionId.Value, integrationEvent.ExhibitionId);
        Assert.Equal(correctCount, integrationEvent.CorrectCount);
        Assert.Equal(1, integrationEvent.TotalCount);
        Assert.Equal(session.CompletedAt!.Value, integrationEvent.CompletedAt);
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
        var outbox = new Mock<IOutboxRepository>();
        IFinishSessionHandler handler = new FinishSessionHandler(sessionRepository.Object, quizRepository.Object, outbox.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(Guid.NewGuid()));
        outbox.VerifyNoOtherCalls();
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
        var outbox = new Mock<IOutboxRepository>();
        IFinishSessionHandler handler = new FinishSessionHandler(sessionRepository.Object, quizRepository.Object, outbox.Object);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(session.Id.Value));
        outbox.VerifyNoOtherCalls();
        sessionRepository.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenUnknownQuiz_ThrowsWithoutReturningResult()
    {
        // Arrange
        var quiz = CreateQuiz();
        var session = GameSession.Start(quiz);
        session.Answer(quiz, quiz.Questions[0].Id.Value, 0);
        var sessionRepository = new Mock<IGameSessionRepository>();
        sessionRepository.Setup(r => r.GetSessionByIdAsync(session.Id)).ReturnsAsync(session);
        var quizRepository = new Mock<IQuizRepository>();
        quizRepository.Setup(r => r.GetQuizByIdAsync(session.QuizId)).ReturnsAsync((Quiz?)null);
        var outbox = new Mock<IOutboxRepository>();
        IFinishSessionHandler handler = new FinishSessionHandler(sessionRepository.Object, quizRepository.Object, outbox.Object);

        // Act
        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(session.Id.Value));

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
        session.Answer(quiz, quiz.Questions[0].Id.Value, 0);
        var sessionRepository = new Mock<IGameSessionRepository>();
        sessionRepository.Setup(r => r.GetSessionByIdAsync(session.Id)).ReturnsAsync(session);
        var quizRepository = new Mock<IQuizRepository>();
        quizRepository.Setup(r => r.GetQuizByIdAsync(session.QuizId)).ReturnsAsync(quiz);
        var expected = new InvalidOperationException("Save failed");
        sessionRepository.Setup(r => r.SaveAsync()).ThrowsAsync(expected);
        var outbox = new Mock<IOutboxRepository>();
        IFinishSessionHandler handler = new FinishSessionHandler(sessionRepository.Object, quizRepository.Object, outbox.Object);

        // Act
        var actual = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(session.Id.Value));

        // Assert
        Assert.Same(expected, actual);
        sessionRepository.Verify(r => r.GetSessionByIdAsync(session.Id), Times.Once);
        quizRepository.Verify(r => r.GetQuizByIdAsync(session.QuizId), Times.Once);
        sessionRepository.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenOutboxFailure_RethrowsWithoutSaving()
    {
        // Arrange
        var quiz = CreateQuiz();
        var session = GameSession.Start(quiz);
        session.Answer(quiz, quiz.Questions[0].Id.Value, 0);
        var sessions = new Mock<IGameSessionRepository>();
        sessions.Setup(r => r.GetSessionByIdAsync(session.Id)).ReturnsAsync(session);
        var quizzes = new Mock<IQuizRepository>();
        quizzes.Setup(r => r.GetQuizByIdAsync(quiz.Id)).ReturnsAsync(quiz);
        var outbox = new Mock<IOutboxRepository>();
        var expected = new InvalidOperationException("Outbox failed");
        outbox.Setup(r => r.AddAsync(It.IsAny<OutboxMessage>())).ThrowsAsync(expected);
        IFinishSessionHandler handler = new FinishSessionHandler(sessions.Object, quizzes.Object, outbox.Object);

        // Act
        var actual = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(session.Id.Value));

        // Assert
        Assert.Same(expected, actual);
        outbox.Verify(r => r.AddAsync(It.IsAny<OutboxMessage>()), Times.Once);
        sessions.Verify(r => r.SaveAsync(), Times.Never);
    }
}
