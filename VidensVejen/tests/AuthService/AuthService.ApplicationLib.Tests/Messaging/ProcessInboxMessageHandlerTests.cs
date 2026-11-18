using System.Data;
using System.Text.Json;
using AuthService.ApplicationLib.Messaging.Inbox.Handlers;
using AuthService.ApplicationLib.Messaging.Inbox.Strategies;
using AuthService.ApplicationLib.Repositories;
using AuthService.DomainLib.Entities;
using AuthService.DomainLib.Enums;
using Moq;
using VidensVejen.BuildingBlocksLib.Messaging.Inbox;
using VidensVejen.BuildingBlocksLib.Messaging.Outbox;
using VidensVejen.BuildingBlocksLib.UnitOfWork;
using VidensVejen.ContractsLib.Events;
using VidensVejen.SharedKernelLib.Exceptions;

namespace AuthService.ApplicationLib.Tests.Messaging;

public class ProcessInboxMessageHandlerTests
{
    private static InboxMessage CreateMessage(Guid userId)
    {
        var integrationEvent = new UserActivateIntegrationEvent(Guid.NewGuid(), "user-activate", userId);
        return new InboxMessage
        {
            Id = integrationEvent.EventId,
            Type = typeof(UserActivateIntegrationEvent).FullName!,
            Topic = "user-activate",
            Content = JsonSerializer.Serialize(integrationEvent),
            RecievedOnUtc = DateTime.UtcNow
        };
    }

    [Fact]
    public async Task Handle_GivenPendingActivation_ActivatesUserAndCommits()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        var user = AuthUser.Create("sue@example.com", "password-hash", []);
        var userId = user.Id;
        var message = CreateMessage(userId.Value);
        var inbox = new Mock<IInboxRepository>();
        var userRepo = new Mock<IAuthUserRepository>();
        var outbox = new Mock<IOutboxRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        inbox.Setup(repository => repository.FindAsync(message.Id, token)).ReturnsAsync(message);
        userRepo.Setup(repository => repository.GetAuthUserByIdAsync(userId))
            .ReturnsAsync(user);
        var handler = new ProcessInboxMessageHandler(
            inbox.Object,
            outbox.Object,
            unitOfWork.Object,
            [new ActivateUserInboxStrategy(userRepo.Object)]);

        // Act
        await handler.HandleAsync(message.Id, token);

