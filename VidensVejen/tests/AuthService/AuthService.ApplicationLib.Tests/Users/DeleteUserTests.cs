using System.Data;
using System.Text.Json;
using AuthService.ApplicationLib.Handlers.Users;
using AuthService.ApplicationLib.Repositories;
using AuthService.DomainLib.Entities;
using AuthService.FacadeLib.Commands.DTOs;
using AuthService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using VidensVejen.BuildingBlocksLib.Messaging.Outbox;
using VidensVejen.BuildingBlocksLib.UnitOfWork;
using VidensVejen.ContractsLib.Events;
using VidensVejen.SharedKernelLib.Exceptions;

namespace AuthService.ApplicationLib.Tests.Users;

public class DeleteUserTests
{

    [Fact]
    public async Task Handle_GivenValid_DeletesUserAndQueuesEvent()
    {
        // Arrange
        var user = AuthUser.Create("sue@example.com", "password-hash", []);
        var command = new DeleteAuthUserCommand(user.Id.Value, [1, 2, 3]);
        var repository = new Mock<IAuthUserRepository>();
        var outbox = new Mock<IOutboxRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var logger = new Mock<ILogger<DeleteUserHandler>>();
        repository.Setup(r => r.GetAuthUserByIdAsync(user.Id)).ReturnsAsync(user);
        OutboxMessage? queuedMessage = null;
        outbox.Setup(r => r.AddAsync(It.IsAny<OutboxMessage>()))
            .Callback<OutboxMessage>(message => queuedMessage = message)
            .Returns(Task.CompletedTask);
        IDeleteUserHandler handler = new DeleteUserHandler(
            repository.Object, outbox.Object, unitOfWork.Object, logger.Object);

        // Act
        await handler.HandleAsync(command);

        // Assert
        Assert.NotNull(queuedMessage);
        Assert.NotEqual(Guid.Empty, queuedMessage.Id);
        Assert.Equal("user-deletion", queuedMessage.Topic);
        Assert.Equal(typeof(UserDeletionIntegrationEvent).FullName, queuedMessage.Type);
        var actualEvent = JsonSerializer.Deserialize<UserDeletionIntegrationEvent>(queuedMessage.Content);
        Assert.Equivalent(
            new UserDeletionIntegrationEvent(queuedMessage.Id, "user-deletion", user.Id.Value),
            actualEvent);

        repository.Verify(r => r.GetAuthUserByIdAsync(user.Id), Times.Once);
        repository.Verify(r => r.Delete(user), Times.Once);
        repository.Verify(r => r.SetOriginalRowVersion(user, command.RowVersion), Times.Once);
        outbox.Verify(r => r.AddAsync(It.IsAny<OutboxMessage>()), Times.Once);
        unitOfWork.Verify(u => u.BeginTransaction(IsolationLevel.Serializable), Times.Once);
        unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        unitOfWork.Verify(u => u.Commit(), Times.Once);
        unitOfWork.Verify(u => u.Rollback(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenMissing_RollsBackAndThrows()
    {
        // Arrange
        var user = AuthUser.Create("sue@example.com", "password-hash", []);
        var command = new DeleteAuthUserCommand(user.Id.Value, [1, 2, 3]);
        var repository = new Mock<IAuthUserRepository>();
        var outbox = new Mock<IOutboxRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var logger = new Mock<ILogger<DeleteUserHandler>>();
        repository.Setup(r => r.GetAuthUserByIdAsync(user.Id)).ReturnsAsync((AuthUser?)null);
        IDeleteUserHandler handler = new DeleteUserHandler(
            repository.Object, outbox.Object, unitOfWork.Object, logger.Object);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.HandleAsync(command));

        repository.Verify(r => r.GetAuthUserByIdAsync(user.Id), Times.Once);
        repository.Verify(r => r.Delete(It.IsAny<AuthUser>()), Times.Never);
        repository.Verify(r => r.SetOriginalRowVersion(It.IsAny<AuthUser>(), It.IsAny<byte[]>()), Times.Never);
        outbox.Verify(r => r.AddAsync(It.IsAny<OutboxMessage>()), Times.Never);
        unitOfWork.Verify(u => u.BeginTransaction(IsolationLevel.Serializable), Times.Once);
        unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        unitOfWork.Verify(u => u.Commit(), Times.Never);
        unitOfWork.Verify(u => u.Rollback(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenOutboxFailure_RollsBackAndThrows()
    {
        // Arrange
        var user = AuthUser.Create("sue@example.com", "password-hash", []);
        var command = new DeleteAuthUserCommand(user.Id.Value, [1, 2, 3]);
        var repository = new Mock<IAuthUserRepository>();
        var outbox = new Mock<IOutboxRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var logger = new Mock<ILogger<DeleteUserHandler>>();
        repository.Setup(r => r.GetAuthUserByIdAsync(user.Id)).ReturnsAsync(user);
        var expectedException = new InvalidOperationException("Write failed");
        outbox.Setup(r => r.AddAsync(It.IsAny<OutboxMessage>())).ThrowsAsync(expectedException);
        IDeleteUserHandler handler = new DeleteUserHandler(
            repository.Object, outbox.Object, unitOfWork.Object, logger.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => handler.HandleAsync(command));
        Assert.Same(expectedException, exception);

        repository.Verify(r => r.GetAuthUserByIdAsync(user.Id), Times.Once);
        repository.Verify(r => r.Delete(user), Times.Once);
        repository.Verify(r => r.SetOriginalRowVersion(user, command.RowVersion), Times.Once);
        outbox.Verify(r => r.AddAsync(It.IsAny<OutboxMessage>()), Times.Once);
        unitOfWork.Verify(u => u.BeginTransaction(IsolationLevel.Serializable), Times.Once);
        unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        unitOfWork.Verify(u => u.Commit(), Times.Never);
        unitOfWork.Verify(u => u.Rollback(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenSaveFailure_RollsBackAndThrows()
    {
        // Arrange
        var user = AuthUser.Create("sue@example.com", "password-hash", []);
        var command = new DeleteAuthUserCommand(user.Id.Value, [1, 2, 3]);
        var repository = new Mock<IAuthUserRepository>();
        var outbox = new Mock<IOutboxRepository>();
        var unitOfWork = new Mock<IUnitOfWork>();
        var logger = new Mock<ILogger<DeleteUserHandler>>();
        repository.Setup(r => r.GetAuthUserByIdAsync(user.Id)).ReturnsAsync(user);
        var expectedException = new InvalidOperationException("Write failed");
        unitOfWork.Setup(u => u.SaveChangesAsync()).ThrowsAsync(expectedException);
        IDeleteUserHandler handler = new DeleteUserHandler(
            repository.Object, outbox.Object, unitOfWork.Object, logger.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => handler.HandleAsync(command));
        Assert.Same(expectedException, exception);

        repository.Verify(r => r.GetAuthUserByIdAsync(user.Id), Times.Once);
        repository.Verify(r => r.Delete(user), Times.Once);
        repository.Verify(r => r.SetOriginalRowVersion(user, command.RowVersion), Times.Once);
        outbox.Verify(r => r.AddAsync(It.IsAny<OutboxMessage>()), Times.Once);
        unitOfWork.Verify(u => u.BeginTransaction(IsolationLevel.Serializable), Times.Once);
        unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        unitOfWork.Verify(u => u.Commit(), Times.Never);
        unitOfWork.Verify(u => u.Rollback(), Times.Once);
    }
}
