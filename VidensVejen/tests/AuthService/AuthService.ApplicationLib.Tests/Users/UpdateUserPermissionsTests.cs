using AuthService.ApplicationLib.Handlers.Users;
using AuthService.ApplicationLib.Repositories;
using AuthService.DomainLib.Entities;
using AuthService.FacadeLib.Commands.DTOs;
using AuthService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using VidensVejen.SharedKernelLib.Exceptions;

namespace AuthService.ApplicationLib.Tests.Users;

public class UpdateUserPermissionsTests
{

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task Handle_GivenPermissionIds_SavesEmptyListOrRejectsUnknownPermission(bool unknownPermission)
    {
        // Arrange
        var user = AuthUser.Create("sue@example.com", "hash", []);
        var repository = new Mock<IAuthUserRepository>();
        var permissions = new Mock<IPermissionRepository>();
        repository.Setup(r => r.GetAuthUserByIdAsync(user.Id)).ReturnsAsync(user);
        byte[] version = [1, 2, 3];
        List<Guid> permissionIds = [];
        if (unknownPermission)
        {
            permissionIds.Add(Guid.NewGuid());
        }
        IUpdateUserPermissionsHandler handler = new UpdateUserPermissionsHandler(
            repository.Object, permissions.Object, new Mock<ILogger<UpdateUserPermissionsHandler>>().Object);
        var command = new UpdateAuthUserPermissionsCommand(user.Id.Value, permissionIds, version);

        // Act & Assert
        if (unknownPermission)
        {
            await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command));
            repository.Verify(r => r.SaveAsync(), Times.Never);
            repository.Verify(r => r.SetOriginalRowVersion(It.IsAny<AuthUser>(), It.IsAny<byte[]>()), Times.Never);
        }
        else
        {
            var result = await handler.HandleAsync(command);
            Assert.Empty(user.Permissions);
            Assert.NotNull(result);
            repository.Verify(r => r.SetOriginalRowVersion(user, version), Times.Once);
            repository.Verify(r => r.SaveAsync(), Times.Once);
        }
    }
}
