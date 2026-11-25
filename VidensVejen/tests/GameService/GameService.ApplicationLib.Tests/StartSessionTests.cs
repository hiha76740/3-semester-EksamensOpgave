using GameService.ApplicationLib.Handlers;
using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities.Quizzes;
using GameService.DomainLib.Entities.Sessions;
using GameService.DomainLib.ValueObjects.Quizzes;
using GameService.FacadeLib.Commands.Interfaces;
using Moq;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.ApplicationLib.Tests;

public class StartSessionTests
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
    public async Task Handle_GivenQuiz_SavesSessionAndReturnsQuizWithoutAnswers()
    {
        // Arrange
        var quiz = CreateQuiz();
        var quizRepository = new Mock<IQuizRepository>();
        quizRepository.Setup(r => r.GetQuizByIdAsync(It.IsAny<QuizId>()))
            .ReturnsAsync(quiz);
        var sessionRepository = new Mock<IGameSessionRepository>();
        GameSession? savedSession = null;
        sessionRepository.Setup(r => r.AddAsync(It.IsAny<GameSession>()))
            .Callback<GameSession>(s => savedSession = s)
            .Returns(Task.CompletedTask);
        IStartSessionHandler handler = new StartSessionHandler(quizRepository.Object, sessionRepository.Object);

        // Act
        var result = await handler.HandleAsync(quiz.Id.Value);

        // Assert
        Assert.NotNull(savedSession);
        Assert.Equal(savedSession.Id.Value, result.SessionId);
        Assert.Equal(quiz.Id, savedSession.QuizId);
        Assert.Equal(quiz.Id.Value, result.Quiz.Id);
        Assert.Equal("Quiz", result.Quiz.Title);
        Assert.Equal("Quiz EN", result.Quiz.TitleEn);
        Assert.Equal("vertical", result.Quiz.QuizLayout);
        Assert.Equal("Done", result.Quiz.EndMessage);
        Assert.Empty(savedSession.Answers);
        Assert.Null(savedSession.CompletedAt);
        Assert.Equal("Done EN", result.Quiz.EndMessageEn);
        var question = Assert.Single(result.Quiz.Questions);
        Assert.Equal(quiz.Questions[0].Id.Value, question.Id);
        Assert.Equal("Question", question.Text);
        Assert.Equal("Question EN", question.TextEn);
        Assert.Collection(question.Choices,
            choice => { Assert.Equal(0, choice.Index); Assert.Equal("Yes", choice.Text); Assert.Equal("Yes EN", choice.TextEn); },
            choice => { Assert.Equal(1, choice.Index); Assert.Equal("No", choice.Text); });
        quizRepository.Verify(r => r.GetQuizByIdAsync(quiz.Id), Times.Once);
        sessionRepository.Verify(r => r.AddAsync(It.IsAny<GameSession>()), Times.Once);
        sessionRepository.Verify(r => r.SaveAsync(), Times.Once);
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

    [Fact]
    public async Task Handle_GivenAddFailure_RethrowsOriginalException()
    {
        // Arrange
        var quiz = CreateQuiz();
        var quizRepository = new Mock<IQuizRepository>();
        quizRepository.Setup(r => r.GetQuizByIdAsync(quiz.Id)).ReturnsAsync(quiz);
        var sessionRepository = new Mock<IGameSessionRepository>();
        var expected = new InvalidOperationException("Add failed");
        sessionRepository.Setup(r => r.AddAsync(It.IsAny<GameSession>())).ThrowsAsync(expected);
        IStartSessionHandler handler = new StartSessionHandler(quizRepository.Object, sessionRepository.Object);

        // Act
        var actual = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(quiz.Id.Value));

        // Assert
        Assert.Same(expected, actual);
        quizRepository.Verify(r => r.GetQuizByIdAsync(quiz.Id), Times.Once);
        sessionRepository.Verify(r => r.AddAsync(It.IsAny<GameSession>()), Times.Once);
        sessionRepository.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenSaveFailure_RethrowsOriginalException()
    {
        // Arrange
        var quiz = CreateQuiz();
        var quizRepository = new Mock<IQuizRepository>();
        quizRepository.Setup(r => r.GetQuizByIdAsync(quiz.Id)).ReturnsAsync(quiz);
        var sessionRepository = new Mock<IGameSessionRepository>();
        var expected = new InvalidOperationException("Save failed");
        sessionRepository.Setup(r => r.SaveAsync()).ThrowsAsync(expected);
        IStartSessionHandler handler = new StartSessionHandler(quizRepository.Object, sessionRepository.Object);

        // Act
        var actual = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(quiz.Id.Value));

        // Assert
        Assert.Same(expected, actual);
        quizRepository.Verify(r => r.GetQuizByIdAsync(quiz.Id), Times.Once);
        sessionRepository.Verify(r => r.AddAsync(It.IsAny<GameSession>()), Times.Once);
        sessionRepository.Verify(r => r.SaveAsync(), Times.Once);
    }
}
