using GameService.ApplicationLib.Handlers;
using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities.Drafts;
using GameService.FacadeLib.Commands.DTOs;
using GameService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.ApplicationLib.Tests;

public class DeleteQuizDraftTests
{
    [Fact]
    public async Task Handle_GivenExistingEntity_SetsRowVersionDeletesAndSaves()
    {
        // Arrange
        var entity = QuizDraft.Create();
        var command = new DeleteQuizDraftCommand(entity.Id.Value, [1, 2, 3]);
        var repository = new Mock<IQuizDraftRepository>(MockBehavior.Strict);
        var sequence = new MockSequence();
        repository.InSequence(sequence).Setup(r => r.GetDraftByIdAsync(entity.Id)).ReturnsAsync(entity);
        repository.InSequence(sequence).Setup(r => r.SetOriginalRowVersion(entity, command.RowVersion));
        repository.InSequence(sequence).Setup(r => r.Delete(entity));
        repository.InSequence(sequence).Setup(r => r.SaveAsync()).Returns(Task.CompletedTask);
        IDeleteDraftHandler handler = new DeleteQuizDraftHandler(repository.Object, NullLogger<DeleteQuizDraftHandler>.Instance);

        // Act
        await handler.HandleAsync(command);

        // Assert
        repository.Verify(r => r.GetDraftByIdAsync(entity.Id), Times.Once);
        repository.Verify(r => r.SetOriginalRowVersion(entity, command.RowVersion), Times.Once);
        repository.Verify(r => r.Delete(entity), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenUnknownEntity_ThrowsWithoutDeletingOrSaving()
    {
        // Arrange
        var command = new DeleteQuizDraftCommand(Guid.NewGuid(), [1, 2, 3]);
        var id = new QuizDraftId(command.DraftId);
        var repository = new Mock<IQuizDraftRepository>();
        repository.Setup(r => r.GetDraftByIdAsync(id)).ReturnsAsync((QuizDraft?)null);
        IDeleteDraftHandler handler = new DeleteQuizDraftHandler(repository.Object, NullLogger<DeleteQuizDraftHandler>.Instance);

        // Act
        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command));

        // Assert
        repository.Verify(r => r.GetDraftByIdAsync(id), Times.Once);
        repository.Verify(r => r.SetOriginalRowVersion(It.IsAny<QuizDraft>(), It.IsAny<byte[]>()), Times.Never);
        repository.Verify(r => r.Delete(It.IsAny<QuizDraft>()), Times.Never);
        repository.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenRowVersionFailure_RethrowsOriginalException()
    {
        // Arrange
        var entity = QuizDraft.Create();
        var command = new DeleteQuizDraftCommand(entity.Id.Value, [1, 2, 3]);
        var repository = new Mock<IQuizDraftRepository>();
        repository.Setup(r => r.GetDraftByIdAsync(entity.Id)).ReturnsAsync(entity);
        var expected = new InvalidOperationException("RowVersion failed");
        repository.Setup(r => r.SetOriginalRowVersion(entity, command.RowVersion)).Throws(expected);
        IDeleteDraftHandler handler = new DeleteQuizDraftHandler(repository.Object, NullLogger<DeleteQuizDraftHandler>.Instance);

        // Act
        var actual = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));

        // Assert
        Assert.Same(expected, actual);
        repository.Verify(r => r.Delete(entity), Times.Never);
        repository.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenDeleteFailure_RethrowsOriginalException()
    {
        // Arrange
        var entity = QuizDraft.Create();
        var command = new DeleteQuizDraftCommand(entity.Id.Value, [1, 2, 3]);
        var repository = new Mock<IQuizDraftRepository>();
        repository.Setup(r => r.GetDraftByIdAsync(entity.Id)).ReturnsAsync(entity);
        var expected = new InvalidOperationException("Delete failed");
        repository.Setup(r => r.Delete(entity)).Throws(expected);
        IDeleteDraftHandler handler = new DeleteQuizDraftHandler(repository.Object, NullLogger<DeleteQuizDraftHandler>.Instance);

        // Act
        var actual = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));

        // Assert
        Assert.Same(expected, actual);
        repository.Verify(r => r.Delete(entity), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenSaveFailure_RethrowsOriginalException()
    {
        // Arrange
        var entity = QuizDraft.Create();
        var command = new DeleteQuizDraftCommand(entity.Id.Value, [1, 2, 3]);
        var repository = new Mock<IQuizDraftRepository>();
        repository.Setup(r => r.GetDraftByIdAsync(entity.Id)).ReturnsAsync(entity);
        var expected = new InvalidOperationException("Save failed");
        repository.Setup(r => r.SaveAsync()).ThrowsAsync(expected);
        IDeleteDraftHandler handler = new DeleteQuizDraftHandler(repository.Object, NullLogger<DeleteQuizDraftHandler>.Instance);

        // Act
        var actual = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));

        // Assert
        Assert.Same(expected, actual);
        repository.Verify(r => r.Delete(entity), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Once);
    }
}
