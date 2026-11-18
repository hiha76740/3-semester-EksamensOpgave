using System.Security.Claims;
using AuthService.ApplicationLib.Authentication;
using AuthService.ApplicationLib.Handlers.Authentication;
using AuthService.ApplicationLib.Repositories;
using AuthService.DomainLib.Entities;
using AuthService.DomainLib.ValueObjects;
using AuthService.FacadeLib.Commands.DTOs;
using AuthService.FacadeLib.Commands.Interfaces;
using Moq;

namespace AuthService.ApplicationLib.Tests.Authentication;

public class RefreshTokenTests
{
    private const string EmailAddress = "sue@example.com";
    private const string ExpiredAccessToken = "expired-access-token";
    private const string CurrentRefreshToken = "current-refresh-token";
    private const string NewAccessToken = "new-access-token";
    private const string NewRefreshToken = "new-refresh-token";

    private static ClaimsPrincipal CreatePrincipal()
    {
        return new ClaimsPrincipal(
            new ClaimsIdentity([new Claim(ClaimTypes.Name, EmailAddress)], "Test"));
    }

    [Fact]
    public async Task Handle_GivenValidRefreshToken_RotatesTokensAndPreservesExpiry()
    {
        // Arrange
        var mockAuthUserRepo = new Mock<IAuthUserRepository>();
        var mockTokenService = new Mock<ITokenService>();
        var mockRefreshTokenService = new Mock<IRefreshTokenService>();
        var command = new RefreshTokenCommand(ExpiredAccessToken, CurrentRefreshToken);
        var principal = CreatePrincipal();
        var email = new Email(EmailAddress);
        var authUser = AuthUser.Create(EmailAddress, "test-password-hash", []);
        authUser.SetRefreshToken(CurrentRefreshToken, true);
        var originalExpiry = authUser.RefreshTokenExpiryTime;

        mockTokenService
            .Setup(s => s.GetPrincipalFromExpiredToken(ExpiredAccessToken))
            .Returns(principal);

        mockAuthUserRepo
            .Setup(r => r.GetAuthUserByEmailAsync(email))
            .ReturnsAsync(authUser);

        mockTokenService
            .Setup(s => s.CreateToken(authUser))
            .Returns(NewAccessToken);

        mockRefreshTokenService
            .Setup(s => s.GenerateRefreshToken())
            .Returns(NewRefreshToken);

        IRefreshTokenHandler handler = new RefreshTokenHandler(
            mockAuthUserRepo.Object,
            mockTokenService.Object,
            mockRefreshTokenService.Object);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(new TokenDto(NewAccessToken, NewRefreshToken), result);
        Assert.Equal(NewRefreshToken, authUser.RefreshToken);
        Assert.Equal(originalExpiry, authUser.RefreshTokenExpiryTime);
        mockTokenService.Verify(s => s.GetPrincipalFromExpiredToken(ExpiredAccessToken), Times.Once);
        mockAuthUserRepo.Verify(r => r.GetAuthUserByEmailAsync(email), Times.Once);
        mockTokenService.Verify(s => s.CreateToken(authUser), Times.Once);
        mockRefreshTokenService.Verify(s => s.GenerateRefreshToken(), Times.Once);
        mockAuthUserRepo.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenUnknownUser_ReturnsNull()
    {
        // Arrange
        var mockAuthUserRepo = new Mock<IAuthUserRepository>();
        var mockTokenService = new Mock<ITokenService>();
        var mockRefreshTokenService = new Mock<IRefreshTokenService>();
        var command = new RefreshTokenCommand(ExpiredAccessToken, CurrentRefreshToken);
        var principal = CreatePrincipal();
        var email = new Email(EmailAddress);

        mockTokenService
            .Setup(s => s.GetPrincipalFromExpiredToken(ExpiredAccessToken))
            .Returns(principal);

        mockAuthUserRepo
            .Setup(r => r.GetAuthUserByEmailAsync(email))
            .ReturnsAsync((AuthUser?)null);

        IRefreshTokenHandler handler = new RefreshTokenHandler(
            mockAuthUserRepo.Object,
            mockTokenService.Object,
            mockRefreshTokenService.Object);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.Null(result);
        mockTokenService.Verify(s => s.GetPrincipalFromExpiredToken(ExpiredAccessToken), Times.Once);
        mockAuthUserRepo.Verify(r => r.GetAuthUserByEmailAsync(email), Times.Once);
        mockTokenService.Verify(s => s.CreateToken(It.IsAny<AuthUser>()), Times.Never);
        mockRefreshTokenService.Verify(s => s.GenerateRefreshToken(), Times.Never);
        mockAuthUserRepo.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenMismatchedRefreshToken_ReturnsNull()
    {
        // Arrange
        var mockAuthUserRepo = new Mock<IAuthUserRepository>();
        var mockTokenService = new Mock<ITokenService>();
        var mockRefreshTokenService = new Mock<IRefreshTokenService>();
        var command = new RefreshTokenCommand(ExpiredAccessToken, "wrong-refresh-token");
        var principal = CreatePrincipal();
        var email = new Email(EmailAddress);
        var authUser = AuthUser.Create(EmailAddress, "test-password-hash", []);
        authUser.SetRefreshToken(CurrentRefreshToken, true);
        var originalExpiry = authUser.RefreshTokenExpiryTime;

        mockTokenService
            .Setup(s => s.GetPrincipalFromExpiredToken(ExpiredAccessToken))
            .Returns(principal);

        mockAuthUserRepo
            .Setup(r => r.GetAuthUserByEmailAsync(email))
            .ReturnsAsync(authUser);

        IRefreshTokenHandler handler = new RefreshTokenHandler(
            mockAuthUserRepo.Object,
            mockTokenService.Object,
            mockRefreshTokenService.Object);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.Null(result);
        Assert.Equal(CurrentRefreshToken, authUser.RefreshToken);
        Assert.Equal(originalExpiry, authUser.RefreshTokenExpiryTime);
        mockTokenService.Verify(s => s.GetPrincipalFromExpiredToken(ExpiredAccessToken), Times.Once);
        mockAuthUserRepo.Verify(r => r.GetAuthUserByEmailAsync(email), Times.Once);
        mockTokenService.Verify(s => s.CreateToken(It.IsAny<AuthUser>()), Times.Never);
        mockRefreshTokenService.Verify(s => s.GenerateRefreshToken(), Times.Never);
        mockAuthUserRepo.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenExpiredRefreshToken_ReturnsNull()
    {
        // Arrange
        var mockAuthUserRepo = new Mock<IAuthUserRepository>();
        var mockTokenService = new Mock<ITokenService>();
        var mockRefreshTokenService = new Mock<IRefreshTokenService>();
        var command = new RefreshTokenCommand(ExpiredAccessToken, CurrentRefreshToken);
        var principal = CreatePrincipal();
        var email = new Email(EmailAddress);
        var authUser = AuthUser.Create(EmailAddress, "test-password-hash", []);
        authUser.SetRefreshToken(CurrentRefreshToken, true);
        // Set the existing expiry in the past without waiting for it to expire.
        var expiryProperty = typeof(AuthUser).GetProperty(nameof(AuthUser.RefreshTokenExpiryTime));
        expiryProperty!.SetValue(authUser, DateTime.UtcNow.AddDays(-1));
        var originalExpiry = authUser.RefreshTokenExpiryTime;

        mockTokenService
            .Setup(s => s.GetPrincipalFromExpiredToken(ExpiredAccessToken))
            .Returns(principal);

        mockAuthUserRepo
            .Setup(r => r.GetAuthUserByEmailAsync(email))
            .ReturnsAsync(authUser);

        IRefreshTokenHandler handler = new RefreshTokenHandler(
            mockAuthUserRepo.Object,
            mockTokenService.Object,
            mockRefreshTokenService.Object);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.Null(result);
        Assert.Equal(CurrentRefreshToken, authUser.RefreshToken);
        Assert.Equal(originalExpiry, authUser.RefreshTokenExpiryTime);
        mockTokenService.Verify(s => s.GetPrincipalFromExpiredToken(ExpiredAccessToken), Times.Once);
        mockAuthUserRepo.Verify(r => r.GetAuthUserByEmailAsync(email), Times.Once);
        mockTokenService.Verify(s => s.CreateToken(It.IsAny<AuthUser>()), Times.Never);
        mockRefreshTokenService.Verify(s => s.GenerateRefreshToken(), Times.Never);
        mockAuthUserRepo.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenIdentityWithoutName_ThrowsInvalidOperationException()
    {
        // Arrange
        var mockAuthUserRepo = new Mock<IAuthUserRepository>();
        var mockTokenService = new Mock<ITokenService>();
        var mockRefreshTokenService = new Mock<IRefreshTokenService>();
        var command = new RefreshTokenCommand(ExpiredAccessToken, CurrentRefreshToken);
        var principal = new ClaimsPrincipal(new ClaimsIdentity());

        mockTokenService
            .Setup(s => s.GetPrincipalFromExpiredToken(ExpiredAccessToken))
            .Returns(principal);

        IRefreshTokenHandler handler = new RefreshTokenHandler(
            mockAuthUserRepo.Object,
            mockTokenService.Object,
            mockRefreshTokenService.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));
        mockTokenService.Verify(s => s.GetPrincipalFromExpiredToken(ExpiredAccessToken), Times.Once);
        mockAuthUserRepo.Verify(r => r.GetAuthUserByEmailAsync(It.IsAny<Email>()), Times.Never);
        mockTokenService.Verify(s => s.CreateToken(It.IsAny<AuthUser>()), Times.Never);
        mockRefreshTokenService.Verify(s => s.GenerateRefreshToken(), Times.Never);
        mockAuthUserRepo.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenNullRefreshToken_ReturnsNullWithoutIssuingTokens()
    {
        // Arrange
        var user = AuthUser.Create(EmailAddress, "password-hash", []);
        var repository = new Mock<IAuthUserRepository>();
        var tokenService = new Mock<ITokenService>();
        var refreshTokenService = new Mock<IRefreshTokenService>();
        repository.Setup(r => r.GetAuthUserByEmailAsync(new Email(EmailAddress))).ReturnsAsync(user);
        tokenService.Setup(s => s.GetPrincipalFromExpiredToken(ExpiredAccessToken)).Returns(CreatePrincipal());
        IRefreshTokenHandler handler = new RefreshTokenHandler(
            repository.Object, tokenService.Object, refreshTokenService.Object);
        var command = new RefreshTokenCommand(ExpiredAccessToken, null!);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.Null(result);
        tokenService.Verify(s => s.CreateToken(It.IsAny<AuthUser>()), Times.Never);
        refreshTokenService.VerifyNoOtherCalls();
        repository.Verify(r => r.SaveAsync(), Times.Never);
        Assert.Null(user.RefreshToken);
        Assert.Null(user.RefreshTokenExpiryTime);
    }
}
