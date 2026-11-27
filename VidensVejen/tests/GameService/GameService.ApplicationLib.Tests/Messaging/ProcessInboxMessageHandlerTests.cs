using System.Data;
using GameService.ApplicationLib.Messaging.Inbox.Handlers;
using GameService.ApplicationLib.Messaging.Inbox.Strategies;
using Moq;
using VidensVejen.BuildingBlocksLib.Messaging.Inbox;
using VidensVejen.BuildingBlocksLib.Messaging.Outbox;
using VidensVejen.BuildingBlocksLib.UnitOfWork;

namespace GameService.ApplicationLib.Tests.Messaging;

public class ProcessInboxMessageHandlerTests
{
    private const string EventType = "Tests.SampleEvent";

    private static InboxMessage CreateMessage()
    {
        return new InboxMessage
        {
            Id = Guid.NewGuid(),
            Type = EventType,
            Topic = "sample",
            Content = "{}",
            RecievedOnUtc = DateTime.UtcNow
        };
    }

    [Fact]
    public async Task Handle_GivenStrategyWithoutOutboxMessage_ProcessesAndCommits()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        var message = CreateMessage();
        var inbox = new Mock<IInboxRepository>();
        var outbox = new Mock<IOutboxRepository>();
        var work = new Mock<IUnitOfWork>();
        var strategy = new Mock<IInboxMessageStrategy>();
        var otherStrategy = new Mock<IInboxMessageStrategy>();
        strategy.SetupGet(s => s.EventType).Returns(EventType);
        otherStrategy.SetupGet(s => s.EventType).Returns("Tests.OtherEvent");
        inbox.Setup(r => r.FindAsync(message.Id, token)).ReturnsAsync(message);
        strategy.Setup(s => s.ApplyAsync(message.Id, message.Content, token))
            .ReturnsAsync((OutboxMessage?)null);
        var handler = new ProcessInboxMessageHandler(inbox.Object, outbox.Object, work.Object,
            [otherStrategy.Object, strategy.Object]);

        // Act
        await handler.HandleAsync(message.Id, token);

