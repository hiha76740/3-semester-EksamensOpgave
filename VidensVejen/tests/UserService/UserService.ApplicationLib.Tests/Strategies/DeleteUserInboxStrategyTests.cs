using System.Text.Json;
using UserService.ApplicationLib.Messaging.Inbox.Strategies;
using UserService.ApplicationLib.Repositories;
using UserService.DomainLib.Entities;
using Moq;
using VidensVejen.ContractsLib.Events;
using VidensVejen.SharedKernelLib.Exceptions;

namespace UserService.ApplicationLib.Tests.Strategies;

public class DeleteUserInboxStrategyTests
{
    [Fact]
    public void EventType_GivenStrategy_MatchesHandledEvent()
    {
        // Arrange
        IInboxMessageStrategy strategy = new DeleteUserInboxStrategy(new Mock<IUserRepository>().Object);

        // Act
        var eventType = strategy.EventType;

        // Assert
        Assert.Equal(typeof(UserDeletionIntegrationEvent).FullName, eventType);
    }

    [Fact]
    public async Task Apply_GivenValidEvent_PerformsDomainOperation()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var token = TestContext.Current.CancellationToken;
        var repository = new Mock<IUserRepository>();
        var user = User.Create(new UserId(Guid.NewGuid()), "Sue", "Storm", "old@example.com");
        repository.Setup(r => r.GetUserByIdAsync(user.Id)).ReturnsAsync(user);
        var integrationEvent = new UserDeletionIntegrationEvent(eventId, "user-deletion", user.Id.Value);
        IInboxMessageStrategy strategy = new DeleteUserInboxStrategy(repository.Object);

        // Act
        var result = await strategy.ApplyAsync(eventId, JsonSerializer.Serialize(integrationEvent), token);

        // Assert
        repository.Verify(r => r.Delete(user), Times.Once);
        repository.Verify(r => r.GetUserByIdAsync(user.Id), Times.Once);
        Assert.Null(result);
        repository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Apply_GivenNullPayload_ThrowsBeforeRepositoryCalls()
    {
        // Arrange
        var repository = new Mock<IUserRepository>();
        IInboxMessageStrategy strategy = new DeleteUserInboxStrategy(repository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => strategy.ApplyAsync(
            Guid.NewGuid(), "null", TestContext.Current.CancellationToken));
        repository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Apply_GivenMalformedJson_ThrowsBeforeRepositoryCalls()
    {
        // Arrange
        var repository = new Mock<IUserRepository>();
        IInboxMessageStrategy strategy = new DeleteUserInboxStrategy(repository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<JsonException>(() => strategy.ApplyAsync(
            Guid.NewGuid(), "{invalid", TestContext.Current.CancellationToken));
        repository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Apply_GivenUnknownUser_ThrowsNotFoundException()
    {
        // Arrange
        var repository = new Mock<IUserRepository>();
        var eventId = Guid.NewGuid();
        var integrationEvent = new UserDeletionIntegrationEvent(eventId, "user-deletion", Guid.NewGuid());
        IInboxMessageStrategy strategy = new DeleteUserInboxStrategy(repository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => strategy.ApplyAsync(
            eventId, JsonSerializer.Serialize(integrationEvent), TestContext.Current.CancellationToken));
        repository.Verify(r => r.GetUserByIdAsync(It.IsAny<UserId>()), Times.Once);
        repository.VerifyNoOtherCalls();
    }
}
