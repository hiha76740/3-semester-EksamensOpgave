using System.Text.Json;
using GameService.ApplicationLib.Messaging.Outbox;
using Moq;
using VidensVejen.BuildingBlocksLib.Messaging.Outbox;
using VidensVejen.BuildingBlocksLib.Messaging.Publisher;
using VidensVejen.BuildingBlocksLib.UnitOfWork;
using VidensVejen.ContractsLib.Events;

namespace GameService.ApplicationLib.Tests.Messaging;

public class PublishOutboxMessageTests
{
    private static OutboxMessage CreateMessage()
    {
        var integrationEvent = new GameCompletedIntegrationEvent(
            Guid.NewGuid(), "game-completed", Guid.NewGuid(), Guid.NewGuid(), "Quiz",
            Guid.NewGuid(), 1, 2, DateTime.UtcNow);
        return new OutboxMessage
        {
            Id = integrationEvent.EventId,
            Type = typeof(GameCompletedIntegrationEvent).FullName!,
            Topic = integrationEvent.EventTopic,
            Content = JsonSerializer.Serialize(integrationEvent),
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    [Fact]
    public async Task Handle_GivenPendingMessage_PublishesEventMarksPublishedAndSaves()
    {
        // Arrange
        var message = CreateMessage();
        using var cancellation = new CancellationTokenSource();
        var token = cancellation.Token;
        var outbox = new Mock<IOutboxRepository>(MockBehavior.Strict);
        var publisher = new Mock<IOutboxEventPublisher>(MockBehavior.Strict);
        var unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
        object? publishedEvent = null;
        var sequence = new MockSequence();
        outbox.InSequence(sequence).Setup(r => r.FindAsync(message.Id, token)).ReturnsAsync(message);
        publisher.InSequence(sequence).Setup(p => p.PublishAsync(message.Topic, It.IsAny<object>()))
            .Callback<string, object>((_, integrationEvent) =>
            {
                Assert.Null(message.PublishedAtUtc);
                publishedEvent = integrationEvent;
            }).Returns(Task.CompletedTask);
        unitOfWork.InSequence(sequence).Setup(u => u.SaveChangesAsync())
            .Callback(() => Assert.NotNull(message.PublishedAtUtc))
            .Returns(Task.CompletedTask);
        var handler = new PublishOutboxMessageHandler(outbox.Object, unitOfWork.Object, publisher.Object);
        var before = DateTime.UtcNow;

        // Act
        await handler.HandleAsync(message.Id, token);

        // Assert
        var actual = Assert.IsType<GameCompletedIntegrationEvent>(publishedEvent);
        Assert.Equal(JsonSerializer.Deserialize<GameCompletedIntegrationEvent>(message.Content), actual);
        Assert.NotNull(message.PublishedAtUtc);
        Assert.InRange(message.PublishedAtUtc.Value, before, DateTime.UtcNow);
        outbox.Verify(r => r.FindAsync(message.Id, token), Times.Once);
        publisher.Verify(p => p.PublishAsync(message.Topic, It.IsAny<object>()), Times.Once);
        unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenUnknownMessage_DoesNotPublishOrSave()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var outbox = new Mock<IOutboxRepository>();
        outbox.Setup(r => r.FindAsync(eventId, CancellationToken.None)).ReturnsAsync((OutboxMessage?)null);
        var publisher = new Mock<IOutboxEventPublisher>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var handler = new PublishOutboxMessageHandler(outbox.Object, unitOfWork.Object, publisher.Object);

        // Act
        await handler.HandleAsync(eventId, CancellationToken.None);

        // Assert
        outbox.Verify(r => r.FindAsync(eventId, CancellationToken.None), Times.Once);
        publisher.VerifyNoOtherCalls();
        unitOfWork.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_GivenAlreadyPublishedMessage_DoesNotPublishAgain()
    {
        // Arrange
        var message = CreateMessage();
        message.SetPublishAtUtc();
        var publishedAt = message.PublishedAtUtc;
        var outbox = new Mock<IOutboxRepository>();
        outbox.Setup(r => r.FindAsync(message.Id, CancellationToken.None)).ReturnsAsync(message);
        var publisher = new Mock<IOutboxEventPublisher>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var handler = new PublishOutboxMessageHandler(outbox.Object, unitOfWork.Object, publisher.Object);

        // Act
        await handler.HandleAsync(message.Id, CancellationToken.None);

        // Assert
        Assert.Equal(publishedAt, message.PublishedAtUtc);
        publisher.VerifyNoOtherCalls();
        unitOfWork.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_GivenPublishFailure_DoesNotMarkPublishedOrSave()
    {
        // Arrange
        var message = CreateMessage();
        var outbox = new Mock<IOutboxRepository>();
        outbox.Setup(r => r.FindAsync(message.Id, CancellationToken.None)).ReturnsAsync(message);
        var publisher = new Mock<IOutboxEventPublisher>();
        var expected = new InvalidOperationException("Publish failed");
        publisher.Setup(p => p.PublishAsync(message.Topic, It.IsAny<object>())).ThrowsAsync(expected);
        var unitOfWork = new Mock<IUnitOfWork>();
        var handler = new PublishOutboxMessageHandler(outbox.Object, unitOfWork.Object, publisher.Object);

        // Act
        var actual = await Assert.ThrowsAsync<InvalidOperationException>(
            () => handler.HandleAsync(message.Id, CancellationToken.None));

        // Assert
        Assert.Same(expected, actual);
        Assert.Null(message.PublishedAtUtc);
        unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenSaveFailure_RethrowsOriginalException()
    {
        // Arrange
        var message = CreateMessage();
        var outbox = new Mock<IOutboxRepository>();
        outbox.Setup(r => r.FindAsync(message.Id, CancellationToken.None)).ReturnsAsync(message);
        var publisher = new Mock<IOutboxEventPublisher>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var expected = new InvalidOperationException("Save failed");
        unitOfWork.Setup(u => u.SaveChangesAsync()).ThrowsAsync(expected);
        var handler = new PublishOutboxMessageHandler(outbox.Object, unitOfWork.Object, publisher.Object);

        // Act
        var actual = await Assert.ThrowsAsync<InvalidOperationException>(
            () => handler.HandleAsync(message.Id, CancellationToken.None));

        // Assert
        Assert.Same(expected, actual);
        publisher.Verify(p => p.PublishAsync(message.Topic, It.IsAny<object>()), Times.Once);
        unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenMalformedJson_DoesNotPublishOrSave()
    {
        // Arrange
        var message = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = typeof(GameCompletedIntegrationEvent).FullName!,
            Topic = "game-completed",
            Content = "invalid JSON"
        };
        var outbox = new Mock<IOutboxRepository>();
        outbox.Setup(r => r.FindAsync(message.Id, CancellationToken.None)).ReturnsAsync(message);
        var publisher = new Mock<IOutboxEventPublisher>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var handler = new PublishOutboxMessageHandler(outbox.Object, unitOfWork.Object, publisher.Object);

        // Act
        await Assert.ThrowsAsync<JsonException>(() => handler.HandleAsync(message.Id, CancellationToken.None));

        // Assert
        Assert.Null(message.PublishedAtUtc);
        publisher.VerifyNoOtherCalls();
        unitOfWork.VerifyNoOtherCalls();
    }
}
