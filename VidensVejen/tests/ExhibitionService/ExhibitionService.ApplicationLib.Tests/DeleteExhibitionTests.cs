//using System.Data;
//using System.Text.Json;
//using VidensVejen.BuildingBlocksLib.UnitOfWork;
//using VidensVejen.BuildingBlocksLib.Messaging.Outbox;
//using VidensVejen.ContractsLib.Events;
//using ExhibitionService.ApplicationLib.Handlers;
//using ExhibitionService.ApplicationLib.Repositories;
//using ExhibitionService.DomainLib.Entities;
//using ExhibitionService.FacadeLib.Commands.Interfaces;
//using Microsoft.Extensions.Logging.Abstractions;
//using Moq;
//using VidensVejen.SharedKernelLib.Exceptions;

//namespace ExhibitionService.ApplicationLib.Tests;


//public class DeleteExhibitionTests
//{
//    [Fact]
//    public async Task Handle_GivenExistingExhibition_DeletesQueuesEventAndCommits()
//    {
//        // Arrange
//        var exhibition = ExhibitionTestData.Published();
//        var repository = new Mock<IExhibitionRepository>();
//        repository.Setup(r => r.GetExhibitionById(exhibition.Id)).ReturnsAsync(exhibition);
//        var outbox = new Mock<IOutboxRepository>();
//        OutboxMessage? queued = null;
//        outbox.Setup(r => r.AddAsync(It.IsAny<OutboxMessage>()))
//            .Callback<OutboxMessage>(message => queued = message).Returns(Task.CompletedTask);
//        var work = new Mock<IUnitOfWork>();
//        IDeleteExhibitionHandler handler = new DeleteExhibitionHandler(repository.Object, outbox.Object, work.Object, NullLogger<DeleteExhibitionHandler>.Instance);

//        // Act
//        await handler.HandleAsync(exhibition.Id.Value);

//        // Assert
//        Assert.NotNull(queued);
//        Assert.NotEqual(Guid.Empty, queued.Id);
//        Assert.Equal("quiz-deletion", queued.Topic);
//        Assert.Equal(typeof(QuizDeletionIntegrationEvent).FullName, queued.Type);
//        var integrationEvent = JsonSerializer.Deserialize<QuizDeletionIntegrationEvent>(queued.Content);
//        Assert.NotNull(integrationEvent);
//        Assert.Equal(queued.Id, integrationEvent.EventId);
//        Assert.Equal(exhibition.Id.Value, integrationEvent.ExhibitionId);
//        repository.Verify(r => r.Delete(exhibition), Times.Once);
//        repository.Verify(r => r.SaveAsync(), Times.Never);
//        work.Verify(w => w.BeginTransaction(IsolationLevel.Serializable), Times.Once);
//        work.Verify(w => w.SaveChangesAsync(), Times.Once);
//        work.Verify(w => w.Commit(), Times.Once);
//        work.Verify(w => w.Rollback(), Times.Never);
//    }

//    [Fact]
//    public async Task Handle_GivenUnknownExhibition_RollsBackWithoutWriting()
//    {
//        // Arrange
//        var repository = new Mock<IExhibitionRepository>();
//        var outbox = new Mock<IOutboxRepository>();
//        var work = new Mock<IUnitOfWork>();
//        IDeleteExhibitionHandler handler = new DeleteExhibitionHandler(repository.Object, outbox.Object, work.Object, NullLogger<DeleteExhibitionHandler>.Instance);

//        // Act
//        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(Guid.NewGuid()));

//        // Assert
//        repository.Verify(r => r.Delete(It.IsAny<Exhibition>()), Times.Never);
//        outbox.VerifyNoOtherCalls();
//        work.Verify(w => w.SaveChangesAsync(), Times.Never);
//        work.Verify(w => w.Commit(), Times.Never);
//        work.Verify(w => w.Rollback(), Times.Once);
//    }

//    [Fact]
//    public async Task Handle_GivenOutboxFailure_RollsBackAndRethrows()
//    {
//        // Arrange
//        var exhibition = ExhibitionTestData.Published();
//        var repository = new Mock<IExhibitionRepository>();
//        repository.Setup(r => r.GetExhibitionById(exhibition.Id)).ReturnsAsync(exhibition);
//        var outbox = new Mock<IOutboxRepository>();
//        var work = new Mock<IUnitOfWork>();
//        var expected = new InvalidOperationException("Outbox failed");
//        outbox.Setup(r => r.AddAsync(It.IsAny<OutboxMessage>())).ThrowsAsync(expected);
//        IDeleteExhibitionHandler handler = new DeleteExhibitionHandler(repository.Object, outbox.Object, work.Object, NullLogger<DeleteExhibitionHandler>.Instance);

//        // Act
//        var actual = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(exhibition.Id.Value));

//        // Assert
//        Assert.Same(expected, actual);
//        work.Verify(w => w.SaveChangesAsync(), Times.Never);
//        work.Verify(w => w.Commit(), Times.Never);
//        work.Verify(w => w.Rollback(), Times.Once);
//    }

//    [Fact]
//    public async Task Handle_GivenSaveFailure_RollsBackAndRethrows()
//    {
//        // Arrange
//        var exhibition = ExhibitionTestData.Published();
//        var repository = new Mock<IExhibitionRepository>();
//        repository.Setup(r => r.GetExhibitionById(exhibition.Id)).ReturnsAsync(exhibition);
//        var outbox = new Mock<IOutboxRepository>();
//        var work = new Mock<IUnitOfWork>();
//        var expected = new InvalidOperationException("Save failed");
//        work.Setup(w => w.SaveChangesAsync()).ThrowsAsync(expected);
//        IDeleteExhibitionHandler handler = new DeleteExhibitionHandler(repository.Object, outbox.Object, work.Object, NullLogger<DeleteExhibitionHandler>.Instance);

//        // Act
//        var actual = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(exhibition.Id.Value));

//        // Assert
//        Assert.Same(expected, actual);
//        work.Verify(w => w.SaveChangesAsync(), Times.Once);
//        work.Verify(w => w.Commit(), Times.Never);
//        work.Verify(w => w.Rollback(), Times.Once);
//    }
//}
