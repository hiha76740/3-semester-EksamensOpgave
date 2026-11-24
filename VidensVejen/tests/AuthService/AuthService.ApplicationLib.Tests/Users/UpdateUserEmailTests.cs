using AuthService.ApplicationLib.Handlers.Users;
using AuthService.ApplicationLib.Repositories;
using AuthService.DomainLib.Entities;
using AuthService.FacadeLib.Commands.DTOs;
using AuthService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using VidensVejen.SharedKernelLib.Exceptions;
using System.Text.Json;
using AuthService.DomainLib.Enums;
using VidensVejen.BuildingBlocksLib.Messaging.Outbox;
using VidensVejen.BuildingBlocksLib.UnitOfWork;
using VidensVejen.ContractsLib.Events;

namespace AuthService.ApplicationLib.Tests.Users;

public class UpdateUserEmailTests
{

    [Fact]
    public async Task Handle_GivenActiveUser_ChangesEmailAndQueuesActivationFlow()
    {
        // Arrange
        var user = AuthUser.Create("old@example.com", "hash", []);
        user.SetStatusActive();
        byte[] version = [1, 2, 3];
        var repository = new Mock<IAuthUserRepository>();
        var outbox = new Mock<IOutboxRepository>();
        var work = new Mock<IUnitOfWork>();
        repository.Setup(r => r.GetAuthUserByIdAsync(user.Id)).ReturnsAsync(user);
        OutboxMessage? queued = null;
        outbox.Setup(r => r.AddAsync(It.IsAny<OutboxMessage>())).Callback<OutboxMessage>(message => queued = message);
        IUpdateUserEmailHandler handler = new UpdateUserEmailHandler(
            repository.Object, outbox.Object, work.Object, new Mock<ILogger<UpdateUserEmailHandler>>().Object);
        var command = new UpdateAuthUserEmailCommand(user.Id.Value, "new@example.com", version);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.Equal("new@example.com", user.Email.Value);
        Assert.Equal("new@example.com", result.Email);
        Assert.Equal(AuthUserStatus.Pending, user.Status);
        Assert.NotNull(queued);
        Assert.Equal("user-update-email", queued.Topic);
        Assert.Equal(typeof(UserUpdateEmailIntegrationEvent).FullName, queued.Type);
        Assert.Equivalent(new UserUpdateEmailIntegrationEvent(queued.Id, queued.Topic, user.Id.Value, command.Email),
            JsonSerializer.Deserialize<UserUpdateEmailIntegrationEvent>(queued.Content));
        repository.Verify(r => r.SetOriginalRowVersion(user, version), Times.Once);
        work.Verify(w => w.SaveChangesAsync(), Times.Once);
        work.Verify(w => w.Commit(), Times.Once);
        work.Verify(w => w.Rollback(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenUnknownUser_RollsBack()
    {
        // Arrange
        var repository = new Mock<IAuthUserRepository>();
        var outbox = new Mock<IOutboxRepository>();
        var work = new Mock<IUnitOfWork>();
        IUpdateUserEmailHandler handler = new UpdateUserEmailHandler(
            repository.Object, outbox.Object, work.Object, new Mock<ILogger<UpdateUserEmailHandler>>().Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.HandleAsync(new UpdateAuthUserEmailCommand(Guid.NewGuid(), "new@example.com", [])));
        outbox.VerifyNoOtherCalls();
        work.Verify(w => w.SaveChangesAsync(), Times.Never);
        work.Verify(w => w.Commit(), Times.Never);
        work.Verify(w => w.Rollback(), Times.Once);
    }
}