        // Assert
        Assert.Equal(AuthUserStatus.Active, user.Status);
        Assert.NotNull(message.ProccessedOnUtc);
        userRepo.Verify(repository => repository.GetAuthUserByIdAsync(userId), Times.Once);
        outbox.Verify(repository => repository.AddAsync(It.IsAny<OutboxMessage>()), Times.Never);
        unitOfWork.Verify(work => work.BeginTransaction(IsolationLevel.Serializable), Times.Once);
        unitOfWork.Verify(work => work.SaveChangesAsync(), Times.Once);
        unitOfWork.Verify(work => work.Commit(), Times.Once);
        unitOfWork.Verify(work => work.Rollback(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenUnknownUser_RollsBackAndThrows()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        var userId = new AuthUserId(Guid.NewGuid());
        var message = CreateMessage(userId.Value);
        var inbox = new Mock<IInboxRepository>();
        var userRepo = new Mock<IAuthUserRepository>();
        var outbox = new Mock<IOutboxRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        inbox.Setup(repository => repository.FindAsync(message.Id, token)).ReturnsAsync(message);
        userRepo.Setup(repository => repository.GetAuthUserByIdAsync(userId))
            .ReturnsAsync((AuthUser?)null);
        var handler = new ProcessInboxMessageHandler(
            inbox.Object,
            outbox.Object,
            unitOfWork.Object,
            [new ActivateUserInboxStrategy(userRepo.Object)]);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(message.Id, token));

        Assert.Null(message.ProccessedOnUtc);
        userRepo.Verify(repository => repository.GetAuthUserByIdAsync(userId), Times.Once);
        outbox.Verify(repository => repository.AddAsync(It.IsAny<OutboxMessage>()), Times.Never);
        unitOfWork.Verify(work => work.BeginTransaction(IsolationLevel.Serializable), Times.Once);
        unitOfWork.Verify(work => work.SaveChangesAsync(), Times.Never);
        unitOfWork.Verify(work => work.Commit(), Times.Never);
        unitOfWork.Verify(work => work.Rollback(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenUnregisteredEventType_RollsBackAndThrows()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        var message = new InboxMessage
        {
            Id = Guid.NewGuid(),
            Type = "UnknownEvent",
            Topic = "unknown-event",
            Content = "{}",
            RecievedOnUtc = DateTime.UtcNow
        };
        var inbox = new Mock<IInboxRepository>();
        var outbox = new Mock<IOutboxRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var userRepo = new Mock<IAuthUserRepository>();
        inbox.Setup(repository => repository.FindAsync(message.Id, token)).ReturnsAsync(message);
        var handler = new ProcessInboxMessageHandler(
            inbox.Object,
            outbox.Object,
            unitOfWork.Object,
            [new ActivateUserInboxStrategy(userRepo.Object)]);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => handler.HandleAsync(message.Id, token));

        Assert.Contains(message.Type, exception.Message);
        Assert.Null(message.ProccessedOnUtc);
        userRepo.VerifyNoOtherCalls();
        outbox.Verify(repository => repository.AddAsync(It.IsAny<OutboxMessage>()), Times.Never);
        unitOfWork.Verify(work => work.SaveChangesAsync(), Times.Never);
        unitOfWork.Verify(work => work.Commit(), Times.Never);
        unitOfWork.Verify(work => work.Rollback(), Times.Once);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Handle_GivenMissingOrProcessedMessage_SkipsActivation(bool alreadyProcessed)
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        var eventId = Guid.NewGuid();
        InboxMessage? message = null;
        if (alreadyProcessed)
        {
            message = CreateMessage(Guid.NewGuid());
            eventId = message.Id;
            message.SetProccesedAtUtc();
        }

        var inbox = new Mock<IInboxRepository>();
        var userRepo = new Mock<IAuthUserRepository>();
        var outbox = new Mock<IOutboxRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        inbox.Setup(repository => repository.FindAsync(eventId, token)).ReturnsAsync(message);
        var handler = new ProcessInboxMessageHandler(
            inbox.Object, outbox.Object, unitOfWork.Object,
            [new ActivateUserInboxStrategy(userRepo.Object)]);

        // Act
        await handler.HandleAsync(eventId, token);

        // Assert
        inbox.Verify(repository => repository.FindAsync(eventId, token), Times.Once);
        userRepo.VerifyNoOtherCalls();
        outbox.VerifyNoOtherCalls();
        unitOfWork.Verify(work => work.SaveChangesAsync(), Times.Never);
        unitOfWork.Verify(work => work.Commit(), Times.Once);
        unitOfWork.Verify(work => work.Rollback(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenSaveFailure_RollsBackAndRethrows()
    {
        // Arrange
        var token = TestContext.Current.CancellationToken;
        var user = AuthUser.Create("sue@example.com", "password-hash", []);
        var message = CreateMessage(user.Id.Value);
        var inbox = new Mock<IInboxRepository>();
        var userRepo = new Mock<IAuthUserRepository>();
        var outbox = new Mock<IOutboxRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var expectedException = new InvalidOperationException("Save failed");
        inbox.Setup(repository => repository.FindAsync(message.Id, token)).ReturnsAsync(message);
        userRepo.Setup(repository => repository.GetAuthUserByIdAsync(user.Id)).ReturnsAsync(user);
        unitOfWork.Setup(work => work.SaveChangesAsync()).ThrowsAsync(expectedException);
        var handler = new ProcessInboxMessageHandler(
            inbox.Object, outbox.Object, unitOfWork.Object,
            [new ActivateUserInboxStrategy(userRepo.Object)]);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => handler.HandleAsync(message.Id, token));

        Assert.Same(expectedException, exception);
        unitOfWork.Verify(work => work.SaveChangesAsync(), Times.Once);
        unitOfWork.Verify(work => work.Commit(), Times.Never);
        unitOfWork.Verify(work => work.Rollback(), Times.Once);
    }
}
