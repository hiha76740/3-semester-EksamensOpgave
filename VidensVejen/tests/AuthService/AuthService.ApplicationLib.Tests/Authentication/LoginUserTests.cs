using AuthService.ApplicationLib.Authentication;
using AuthService.ApplicationLib.Handlers.Authentication;
using AuthService.ApplicationLib.Repositories;
using AuthService.DomainLib.Entities;
using AuthService.DomainLib.ValueObjects;
using AuthService.FacadeLib.Commands.DTOs;
using AuthService.FacadeLib.Commands.Interfaces;
using Moq;
using VidensVejen.SharedKernelLib.Exceptions;

namespace AuthService.ApplicationLib.Tests.Authentication;

public class LoginUserTests
{
    private const string EmailAddress = "sue@example.com";
    private const string Password = "test-password";
    private const string PasswordHash = "test-password-hash";
    private const string AccessToken = "test-access-token";
    private const string RefreshToken = "test-refresh-token";

    [Fact]
    public async Task Handle_GivenValidCredentials_ReturnsTokensAndSavesRefreshToken()
    {
        // Arrange
        var mockAuthUserRepo = new Mock<IAuthUserRepository>();
        var mockPasswordHashService = new Mock<IPasswordHashService>();
        var mockTokenService = new Mock<ITokenService>();
        var mockRefreshTokenService = new Mock<IRefreshTokenService>();
        var command = new LoginUserCommand(Email: EmailAddress, Password: Password);
        var email = new Email(EmailAddress);
        var authUser = AuthUser.Create(EmailAddress, PasswordHash, []);
        authUser.SetStatusActive();

        mockAuthUserRepo
            .Setup(r => r.GetAuthUserByEmailAsync(email))
            .ReturnsAsync(authUser);

        mockPasswordHashService
            .Setup(s => s.Verify(Password, PasswordHash))
            .Returns(true);

        mockTokenService
            .Setup(s => s.CreateToken(authUser))
            .Returns(AccessToken);

        mockRefreshTokenService
            .Setup(s => s.GenerateRefreshToken())
            .Returns(RefreshToken);

        ILoginUserHandler handler = new LoginUserHandler(
            mockAuthUserRepo.Object,
            mockPasswordHashService.Object,
            mockTokenService.Object,
            mockRefreshTokenService.Object);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(new TokenDto(AccessToken, RefreshToken), result);
        Assert.Equal(RefreshToken, authUser.RefreshToken);
        Assert.NotNull(authUser.RefreshTokenExpiryTime);
        mockAuthUserRepo.Verify(r => r.GetAuthUserByEmailAsync(email), Times.Once);
        mockPasswordHashService.Verify(
            s => s.Verify(Password, PasswordHash),
            Times.Once);
        mockTokenService.Verify(s => s.CreateToken(authUser), Times.Once);
        mockRefreshTokenService.Verify(s => s.GenerateRefreshToken(), Times.Once);
        mockAuthUserRepo.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenUnknownEmail_ReturnsNull()
    {
        // Arrange
        var mockAuthUserRepo = new Mock<IAuthUserRepository>();
        var mockPasswordHashService = new Mock<IPasswordHashService>();
        var mockTokenService = new Mock<ITokenService>();
        var mockRefreshTokenService = new Mock<IRefreshTokenService>();
        var command = new LoginUserCommand(Email: EmailAddress, Password: Password);
        var email = new Email(EmailAddress);

        mockAuthUserRepo
            .Setup(r => r.GetAuthUserByEmailAsync(email))
            .ReturnsAsync((AuthUser?)null);

        ILoginUserHandler handler = new LoginUserHandler(
            mockAuthUserRepo.Object,
            mockPasswordHashService.Object,
            mockTokenService.Object,
            mockRefreshTokenService.Object);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.Null(result);
        mockAuthUserRepo.Verify(r => r.GetAuthUserByEmailAsync(email), Times.Once);
        mockPasswordHashService.Verify(
            s => s.Verify(It.IsAny<string>(), It.IsAny<string>()),
            Times.Never);
        mockTokenService.Verify(s => s.CreateToken(It.IsAny<AuthUser>()), Times.Never);
        mockRefreshTokenService.Verify(s => s.GenerateRefreshToken(), Times.Never);
        mockAuthUserRepo.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenWrongPassword_ReturnsNull()
    {
        // Arrange
        var mockAuthUserRepo = new Mock<IAuthUserRepository>();
        var mockPasswordHashService = new Mock<IPasswordHashService>();
        var mockTokenService = new Mock<ITokenService>();
        var mockRefreshTokenService = new Mock<IRefreshTokenService>();
        var command = new LoginUserCommand(Email: EmailAddress, Password: Password);
        var email = new Email(EmailAddress);
        var authUser = AuthUser.Create(EmailAddress, PasswordHash, []);

        mockAuthUserRepo
            .Setup(r => r.GetAuthUserByEmailAsync(email))
            .ReturnsAsync(authUser);

        mockPasswordHashService
            .Setup(s => s.Verify(Password, PasswordHash))
            .Returns(false);

        ILoginUserHandler handler = new LoginUserHandler(
            mockAuthUserRepo.Object,
            mockPasswordHashService.Object,
            mockTokenService.Object,
            mockRefreshTokenService.Object);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.Null(result);
        mockAuthUserRepo.Verify(r => r.GetAuthUserByEmailAsync(email), Times.Once);
        mockPasswordHashService.Verify(
            s => s.Verify(Password, PasswordHash),
            Times.Once);
        mockTokenService.Verify(s => s.CreateToken(It.IsAny<AuthUser>()), Times.Never);
        mockRefreshTokenService.Verify(s => s.GenerateRefreshToken(), Times.Never);
        mockAuthUserRepo.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenPendingUserWithCorrectPassword_ThrowsWithoutIssuingTokens()
    {
        // Arrange
        var user = AuthUser.Create(EmailAddress, PasswordHash, []);
        var repository = new Mock<IAuthUserRepository>();
        var passwordService = new Mock<IPasswordHashService>();
        var tokenService = new Mock<ITokenService>();
        var refreshTokenService = new Mock<IRefreshTokenService>();
        repository.Setup(r => r.GetAuthUserByEmailAsync(new Email(EmailAddress))).ReturnsAsync(user);
        passwordService.Setup(s => s.Verify(Password, PasswordHash)).Returns(true);
        ILoginUserHandler handler = new LoginUserHandler(
            repository.Object, passwordService.Object, tokenService.Object, refreshTokenService.Object);
        var command = new LoginUserCommand(EmailAddress, Password);

        // Act & Assert
        await Assert.ThrowsAsync<UserNotActivatedException>(() => handler.HandleAsync(command));

        Assert.Null(user.RefreshToken);
        Assert.Null(user.RefreshTokenExpiryTime);
        tokenService.VerifyNoOtherCalls();
        refreshTokenService.VerifyNoOtherCalls();
        repository.Verify(r => r.SaveAsync(), Times.Never);
    }
}
