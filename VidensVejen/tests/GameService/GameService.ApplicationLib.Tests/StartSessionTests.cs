using GameService.ApplicationLib.Handlers;
using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities;
using GameService.DomainLib.ValueObjects;
using GameService.FacadeLib.Commands.Interfaces;
using Moq;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.ApplicationLib.Tests;

public class StartSessionTests
{
    private static Quiz CreateValidDraft()
    {
        return Quiz.Create("Quiz", Guid.NewGuid(), "vertical", "Done",
            [new NewQuestion("Question", [new Choice("Yes", true), new Choice("No", false)], "Correct", "Incorrect")]);
    }

    [Fact]
    public async Task Handle_GivenPublishedQuiz_SavesSessionAndReturnsQuizWithoutAnswers()
    {
        // Arrange
        var quiz = CreateValidDraft();
        quiz.Publish();
        var quizRepository = new Mock<IQuizRepository>();
        quizRepository.Setup(r => r.GetQuizByIdAsync(It.IsAny<QuizId>()))
            .ReturnsAsync(quiz);
        var sessionRepository = new Mock<IGameSessionRepository>();
        GameSession? savedSession = null;
        sessionRepository.Setup(r => r.AddAsync(It.IsAny<GameSession>()))
            .Callback<GameSession>(s => savedSession = s);
        IStartSessionHandler handler = new StartSessionHandler(quizRepository.Object, sessionRepository.Object);

        // Act
        var result = await handler.HandleAsync(quiz.Id.Value);

        // Assert
        Assert.NotNull(savedSession);
        Assert.Equal(savedSession.Id.Value, result.SessionId);
        Assert.Equal(quiz.Id, savedSession.QuizId);
        Assert.Equal(quiz.Id.Value, result.Quiz.Id);
        var question = Assert.Single(result.Quiz.Questions);
        Assert.Collection(question.Choices,
            choice => { Assert.Equal(0, choice.Index); Assert.Equal("Yes", choice.Text); },
            choice => { Assert.Equal(1, choice.Index); Assert.Equal("No", choice.Text); });
        sessionRepository.Verify(r => r.AddAsync(It.IsAny<GameSession>()), Times.Once);
        sessionRepository.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenDraft_ThrowsDomainExceptionWithoutSaving()
    {
        // Arrange
        var quiz = CreateValidDraft();
        var quizRepository = new Mock<IQuizRepository>();
        quizRepository.Setup(r => r.GetQuizByIdAsync(It.IsAny<QuizId>()))
            .ReturnsAsync(quiz);
        var sessionRepository = new Mock<IGameSessionRepository>();
        IStartSessionHandler handler = new StartSessionHandler(quizRepository.Object, sessionRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(quiz.Id.Value));
        sessionRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_GivenUnknownQuiz_ThrowsNotFoundExceptionWithoutSaving()
    {
        // Arrange
        var quizRepository = new Mock<IQuizRepository>();
        quizRepository.Setup(r => r.GetQuizByIdAsync(It.IsAny<QuizId>()))
            .ReturnsAsync((Quiz?)null);
        var sessionRepository = new Mock<IGameSessionRepository>();
        IStartSessionHandler handler = new StartSessionHandler(quizRepository.Object, sessionRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(Guid.NewGuid()));
        sessionRepository.VerifyNoOtherCalls();
    }
}
