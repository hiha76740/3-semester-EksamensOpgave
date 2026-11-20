using GameService.ApplicationLib.Handlers;
using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities;
using GameService.FacadeLib.Commands.DTOs;
using GameService.FacadeLib.Commands.Interfaces;
using Moq;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.ApplicationLib.Tests;

public class CreateQuizTests
{
    private static CreateQuizCommand CreateCommand()
    {
        return new CreateQuizCommand("Quiz", Guid.NewGuid(), "vertical",
            [new CreateQuestionCommand("Question",
                [new CreateChoiceCommand("Yes", true), new CreateChoiceCommand("No", false)],
                "Correct", "Incorrect")], "Done");
    }

    [Fact]
    public async Task Handle_GivenValidCommand_MapsQuizAndSaves()
    {
        // Arrange
        var command = CreateCommand();
        var repository = new Mock<IQuizRepository>();
        Quiz? savedQuiz = null;
        repository.Setup(r => r.AddAsync(It.IsAny<Quiz>()))
            .Callback<Quiz>(quiz => savedQuiz = quiz);
        ICreateQuizHandler handler = new CreateQuizHandler(repository.Object);

        // Act
        var id = await handler.HandleAsync(command);

        // Assert
        Assert.NotNull(savedQuiz);
        Assert.NotEqual(Guid.Empty, id);
        Assert.Equal(id, savedQuiz.Id.Value);
        Assert.Equal(command.ExhibitionId, savedQuiz.ExhibitionId.Value);
        Assert.Equal(command.Title, savedQuiz.Title);
        Assert.Equal(command.QuizLayout, savedQuiz.QuizLayout);
        Assert.Equal(command.EndMessage, savedQuiz.EndMessage);
        var question = Assert.Single(savedQuiz.Questions);
        Assert.Equal(command.Questions[0].Text, question.Text);
        Assert.Equal(command.Questions[0].CorrectFeedback, question.CorrectFeedback);
        Assert.Equal(command.Questions[0].IncorrectFeedback, question.IncorrectFeedback);
        Assert.Collection(question.Choices,
            choice => { Assert.Equal("Yes", choice.Text); Assert.True(choice.IsCorrect); },
            choice => { Assert.Equal("No", choice.Text); Assert.False(choice.IsCorrect); });
        repository.Verify(r => r.AddAsync(savedQuiz), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenInvalidQuiz_ThrowsWithoutWriting()
    {
        // Arrange
        var command = CreateCommand() with { Title = "" };
        var repository = new Mock<IQuizRepository>();
        ICreateQuizHandler handler = new CreateQuizHandler(repository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(command));
        repository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_GivenAddFailure_RethrowsOriginalException()
    {
        // Arrange
        var command = CreateCommand();
        var repository = new Mock<IQuizRepository>();
        var expected = new InvalidOperationException("Write failed");
        repository.Setup(r => r.AddAsync(It.IsAny<Quiz>())).ThrowsAsync(expected);
        ICreateQuizHandler handler = new CreateQuizHandler(repository.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));
        Assert.Same(expected, exception);
        repository.Verify(r => r.AddAsync(It.IsAny<Quiz>()), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenSaveFailure_RethrowsOriginalException()
    {
        // Arrange
        var command = CreateCommand();
        var repository = new Mock<IQuizRepository>();
        var expected = new InvalidOperationException("Write failed");
        repository.Setup(r => r.SaveAsync()).ThrowsAsync(expected);
        ICreateQuizHandler handler = new CreateQuizHandler(repository.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));
        Assert.Same(expected, exception);
        repository.Verify(r => r.AddAsync(It.IsAny<Quiz>()), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Once);
    }
}
