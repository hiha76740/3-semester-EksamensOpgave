using System.Text.Json;
using UserService.ApplicationLib.Messaging.Inbox.Strategies;
using UserService.ApplicationLib.Repositories;
using UserService.DomainLib.Entities;
using Moq;
using VidensVejen.ContractsLib.Events;
using VidensVejen.SharedKernelLib.Exceptions;

namespace UserService.ApplicationLib.Tests.Strategies;

public class UpdateUserEmailInboxStrategyTests
{
    [Fact]
    public void EventType_GivenStrategy_MatchesHandledEvent()
    {
        // Arrange
        IInboxMessageStrategy strategy = new UpdateUserEmailInboxStrategy(new Mock<IUserRepository>().Object);

        // Act
        var eventType = strategy.EventType;

        // Assert
        Assert.Equal(typeof(UserUpdateEmailIntegrationEvent).FullName, eventType);
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
        var integrationEvent = new UserUpdateEmailIntegrationEvent(eventId, "user-update-email", user.Id.Value, "new@example.com");
        IInboxMessageStrategy strategy = new UpdateUserEmailInboxStrategy(repository.Object);

        // Act
        var result = await strategy.ApplyAsync(eventId, JsonSerializer.Serialize(integrationEvent), token);

        // Assert
        Assert.Equal("new@example.com", user.EmailAddress.Value);
        repository.Verify(r => r.GetUserByIdAsync(user.Id), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(eventId, result.Id);
        Assert.Equal("user-activate", result.Topic);
        Assert.Equal(typeof(UserActivateIntegrationEvent).FullName, result.Type);
        Assert.Equivalent(new UserActivateIntegrationEvent(eventId, "user-activate", user.Id.Value),
            JsonSerializer.Deserialize<UserActivateIntegrationEvent>(result.Content));
        repository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Apply_GivenNullPayload_ThrowsBeforeRepositoryCalls()
    {
        // Arrange
        var repository = new Mock<IUserRepository>();
        IInboxMessageStrategy strategy = new UpdateUserEmailInboxStrategy(repository.Object);

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
        IInboxMessageStrategy strategy = new UpdateUserEmailInboxStrategy(repository.Object);

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
        var integrationEvent = new UserUpdateEmailIntegrationEvent(eventId, "user-update-email", Guid.NewGuid(), "new@example.com");
        IInboxMessageStrategy strategy = new UpdateUserEmailInboxStrategy(repository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => strategy.ApplyAsync(
            eventId, JsonSerializer.Serialize(integrationEvent), TestContext.Current.CancellationToken));
        repository.Verify(r => r.GetUserByIdAsync(It.IsAny<UserId>()), Times.Once);
        repository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Apply_GivenUnchangedEmail_ThrowsDomainException()
    {
        // Arrange
        var user = User.Create(new UserId(Guid.NewGuid()), "Sue", "Storm", "sue@example.com");
        var repository = new Mock<IUserRepository>();
        repository.Setup(r => r.GetUserByIdAsync(user.Id)).ReturnsAsync(user);
        var eventId = Guid.NewGuid();
        var integrationEvent = new UserUpdateEmailIntegrationEvent(eventId, "user-update-email", user.Id.Value, "sue@example.com");
        IInboxMessageStrategy strategy = new UpdateUserEmailInboxStrategy(repository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => strategy.ApplyAsync(
            eventId, JsonSerializer.Serialize(integrationEvent), TestContext.Current.CancellationToken));
        repository.Verify(r => r.GetUserByIdAsync(user.Id), Times.Once);
        repository.VerifyNoOtherCalls();
    }
}
