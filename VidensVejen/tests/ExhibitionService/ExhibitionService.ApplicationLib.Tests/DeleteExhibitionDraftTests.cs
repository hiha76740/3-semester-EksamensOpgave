//using ExhibitionService.ApplicationLib.Handlers;
//using ExhibitionService.ApplicationLib.Repositories;
//using ExhibitionService.DomainLib.Entities;
//using ExhibitionService.FacadeLib.Commands.Interfaces;
//using Microsoft.Extensions.Logging.Abstractions;
//using Moq;
//using VidensVejen.SharedKernelLib.Exceptions;

//namespace ExhibitionService.ApplicationLib.Tests;


//public class DeleteExhibitionDraftTests
//{
//    [Fact]
//    public async Task Handle_GivenExistingDraft_DeletesAndSaves()
//    {
//        // Arrange
//        var draft = ExhibitionDraft.Create();
//        var repository = new Mock<IExhibitionDraftRepository>();
//        repository.Setup(r => r.GetExhibitionDraftByIdAsync(draft.Id)).ReturnsAsync(draft);
//        IDeleteExhibitionDraftHandler handler = new DeleteExhibitionDraftHandler(repository.Object, NullLogger<DeleteExhibitionDraftHandler>.Instance);

//        // Act
//        await handler.HandleAsync(draft.Id.Value);

//        // Assert
//        repository.Verify(r => r.Delete(draft), Times.Once);
//        repository.Verify(r => r.SaveAsync(), Times.Once);
//    }

//    [Fact]
//    public async Task Handle_GivenUnknownDraft_ThrowsWithoutSaving()
//    {
//        // Arrange
//        var repository = new Mock<IExhibitionDraftRepository>();
//        IDeleteExhibitionDraftHandler handler = new DeleteExhibitionDraftHandler(repository.Object, NullLogger<DeleteExhibitionDraftHandler>.Instance);

//        // Act
//        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(Guid.NewGuid()));

//        // Assert
//        repository.Verify(r => r.Delete(It.IsAny<ExhibitionDraft>()), Times.Never);
//        repository.Verify(r => r.SaveAsync(), Times.Never);
//    }
//}
