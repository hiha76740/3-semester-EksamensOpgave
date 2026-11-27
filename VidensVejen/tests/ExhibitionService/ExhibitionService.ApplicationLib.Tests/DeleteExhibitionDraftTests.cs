using ExhibitionService.FacadeLib.Commands.DTOs;
using ExhibitionService.ApplicationLib.Handlers;
using ExhibitionService.ApplicationLib.Repositories;
using ExhibitionService.DomainLib.Entities;
using ExhibitionService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using VidensVejen.SharedKernelLib.Exceptions;

namespace ExhibitionService.ApplicationLib.Tests;


public class DeleteExhibitionDraftTests
{
    [Fact]
    public async Task Handle_GivenExistingDraft_DeletesAndSaves()
    {
        // Arrange
        var draft = ExhibitionDraft.Create();
        var repository = new Mock<IExhibitionDraftRepository>();
        repository.Setup(r => r.GetExhibitionDraftByIdAsync(draft.Id)).ReturnsAsync(draft);
        IDeleteExhibitionDraftHandler handler = new DeleteExhibitionDraftHandler(repository.Object, NullLogger<DeleteExhibitionDraftHandler>.Instance);

        // Act
        await handler.HandleAsync(new DeleteExhibitionDraftCommand(draft.Id.Value, [1, 2, 3]));

        // Assert
        repository.Verify(r => r.SetOriginalRowVersion(draft, It.Is<byte[]>(v => v.SequenceEqual(new byte[] { 1, 2, 3 }))), Times.Once);
        repository.Verify(r => r.Delete(draft), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenUnknownDraft_ThrowsWithoutSaving()
    {
        // Arrange
        var repository = new Mock<IExhibitionDraftRepository>();
        IDeleteExhibitionDraftHandler handler = new DeleteExhibitionDraftHandler(repository.Object, NullLogger<DeleteExhibitionDraftHandler>.Instance);

        // Act
        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(new DeleteExhibitionDraftCommand(Guid.NewGuid(), [1, 2, 3])));

        // Assert
        repository.Verify(r => r.SetOriginalRowVersion(It.IsAny<ExhibitionDraft>(), It.IsAny<byte[]>()), Times.Never);
        repository.Verify(r => r.Delete(It.IsAny<ExhibitionDraft>()), Times.Never);
        repository.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenConcurrencyConflict_RethrowsConflict()
    {
        // Arrange
        var draft = ExhibitionDraft.Create();
        var command = new DeleteExhibitionDraftCommand(draft.Id.Value, [1, 2, 3]);
        var repository = new Mock<IExhibitionDraftRepository>();
        repository.Setup(r => r.GetExhibitionDraftByIdAsync(draft.Id)).ReturnsAsync(draft);
        var expected = new ConcurrencyConflictException("Changed by another request", new InvalidOperationException());
        repository.Setup(r => r.SaveAsync()).ThrowsAsync(expected);
        IDeleteExhibitionDraftHandler handler = new DeleteExhibitionDraftHandler(repository.Object, NullLogger<DeleteExhibitionDraftHandler>.Instance);

        // Act
        var actual = await Assert.ThrowsAsync<ConcurrencyConflictException>(() => handler.HandleAsync(command));

        // Assert
        Assert.Same(expected, actual);
        repository.Verify(r => r.SetOriginalRowVersion(draft, command.RowVersion), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Once);
    }
}
