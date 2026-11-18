using VidensVejen.SharedKernelLib.Exceptions;
using System.Data;
using System.Text.Json;
using Moq;
using UserService.ApplicationLib.Messaging.Inbox.Handlers;
using UserService.ApplicationLib.Messaging.Inbox.Strategies;
using UserService.ApplicationLib.Repositories;
using UserService.DomainLib.Entities;
using UserService.DomainLib.ValueObjects;
using VidensVejen.BuildingBlocksLib.Messaging.Inbox;
using VidensVejen.BuildingBlocksLib.Messaging.Outbox;
using VidensVejen.BuildingBlocksLib.UnitOfWork;
using VidensVejen.ContractsLib.Events;

namespace UserService.ApplicationLib.Tests;

public class ProcessInboxMessageHandlerTests
{
    private static InboxMessage CreateMessage(UserRegisteredIntegrationEvent integrationEvent)
    {
        return CreateMessage(integrationEvent.EventId, JsonSerializer.Serialize(integrationEvent));
    }

    private static InboxMessage CreateMessage(Guid eventId, string content)
    {
        return new InboxMessage
        {
            Id = eventId,
            Type = typeof(UserRegisteredIntegrationEvent).FullName!,
            Topic = "user-registered",
            Content = content,
            RecievedOnUtc = DateTime.UtcNow
        };
    }

