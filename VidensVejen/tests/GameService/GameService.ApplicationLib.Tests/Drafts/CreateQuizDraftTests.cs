using GameService.ApplicationLib.Handlers;
using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities.Drafts;
using GameService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace GameService.ApplicationLib.Tests.Drafts;

public class CreateQuizDraftTests
{
    [Fact]
    public async Task Handle_CreatesEmptyDraftAndReturnsSavedId()
    {
        // Arrange
        var repository = new Mock<IQuizDraftRepository>();
        QuizDraft? savedDraft = null;
        repository.Setup(r => r.AddAsync(It.IsAny<QuizDraft>()))
            .Callback<QuizDraft>(draft => savedDraft = draft)
            .Returns(Task.CompletedTask);
        ICreateQuizDraftHandler handler = new CreateQuizDraftHandler(
            repository.Object, NullLogger<CreateQuizDraftHandler>.Instance);

        // Act
        var id = await handler.HandleAsync();

        // Assert
        Assert.NotNull(savedDraft);
        Assert.NotEqual(Guid.Empty, id);
        Assert.Equal(id, savedDraft.Id.Value);
        Assert.Null(savedDraft.Instruction);
        Assert.Null(savedDraft.InstructionEn);
        Assert.Null(savedDraft.Title);
        Assert.Null(savedDraft.TitleEn);
        Assert.Null(savedDraft.ExhibitionId);
        Assert.Null(savedDraft.QuizLayout);
        Assert.Null(savedDraft.EndMessage);
        Assert.Null(savedDraft.EndMessageEn);
        Assert.Empty(savedDraft.Questions);
        repository.Verify(r => r.AddAsync(savedDraft), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenAddFailure_RethrowsWithoutSaving()
    {
        // Arrange
        var repository = new Mock<IQuizDraftRepository>();
        var expected = new InvalidOperationException("Add failed");
        repository.Setup(r => r.AddAsync(It.IsAny<QuizDraft>())).ThrowsAsync(expected);
        ICreateQuizDraftHandler handler = new CreateQuizDraftHandler(
            repository.Object, NullLogger<CreateQuizDraftHandler>.Instance);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(handler.HandleAsync);

        // Assert
        Assert.Same(expected, exception);
        repository.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenSaveFailure_RethrowsOriginalException()
    {
        // Arrange
        var repository = new Mock<IQuizDraftRepository>();
        var expected = new InvalidOperationException("Save failed");
        repository.Setup(r => r.SaveAsync()).ThrowsAsync(expected);
        ICreateQuizDraftHandler handler = new CreateQuizDraftHandler(
            repository.Object, NullLogger<CreateQuizDraftHandler>.Instance);

        // Act
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(handler.HandleAsync);

        // Assert
        Assert.Same(expected, exception);
        repository.Verify(r => r.AddAsync(It.IsAny<QuizDraft>()), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Once);
    }
}
