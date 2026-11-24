using GameService.ApplicationLib.Handlers;
using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities;
using GameService.FacadeLib.Commands.DTOs;
using GameService.FacadeLib.Commands.Interfaces;
using Moq;

namespace GameService.ApplicationLib.Tests;

public class CreateQuizDraftTests
{
    private static CreateQuizCommand CreateCommand()
    {
        return new CreateQuizCommand("Quiz", Guid.NewGuid(), "vertical",
            [new CreateQuestionCommand("Question",
                [new CreateChoiceCommand("Yes", true), new CreateChoiceCommand("No", false)],
                "Correct", "Incorrect")], "Done");
    }

    [Fact]
    public async Task Handle_GivenValidCommand_MapsDraftAndSaves()
    {
        // Arrange
        var command = CreateCommand();
        var repository = new Mock<IQuizDraftRepository>();
        QuizDraft? savedDraft = null;
        repository.Setup(r => r.AddAsync(It.IsAny<QuizDraft>()))
            .Callback<QuizDraft>(draft => savedDraft = draft);
        ICreateQuizDraftHandler handler = new CreateQuizDraftHandler(repository.Object);

        // Act
        var id = await handler.HandleAsync(command);

        // Assert
        Assert.NotNull(savedDraft);
        Assert.Equal(id, savedDraft.Id.Value);
        Assert.Equal(command.Title, savedDraft.Title);
        Assert.Equal(command.ExhibitionId, savedDraft.ExhibitionId?.Value);
        Assert.Equal(command.QuizLayout, savedDraft.QuizLayout);
        Assert.Equal(command.EndMessage, savedDraft.EndMessage);
        var question = Assert.Single(savedDraft.Questions);
        Assert.Equal(command.Questions![0].Text, question.Text);
        Assert.Collection(question.Choices,
            choice => { Assert.Equal("Yes", choice.Text); Assert.True(choice.IsCorrect); },
            choice => { Assert.Equal("No", choice.Text); Assert.False(choice.IsCorrect); });
        repository.Verify(r => r.AddAsync(savedDraft), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenEmptyCommand_SavesEmptyDraft()
    {
        // Arrange
        var command = new CreateQuizCommand(null, null, null, null, null);
        var repository = new Mock<IQuizDraftRepository>();
        QuizDraft? savedDraft = null;
        repository.Setup(r => r.AddAsync(It.IsAny<QuizDraft>()))
            .Callback<QuizDraft>(draft => savedDraft = draft);
        ICreateQuizDraftHandler handler = new CreateQuizDraftHandler(repository.Object);

        // Act
        await handler.HandleAsync(command);

        // Assert
        Assert.NotNull(savedDraft);
        Assert.Null(savedDraft.Title);
        Assert.Null(savedDraft.ExhibitionId);
        Assert.Empty(savedDraft.Questions);
        repository.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenSaveFailure_RethrowsOriginalException()
    {
        // Arrange
        var command = CreateCommand();
        var repository = new Mock<IQuizDraftRepository>();
        var expected = new InvalidOperationException("Write failed");
        repository.Setup(r => r.SaveAsync()).ThrowsAsync(expected);
        ICreateQuizDraftHandler handler = new CreateQuizDraftHandler(repository.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));
        Assert.Same(expected, exception);
        repository.Verify(r => r.AddAsync(It.IsAny<QuizDraft>()), Times.Once);
    }
}
