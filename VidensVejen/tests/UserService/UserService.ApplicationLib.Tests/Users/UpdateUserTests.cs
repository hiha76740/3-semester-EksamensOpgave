using UserService.ApplicationLib.Handlers;
using UserService.ApplicationLib.Repositories;
using UserService.DomainLib.Entities;
using UserService.FacadeLib.Commands.DTOs;
using UserService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using VidensVejen.SharedKernelLib.Exceptions;

namespace UserService.ApplicationLib.Tests.Users;

public class UpdateUserTests
{

    [Theory]
    [InlineData("Susan", "Storm", true)]
    [InlineData("Sue", "Richards", true)]
    [InlineData("Sue", "Storm", false)]
    [InlineData("sue", "storm", false)]
    public async Task Handle_GivenNames_UpdatesAndSavesOnlyWhenChanged(
        string firstname, string surname, bool changed)
    {
        // Arrange
        var user = User.Create(new UserId(Guid.NewGuid()), "Sue", "Storm", "sue@example.com");
        byte[] version = [1, 2, 3];
        var repository = new Mock<IUserRepository>();
        repository.Setup(r => r.GetUserByIdAsync(user.Id)).ReturnsAsync(user);
        IUpdateUserHandler handler = new UpdateUserHandler(repository.Object, new Mock<ILogger<UpdateUserHandler>>().Object);
        var command = new UpdateUserCommand(user.Id.Value, firstname, surname, version);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.Equal(changed ? firstname : "Sue", user.Firstname);
        Assert.Equal(changed ? surname : "Storm", user.Surname);
        Assert.Equal(user.Firstname, result.Firstname);
        Assert.Equal(user.Surname, result.Surname);
        repository.Verify(r => r.SaveAsync(), changed ? Times.Once() : Times.Never());
        repository.Verify(r => r.SetOriginalRowVersion(user, version), changed ? Times.Once() : Times.Never());
    }

    [Fact]
    public async Task Handle_GivenUnknownUser_ThrowsWithoutSaving()
    {
        // Arrange
        var repository = new Mock<IUserRepository>();
        IUpdateUserHandler handler = new UpdateUserHandler(repository.Object, new Mock<ILogger<UpdateUserHandler>>().Object);
        var command = new UpdateUserCommand(Guid.NewGuid(), "Sue", "Storm", []);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command));
        repository.Verify(r => r.SaveAsync(), Times.Never());
    }
}
