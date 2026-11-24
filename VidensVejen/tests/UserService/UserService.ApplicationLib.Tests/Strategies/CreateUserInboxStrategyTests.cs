using System.Text.Json;
using UserService.ApplicationLib.Messaging.Inbox.Strategies;
using UserService.ApplicationLib.Repositories;
using UserService.DomainLib.Entities;
using Moq;
using VidensVejen.ContractsLib.Events;
using VidensVejen.SharedKernelLib.Exceptions;

namespace UserService.ApplicationLib.Tests.Strategies;

public class CreateUserInboxStrategyTests
{
    [Fact]
    public void EventType_GivenStrategy_MatchesHandledEvent()
    {
        // Arrange
        IInboxMessageStrategy strategy = new CreateUserInboxStrategy(new Mock<IUserRepository>().Object);

        // Act
        var eventType = strategy.EventType;

        // Assert
        Assert.Equal(typeof(UserRegisteredIntegrationEvent).FullName, eventType);
    }

    [Fact]
    public async Task Apply_GivenValidEvent_PerformsDomainOperation()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var token = TestContext.Current.CancellationToken;
        var repository = new Mock<IUserRepository>();
        var userId = Guid.NewGuid();
        User? captured = null;
        repository.Setup(r => r.AddAsync(It.IsAny<User>())).Callback<User>(user => captured = user);
        var integrationEvent = new UserRegisteredIntegrationEvent(eventId, "user-registered", userId, "Sue", "Storm", "sue@example.com");
        IInboxMessageStrategy strategy = new CreateUserInboxStrategy(repository.Object);

        // Act
        var result = await strategy.ApplyAsync(eventId, JsonSerializer.Serialize(integrationEvent), token);

        // Assert
        Assert.NotNull(captured);
        Assert.Equal(userId, captured.Id.Value);
        Assert.Equal("Sue", captured.Firstname);
        Assert.Equal("Storm", captured.Surname);
        Assert.Equal("sue@example.com", captured.EmailAddress.Value);
        repository.Verify(r => r.AddAsync(captured), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(eventId, result.Id);
        Assert.Equal("user-activate", result.Topic);
        Assert.Equal(typeof(UserActivateIntegrationEvent).FullName, result.Type);
        Assert.Equivalent(new UserActivateIntegrationEvent(eventId, "user-activate", userId),
            JsonSerializer.Deserialize<UserActivateIntegrationEvent>(result.Content));
        repository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Apply_GivenNullPayload_ThrowsBeforeRepositoryCalls()
    {
        // Arrange
        var repository = new Mock<IUserRepository>();
        IInboxMessageStrategy strategy = new CreateUserInboxStrategy(repository.Object);

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
        IInboxMessageStrategy strategy = new CreateUserInboxStrategy(repository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<JsonException>(() => strategy.ApplyAsync(
            Guid.NewGuid(), "{invalid", TestContext.Current.CancellationToken));
        repository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Apply_GivenInvalidUserData_ThrowsWithoutAddingUser()
    {
        // Arrange
        var repository = new Mock<IUserRepository>();
        var eventId = Guid.NewGuid();
        var integrationEvent = new UserRegisteredIntegrationEvent(
            eventId, "user-registered", Guid.NewGuid(), "", "Storm", "sue@example.com");
        IInboxMessageStrategy strategy = new CreateUserInboxStrategy(repository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => strategy.ApplyAsync(
            eventId, JsonSerializer.Serialize(integrationEvent), TestContext.Current.CancellationToken));
        repository.VerifyNoOtherCalls();
    }
}
