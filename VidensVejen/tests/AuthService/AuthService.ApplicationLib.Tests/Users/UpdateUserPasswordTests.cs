using AuthService.ApplicationLib.Handlers.Users;
using AuthService.ApplicationLib.Repositories;
using AuthService.DomainLib.Entities;
using AuthService.FacadeLib.Commands.DTOs;
using AuthService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using VidensVejen.SharedKernelLib.Exceptions;
using AuthService.ApplicationLib.Authentication;

namespace AuthService.ApplicationLib.Tests.Users;

public class UpdateUserPasswordTests
{

    [Theory]
    [InlineData(true, true)]
    [InlineData(true, false)]
    [InlineData(false, false)]
    public async Task Handle_GivenUserAndPassword_OnlySavesForValidCredentials(bool found, bool passwordMatches)
    {
        // Arrange
        var user = AuthUser.Create("sue@example.com", "old-hash", []);
        var repository = new Mock<IAuthUserRepository>();
        var passwords = new Mock<IPasswordHashService>();
        repository.Setup(r => r.GetAuthUserByIdAsync(user.Id)).ReturnsAsync(found ? user : null);
        passwords.Setup(s => s.Verify("old-password", "old-hash")).Returns(passwordMatches);
        passwords.Setup(s => s.Hash("new-password")).Returns("new-hash");
        byte[] version = [1, 2, 3];
        var command = new UpdateAuthUserPasswordCommand(user.Id.Value, "new-password", "old-password", version);
        IUpdateUserPasswordHandler handler = new UpdateUserPasswordHandler(
            repository.Object, passwords.Object, new Mock<ILogger<UpdateUserPasswordHandler>>().Object);

        // Act & Assert
        if (found == false)
        {
            await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command));
            passwords.VerifyNoOtherCalls();
        }
        else if (passwordMatches == false)
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));
            passwords.Verify(s => s.Hash(It.IsAny<string>()), Times.Never);
        }
        else
        {
            await handler.HandleAsync(command);
            Assert.Equal("new-hash", user.PasswordHash);
            repository.Verify(r => r.SetOriginalRowVersion(user, version), Times.Once);
        }

        repository.Verify(r => r.SaveAsync(), found && passwordMatches ? Times.Once() : Times.Never());
    }
}
