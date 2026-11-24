using System.Text.Json;
using AuthService.ApplicationLib.Messaging.Inbox.Strategies;
using AuthService.ApplicationLib.Repositories;
using AuthService.DomainLib.Entities;
using Moq;
using VidensVejen.ContractsLib.Events;
using VidensVejen.SharedKernelLib.Exceptions;

namespace AuthService.ApplicationLib.Tests.Users;

public class ActivateUserInboxStrategyTests
{
    [Fact]
    public void EventType_GivenStrategy_MatchesHandledEvent()
    {
        // Arrange
        IInboxMessageStrategy strategy = new ActivateUserInboxStrategy(new Mock<IAuthUserRepository>().Object);

        // Act
        var eventType = strategy.EventType;

        // Assert
        Assert.Equal(typeof(UserActivateIntegrationEvent).FullName, eventType);
    }

    [Fact]
    public async Task Apply_GivenValidEvent_PerformsDomainOperation()
    {
        // Arrange
        var eventId = Guid.NewGuid();
        var token = TestContext.Current.CancellationToken;
        var repository = new Mock<IAuthUserRepository>();
        var user = AuthUser.Create("sue@example.com", "hash", []);
        repository.Setup(r => r.GetAuthUserByIdAsync(user.Id)).ReturnsAsync(user);
        var integrationEvent = new UserActivateIntegrationEvent(eventId, "user-activate", user.Id.Value);
        IInboxMessageStrategy strategy = new ActivateUserInboxStrategy(repository.Object);

        // Act
        var result = await strategy.ApplyAsync(eventId, JsonSerializer.Serialize(integrationEvent), token);

        // Assert
        Assert.Equal(AuthService.DomainLib.Enums.AuthUserStatus.Active, user.Status);
        repository.Verify(r => r.GetAuthUserByIdAsync(user.Id), Times.Once);
        Assert.Null(result);
        repository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Apply_GivenNullPayload_ThrowsBeforeRepositoryCalls()
    {
        // Arrange
        var repository = new Mock<IAuthUserRepository>();
        IInboxMessageStrategy strategy = new ActivateUserInboxStrategy(repository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => strategy.ApplyAsync(
            Guid.NewGuid(), "null", TestContext.Current.CancellationToken));
        repository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Apply_GivenMalformedJson_ThrowsBeforeRepositoryCalls()
    {
        // Arrange
        var repository = new Mock<IAuthUserRepository>();
        IInboxMessageStrategy strategy = new ActivateUserInboxStrategy(repository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<JsonException>(() => strategy.ApplyAsync(
            Guid.NewGuid(), "{invalid", TestContext.Current.CancellationToken));
        repository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Apply_GivenUnknownUser_ThrowsNotFoundException()
    {
        // Arrange
        var repository = new Mock<IAuthUserRepository>();
        var eventId = Guid.NewGuid();
        var integrationEvent = new UserActivateIntegrationEvent(eventId, "user-activate", Guid.NewGuid());
        IInboxMessageStrategy strategy = new ActivateUserInboxStrategy(repository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => strategy.ApplyAsync(
            eventId, JsonSerializer.Serialize(integrationEvent), TestContext.Current.CancellationToken));
        repository.Verify(r => r.GetAuthUserByIdAsync(It.IsAny<AuthUserId>()), Times.Once);
        repository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Apply_GivenAlreadyActiveUser_ThrowsDomainException()
    {
        // Arrange
        var user = AuthUser.Create("sue@example.com", "hash", []);
        user.SetStatusActive();
        var repository = new Mock<IAuthUserRepository>();
        repository.Setup(r => r.GetAuthUserByIdAsync(user.Id)).ReturnsAsync(user);
        var eventId = Guid.NewGuid();
        var integrationEvent = new UserActivateIntegrationEvent(eventId, "user-activate", user.Id.Value);
        IInboxMessageStrategy strategy = new ActivateUserInboxStrategy(repository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => strategy.ApplyAsync(
            eventId, JsonSerializer.Serialize(integrationEvent), TestContext.Current.CancellationToken));
        repository.Verify(r => r.GetAuthUserByIdAsync(user.Id), Times.Once);
        repository.VerifyNoOtherCalls();
    }
}