    [Fact]
    public async Task Handle_GivenPendingMessage_CreatesUserAndQueuesActivation()
    {
        // Arrange
        var mockInbox = new Mock<IInboxRepository>();
        var mockUserRepo = new Mock<IUserRepository>();
        var mockOutbox = new Mock<IOutboxRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var eventId = Guid.NewGuid();
        var cancellationToken = TestContext.Current.CancellationToken;
        var userId = Guid.NewGuid();
        var integrationEvent = new UserRegisteredIntegrationEvent(
            eventId, "user-registered", userId, "Sue", "Storm", "sue@example.com");
        var message = CreateMessage(integrationEvent);
        User? savedUser = null;
        OutboxMessage? queuedMessage = null;

        mockUserRepo.Setup(r => r.AddAsync(It.IsAny<User>()))
            .Callback<User>(user => savedUser = user);
        mockOutbox.Setup(r => r.AddAsync(It.IsAny<OutboxMessage>()))
            .Callback<OutboxMessage>(outboxMessage => queuedMessage = outboxMessage);

        mockInbox.Setup(r => r.FindAsync(eventId, cancellationToken))
            .ReturnsAsync(message);
        var handler = new ProcessInboxMessageHandler(
            mockInbox.Object, mockOutbox.Object, mockUnitOfWork.Object,
            [new CreateUserInboxStrategy(mockUserRepo.Object)]);

        // Act
        await handler.HandleAsync(eventId, cancellationToken);

        // Assert

        mockInbox.Verify(r => r.FindAsync(eventId, cancellationToken), Times.Once);
        mockUnitOfWork.Verify(u => u.BeginTransaction(IsolationLevel.Serializable), Times.Once);
        mockUserRepo.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
        mockOutbox.Verify(r => r.AddAsync(It.IsAny<OutboxMessage>()), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        mockUnitOfWork.Verify(u => u.Commit(), Times.Once);
        mockUnitOfWork.Verify(u => u.Rollback(), Times.Never);

        Assert.NotNull(savedUser);
        Assert.Equal(userId, savedUser.Id.Value);
        Assert.Equal("Sue", savedUser.Firstname);
        Assert.Equal("Storm", savedUser.Surname);
        Assert.Equal(new Email("sue@example.com"), savedUser.EmailAddress);
        Assert.NotNull(message.ProccessedOnUtc);

        Assert.NotNull(queuedMessage);
        Assert.Equal(eventId, queuedMessage.Id);
        Assert.Equal("user-activate", queuedMessage.Topic);
        Assert.Equal(typeof(UserActivateIntegrationEvent).FullName, queuedMessage.Type);
        var actualEvent = JsonSerializer.Deserialize<UserActivateIntegrationEvent>(queuedMessage.Content);
        Assert.Equivalent(new UserActivateIntegrationEvent(eventId, "user-activate", userId), actualEvent);
    }

    [Fact]
    public async Task Handle_GivenMissingMessage_CommitsWithoutWrites()
    {
        // Arrange
        var mockInbox = new Mock<IInboxRepository>();
        var mockUserRepo = new Mock<IUserRepository>();
        var mockOutbox = new Mock<IOutboxRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var eventId = Guid.NewGuid();
        var cancellationToken = TestContext.Current.CancellationToken;

        mockInbox.Setup(r => r.FindAsync(eventId, cancellationToken))
            .ReturnsAsync((InboxMessage?)null);
        var handler = new ProcessInboxMessageHandler(
            mockInbox.Object, mockOutbox.Object, mockUnitOfWork.Object,
            [new CreateUserInboxStrategy(mockUserRepo.Object)]);

        // Act
        await handler.HandleAsync(eventId, cancellationToken);

        // Assert

        mockInbox.Verify(r => r.FindAsync(eventId, cancellationToken), Times.Once);
        mockUnitOfWork.Verify(u => u.BeginTransaction(IsolationLevel.Serializable), Times.Once);
        mockUserRepo.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        mockOutbox.Verify(r => r.AddAsync(It.IsAny<OutboxMessage>()), Times.Never);
        mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        mockUnitOfWork.Verify(u => u.Commit(), Times.Once);
        mockUnitOfWork.Verify(u => u.Rollback(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenProcessedMessage_CommitsWithoutWrites()
    {
        // Arrange
        var mockInbox = new Mock<IInboxRepository>();
        var mockUserRepo = new Mock<IUserRepository>();
        var mockOutbox = new Mock<IOutboxRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var eventId = Guid.NewGuid();
        var cancellationToken = TestContext.Current.CancellationToken;
        var userId = Guid.NewGuid();
        var integrationEvent = new UserRegisteredIntegrationEvent(
            eventId, "user-registered", userId, "Sue", "Storm", "sue@example.com");
        var message = CreateMessage(integrationEvent);
        message.SetProccesedAtUtc();

        mockInbox.Setup(r => r.FindAsync(eventId, cancellationToken))
            .ReturnsAsync(message);
        var handler = new ProcessInboxMessageHandler(
            mockInbox.Object, mockOutbox.Object, mockUnitOfWork.Object,
            [new CreateUserInboxStrategy(mockUserRepo.Object)]);

        // Act
        await handler.HandleAsync(eventId, cancellationToken);

        // Assert

        mockInbox.Verify(r => r.FindAsync(eventId, cancellationToken), Times.Once);
        mockUnitOfWork.Verify(u => u.BeginTransaction(IsolationLevel.Serializable), Times.Once);
        mockUserRepo.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        mockOutbox.Verify(r => r.AddAsync(It.IsAny<OutboxMessage>()), Times.Never);
        mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        mockUnitOfWork.Verify(u => u.Commit(), Times.Once);
        mockUnitOfWork.Verify(u => u.Rollback(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenNullPayload_RollsBackAndThrows()
    {
        // Arrange
        var mockInbox = new Mock<IInboxRepository>();
        var mockUserRepo = new Mock<IUserRepository>();
        var mockOutbox = new Mock<IOutboxRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var eventId = Guid.NewGuid();
        var cancellationToken = TestContext.Current.CancellationToken;
        var message = CreateMessage(eventId, "null");

        mockInbox.Setup(r => r.FindAsync(eventId, cancellationToken))
            .ReturnsAsync(message);
        var handler = new ProcessInboxMessageHandler(
            mockInbox.Object, mockOutbox.Object, mockUnitOfWork.Object,
            [new CreateUserInboxStrategy(mockUserRepo.Object)]);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => handler.HandleAsync(eventId, cancellationToken));
        Assert.Contains(eventId.ToString(), exception.Message);

        mockInbox.Verify(r => r.FindAsync(eventId, cancellationToken), Times.Once);
        mockUnitOfWork.Verify(u => u.BeginTransaction(IsolationLevel.Serializable), Times.Once);
        mockUserRepo.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        mockOutbox.Verify(r => r.AddAsync(It.IsAny<OutboxMessage>()), Times.Never);
        mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        mockUnitOfWork.Verify(u => u.Commit(), Times.Never);
        mockUnitOfWork.Verify(u => u.Rollback(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenSaveFailure_RollsBackAndRethrows()
    {
        // Arrange
        var mockInbox = new Mock<IInboxRepository>();
        var mockUserRepo = new Mock<IUserRepository>();
        var mockOutbox = new Mock<IOutboxRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        var eventId = Guid.NewGuid();
        var cancellationToken = TestContext.Current.CancellationToken;
        var userId = Guid.NewGuid();
        var integrationEvent = new UserRegisteredIntegrationEvent(
            eventId, "user-registered", userId, "Sue", "Storm", "sue@example.com");
        var message = CreateMessage(integrationEvent);
        var expectedException = new InvalidOperationException("Save failed");
        mockUnitOfWork.Setup(u => u.SaveChangesAsync()).ThrowsAsync(expectedException);

        mockInbox.Setup(r => r.FindAsync(eventId, cancellationToken))
            .ReturnsAsync(message);
        var handler = new ProcessInboxMessageHandler(
            mockInbox.Object, mockOutbox.Object, mockUnitOfWork.Object,
            [new CreateUserInboxStrategy(mockUserRepo.Object)]);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => handler.HandleAsync(eventId, cancellationToken));
        Assert.Same(expectedException, exception);

        mockInbox.Verify(r => r.FindAsync(eventId, cancellationToken), Times.Once);
        mockUnitOfWork.Verify(u => u.BeginTransaction(IsolationLevel.Serializable), Times.Once);
        mockUserRepo.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
        mockOutbox.Verify(r => r.AddAsync(It.IsAny<OutboxMessage>()), Times.Once);
        mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        mockUnitOfWork.Verify(u => u.Commit(), Times.Never);
        mockUnitOfWork.Verify(u => u.Rollback(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenDeletionMessage_DeletesUserWithoutQueuingEvent()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        var user = User.Create(new UserId(Guid.NewGuid()), "Sue", "Storm", "sue@example.com");
        var integrationEvent = new UserDeletionIntegrationEvent(Guid.NewGuid(), "user-deletion", user.Id.Value);
        var message = new InboxMessage
        {
            Id = integrationEvent.EventId,
            Type = typeof(UserDeletionIntegrationEvent).FullName!,
            Topic = integrationEvent.EventTopic,
            Content = JsonSerializer.Serialize(integrationEvent),
            RecievedOnUtc = DateTime.UtcNow
        };
        var inbox = new Mock<IInboxRepository>();
        var userRepo = new Mock<IUserRepository>();
        var outbox = new Mock<IOutboxRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        inbox.Setup(r => r.FindAsync(message.Id, token)).ReturnsAsync(message);
        userRepo.Setup(r => r.GetUserByIdAsync(user.Id)).ReturnsAsync(user);
        var handler = new ProcessInboxMessageHandler(
            inbox.Object, outbox.Object, unitOfWork.Object,
            [new CreateUserInboxStrategy(userRepo.Object), new DeleteUserInboxStrategy(userRepo.Object)]);

        // Act
        await handler.HandleAsync(message.Id, token);

        // Assert
        userRepo.Verify(r => r.Delete(user), Times.Once);
        userRepo.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        outbox.Verify(r => r.AddAsync(It.IsAny<OutboxMessage>()), Times.Never);
        Assert.NotNull(message.ProccessedOnUtc);
        unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        unitOfWork.Verify(u => u.Commit(), Times.Once);
        unitOfWork.Verify(u => u.Rollback(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenUnknownDeletionUser_RollsBackAndThrows()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        var integrationEvent = new UserDeletionIntegrationEvent(
            Guid.NewGuid(), "user-deletion", Guid.NewGuid());
        var message = new InboxMessage
        {
            Id = integrationEvent.EventId,
            Type = typeof(UserDeletionIntegrationEvent).FullName!,
            Topic = integrationEvent.EventTopic,
            Content = JsonSerializer.Serialize(integrationEvent),
            RecievedOnUtc = DateTime.UtcNow
        };
        var inbox = new Mock<IInboxRepository>();
        var userRepo = new Mock<IUserRepository>();
        var outbox = new Mock<IOutboxRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        inbox.Setup(repository => repository.FindAsync(message.Id, token)).ReturnsAsync(message);
        userRepo.Setup(repository => repository.GetUserByIdAsync(new UserId(integrationEvent.UserId)))
            .ReturnsAsync((User?)null);
        var handler = new ProcessInboxMessageHandler(
            inbox.Object, outbox.Object, unitOfWork.Object,
            [new DeleteUserInboxStrategy(userRepo.Object)]);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(message.Id, token));

        Assert.Null(message.ProccessedOnUtc);
        userRepo.Verify(repository => repository.Delete(It.IsAny<User>()), Times.Never);
        outbox.VerifyNoOtherCalls();
        unitOfWork.Verify(work => work.SaveChangesAsync(), Times.Never);
        unitOfWork.Verify(work => work.Commit(), Times.Never);
        unitOfWork.Verify(work => work.Rollback(), Times.Once);
    }
}
