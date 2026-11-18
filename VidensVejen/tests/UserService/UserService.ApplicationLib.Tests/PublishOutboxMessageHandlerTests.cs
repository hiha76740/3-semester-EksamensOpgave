using System.Text.Json;
using UserService.ApplicationLib.Messaging.Outbox;
using Moq;
using VidensVejen.BuildingBlocksLib.Messaging.Outbox;
using VidensVejen.BuildingBlocksLib.Messaging.Publisher;
using VidensVejen.BuildingBlocksLib.UnitOfWork;
using VidensVejen.ContractsLib.Events;

namespace UserService.ApplicationLib.Tests;

public class PublishOutboxMessageHandlerTests
{
    private const string Topic = "user-activate";

    private static OutboxMessage CreateMessage(UserActivateIntegrationEvent integrationEvent)
    {
        return new OutboxMessage
        {
            Id = integrationEvent.EventId,
            Type = typeof(UserActivateIntegrationEvent).FullName!,
            Topic = Topic,
            Content = JsonSerializer.Serialize(integrationEvent),
            CreatedAtUtc = DateTime.UtcNow
        };
    }

    [Fact]
    public async Task Handle_GivenPendingMessage_PublishesEventAndSavesTimestamp()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        var integrationEvent = new UserActivateIntegrationEvent(Guid.NewGuid(), Topic, Guid.NewGuid());
        var message = CreateMessage(integrationEvent);
        var outbox = new Mock<IOutboxRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var publisher = new Mock<IOutboxEventPublisher>();
        outbox.Setup(repository => repository.FindAsync(message.Id, token)).ReturnsAsync(message);
        var handler = new PublishOutboxMessageHandler(outbox.Object, unitOfWork.Object, publisher.Object);

        // Act
        await handler.HandleAsync(message.Id, token);

        // Assert
        publisher.Verify(service => service.PublishAsync<object>(Topic, integrationEvent), Times.Once);
        Assert.NotNull(message.PublishedAtUtc);
        unitOfWork.Verify(work => work.SaveChangesAsync(), Times.Once);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Handle_GivenMissingOrPublishedMessage_SkipsPublishing(bool alreadyPublished)
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        var eventId = Guid.NewGuid();
        var outbox = new Mock<IOutboxRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var publisher = new Mock<IOutboxEventPublisher>();
        OutboxMessage? message = null;
        if (alreadyPublished)
        {
            var integrationEvent = new UserActivateIntegrationEvent(eventId, Topic, Guid.NewGuid());
            message = CreateMessage(integrationEvent);
            message.SetPublishAtUtc();
        }

        outbox.Setup(repository => repository.FindAsync(eventId, token)).ReturnsAsync(message);
        var handler = new PublishOutboxMessageHandler(outbox.Object, unitOfWork.Object, publisher.Object);

        // Act
        await handler.HandleAsync(eventId, token);

        // Assert
        outbox.Verify(repository => repository.FindAsync(eventId, token), Times.Once);
        publisher.Verify(service => service.PublishAsync(It.IsAny<string>(), It.IsAny<object>()), Times.Never);
        unitOfWork.Verify(work => work.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenPublisherFailure_LeavesMessagePendingAndRethrows()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        var integrationEvent = new UserActivateIntegrationEvent(Guid.NewGuid(), Topic, Guid.NewGuid());
        var message = CreateMessage(integrationEvent);
        var outbox = new Mock<IOutboxRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var publisher = new Mock<IOutboxEventPublisher>();
        var expectedException = new InvalidOperationException("Publisher failed");
        outbox.Setup(repository => repository.FindAsync(message.Id, token)).ReturnsAsync(message);
        publisher.Setup(service => service.PublishAsync(Topic, It.IsAny<object>()))
            .ThrowsAsync(expectedException);
        var handler = new PublishOutboxMessageHandler(outbox.Object, unitOfWork.Object, publisher.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => handler.HandleAsync(message.Id, token));

        Assert.Same(expectedException, exception);
        Assert.Null(message.PublishedAtUtc);
        publisher.Verify(service => service.PublishAsync<object>(Topic, integrationEvent), Times.Once);
        unitOfWork.Verify(work => work.SaveChangesAsync(), Times.Never);
    }
}