        // Assert
        strategy.Verify(s => s.ApplyAsync(message.Id, message.Content, token), Times.Once);
        otherStrategy.Verify(s => s.ApplyAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        Assert.NotNull(message.ProccessedOnUtc);
        outbox.Verify(r => r.AddAsync(It.IsAny<OutboxMessage>()), Times.Never);
        work.Verify(w => w.BeginTransaction(IsolationLevel.Serializable), Times.Once);
        work.Verify(w => w.SaveChangesAsync(), Times.Once);
        work.Verify(w => w.Commit(), Times.Once);
        work.Verify(w => w.Rollback(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenStrategyWithOutboxMessage_ProcessesAndCommits()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        var message = CreateMessage();
        var inbox = new Mock<IInboxRepository>();
        var outbox = new Mock<IOutboxRepository>();
        var work = new Mock<IUnitOfWork>();
        var strategy = new Mock<IInboxMessageStrategy>();
        var otherStrategy = new Mock<IInboxMessageStrategy>();
        strategy.SetupGet(s => s.EventType).Returns(EventType);
        otherStrategy.SetupGet(s => s.EventType).Returns("Tests.OtherEvent");
        inbox.Setup(r => r.FindAsync(message.Id, token)).ReturnsAsync(message);
        var outgoing = new OutboxMessage
        {
            Id = message.Id,
            Type = "Tests.OutgoingEvent",
            Topic = "outgoing",
            Content = "{}",
            CreatedAtUtc = DateTime.UtcNow
        };
        strategy.Setup(s => s.ApplyAsync(message.Id, message.Content, token))
            .ReturnsAsync(outgoing);
        var handler = new ProcessInboxMessageHandler(inbox.Object, outbox.Object, work.Object,
            [otherStrategy.Object, strategy.Object]);

        // Act
        await handler.HandleAsync(message.Id, token);

        // Assert
        strategy.Verify(s => s.ApplyAsync(message.Id, message.Content, token), Times.Once);
        otherStrategy.Verify(s => s.ApplyAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        Assert.NotNull(message.ProccessedOnUtc);
        outbox.Verify(r => r.AddAsync(outgoing), Times.Once);
        work.Verify(w => w.BeginTransaction(IsolationLevel.Serializable), Times.Once);
        work.Verify(w => w.SaveChangesAsync(), Times.Once);
        work.Verify(w => w.Commit(), Times.Once);
        work.Verify(w => w.Rollback(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenMissingMessage_SkipsStrategyAndCommits()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        var message = CreateMessage();

        var inbox = new Mock<IInboxRepository>();
        var outbox = new Mock<IOutboxRepository>();
        var work = new Mock<IUnitOfWork>();
        var strategy = new Mock<IInboxMessageStrategy>();
        strategy.SetupGet(s => s.EventType).Returns(EventType);
        inbox.Setup(r => r.FindAsync(message.Id, token)).ReturnsAsync((InboxMessage?)null);
        var handler = new ProcessInboxMessageHandler(inbox.Object, outbox.Object, work.Object, [strategy.Object]);

        // Act
        await handler.HandleAsync(message.Id, token);

        // Assert
        inbox.Verify(r => r.FindAsync(message.Id, token), Times.Once);
        strategy.Verify(s => s.ApplyAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        outbox.VerifyNoOtherCalls();
        work.Verify(w => w.SaveChangesAsync(), Times.Never);
        work.Verify(w => w.Commit(), Times.Once);
        work.Verify(w => w.Rollback(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenProcessedMessage_SkipsStrategyAndCommits()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        var message = CreateMessage();
        message.SetProccesedAtUtc();
        var inbox = new Mock<IInboxRepository>();
        var outbox = new Mock<IOutboxRepository>();
        var work = new Mock<IUnitOfWork>();
        var strategy = new Mock<IInboxMessageStrategy>();
        strategy.SetupGet(s => s.EventType).Returns(EventType);
        inbox.Setup(r => r.FindAsync(message.Id, token)).ReturnsAsync(message);
        var handler = new ProcessInboxMessageHandler(inbox.Object, outbox.Object, work.Object, [strategy.Object]);

        // Act
        await handler.HandleAsync(message.Id, token);

        // Assert
        inbox.Verify(r => r.FindAsync(message.Id, token), Times.Once);
        strategy.Verify(s => s.ApplyAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        outbox.VerifyNoOtherCalls();
        work.Verify(w => w.SaveChangesAsync(), Times.Never);
        work.Verify(w => w.Commit(), Times.Once);
        work.Verify(w => w.Rollback(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenUnknownTypeFailure_RollsBackAndThrows()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        var message = CreateMessage();
        var inbox = new Mock<IInboxRepository>();
        var outbox = new Mock<IOutboxRepository>();
        var work = new Mock<IUnitOfWork>();
        var strategy = new Mock<IInboxMessageStrategy>();
        strategy.SetupGet(s => s.EventType).Returns("Tests.OtherEvent");
        inbox.Setup(r => r.FindAsync(message.Id, token)).ReturnsAsync(message);
        var handler = new ProcessInboxMessageHandler(inbox.Object, outbox.Object, work.Object, [strategy.Object]);

        // Act & Assert
        var actual = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(message.Id, token));
        Assert.Contains(EventType, actual.Message);
        strategy.Verify(s => s.ApplyAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        Assert.Null(message.ProccessedOnUtc);
        outbox.VerifyNoOtherCalls();
        work.Verify(w => w.SaveChangesAsync(), Times.Never);
        work.Verify(w => w.Commit(), Times.Never);
        work.Verify(w => w.Rollback(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenStrategyFailure_RollsBackAndThrows()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        var message = CreateMessage();
        var inbox = new Mock<IInboxRepository>();
        var outbox = new Mock<IOutboxRepository>();
        var work = new Mock<IUnitOfWork>();
        var strategy = new Mock<IInboxMessageStrategy>();
        strategy.SetupGet(s => s.EventType).Returns(EventType);
        inbox.Setup(r => r.FindAsync(message.Id, token)).ReturnsAsync(message);
        var expected = new InvalidOperationException("Operation failed");
        strategy.Setup(s => s.ApplyAsync(message.Id, message.Content, token)).ThrowsAsync(expected);
        var handler = new ProcessInboxMessageHandler(inbox.Object, outbox.Object, work.Object, [strategy.Object]);

        // Act & Assert
        var actual = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(message.Id, token));
        Assert.Same(expected, actual);
        Assert.Null(message.ProccessedOnUtc);
        outbox.VerifyNoOtherCalls();
        work.Verify(w => w.SaveChangesAsync(), Times.Never);
        work.Verify(w => w.Commit(), Times.Never);
        work.Verify(w => w.Rollback(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenOutboxFailure_RollsBackAndThrows()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        var message = CreateMessage();
        var inbox = new Mock<IInboxRepository>();
        var outbox = new Mock<IOutboxRepository>();
        var work = new Mock<IUnitOfWork>();
        var strategy = new Mock<IInboxMessageStrategy>();
        strategy.SetupGet(s => s.EventType).Returns(EventType);
        inbox.Setup(r => r.FindAsync(message.Id, token)).ReturnsAsync(message);
        var expected = new InvalidOperationException("Operation failed");
        var outgoing = new OutboxMessage { Id = message.Id, Type = "Outgoing", Topic = "outgoing", Content = "{}" };
        strategy.Setup(s => s.ApplyAsync(message.Id, message.Content, token)).ReturnsAsync(outgoing);
        outbox.Setup(r => r.AddAsync(outgoing)).ThrowsAsync(expected);
        var handler = new ProcessInboxMessageHandler(inbox.Object, outbox.Object, work.Object, [strategy.Object]);

        // Act & Assert
        var actual = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(message.Id, token));
        Assert.Same(expected, actual);
        work.Verify(w => w.SaveChangesAsync(), Times.Never);
        work.Verify(w => w.Commit(), Times.Never);
        work.Verify(w => w.Rollback(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenSaveFailure_RollsBackAndThrows()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        var message = CreateMessage();
        var inbox = new Mock<IInboxRepository>();
        var outbox = new Mock<IOutboxRepository>();
        var work = new Mock<IUnitOfWork>();
        var strategy = new Mock<IInboxMessageStrategy>();
        strategy.SetupGet(s => s.EventType).Returns(EventType);
        inbox.Setup(r => r.FindAsync(message.Id, token)).ReturnsAsync(message);
        var expected = new InvalidOperationException("Operation failed");
        strategy.Setup(s => s.ApplyAsync(message.Id, message.Content, token)).ReturnsAsync((OutboxMessage?)null);
        work.Setup(w => w.SaveChangesAsync()).ThrowsAsync(expected);
        var handler = new ProcessInboxMessageHandler(inbox.Object, outbox.Object, work.Object, [strategy.Object]);

        // Act & Assert
        var actual = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(message.Id, token));
        Assert.Same(expected, actual);
        outbox.VerifyNoOtherCalls();
        work.Verify(w => w.SaveChangesAsync(), Times.Once);
        work.Verify(w => w.Commit(), Times.Never);
        work.Verify(w => w.Rollback(), Times.Once);
    }
}
