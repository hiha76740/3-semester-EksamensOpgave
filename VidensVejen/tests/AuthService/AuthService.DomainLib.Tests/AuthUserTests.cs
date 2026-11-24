using AuthService.DomainLib.Entities;
using AuthService.DomainLib.Enums;
using AuthService.DomainLib.ValueObjects;
using VidensVejen.SharedKernelLib.Exceptions;

namespace AuthService.DomainLib.Tests;

public class AuthUserTests
{
    private static readonly string s_emailAddress = "henrik@example.com";
    private static readonly string s_passwordHash = "test-password-hash";

    private static AuthUser CreateAuthUser(
        string? emailAddress = null,
        string? passwordHash = null,
        AccessPermission[]? permissions = null
        )
    {
        return AuthUser.Create(
            emailAddress ?? s_emailAddress,
            passwordHash ?? s_passwordHash,
            [.. permissions ?? []]
            );
    }

    [Fact]
    public void Create_GivenValidInput_InitializesPendingUser()
    {
        // Arrange
        AccessPermission[] expectedPermissions =
        [
            new AccessPermission("User.Create"),
            new AccessPermission("Quiz.Edit")
        ];
        var expectedEmail = new Email(s_emailAddress);

        // Act
        var user = CreateAuthUser(permissions: expectedPermissions);

        // Assert
        Assert.NotNull(user.Id);
        Assert.Equal(expectedEmail, user.Email);
        Assert.Equal(s_passwordHash, user.PasswordHash);
        Assert.Collection(
            user.Permissions,
            permission => Assert.Same(expectedPermissions[0], permission),
            permission => Assert.Same(expectedPermissions[1], permission));
        Assert.Equal(AuthUserStatus.Pending, user.Status);
        Assert.Null(user.RefreshToken);
        Assert.Null(user.RefreshTokenExpiryTime);
    }

    [Fact]
    public void Create_GivenNoPermissions_CreatesUserWithoutPermissions()
    {
        // Arrange
        AccessPermission[] permissions = [];

        // Act
        var user = CreateAuthUser(permissions: permissions);

        // Assert
        Assert.Empty(user.Permissions);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_GivenBlankEmail_ThrowsDomainException(string? invalidValue)
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => AuthUser.Create(invalidValue!, s_passwordHash, []));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_GivenBlankPasswordHash_ThrowsDomainException(string? invalidValue)
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => AuthUser.Create(s_emailAddress, invalidValue!, []));
    }

    [Fact]
    public void SetRefreshToken_GivenRefreshExpiryIsTrue_SetsTokenAndSevenDayExpiry()
    {
        // Arrange
        var user = CreateAuthUser();
        var earliestExpiry = DateTime.UtcNow.AddDays(7);

        // Act
        user.SetRefreshToken("first-token", refreshExpiryTime: true);
        var latestExpiry = DateTime.UtcNow.AddDays(7);

        // Assert
        Assert.Equal("first-token", user.RefreshToken);
        var expiry = Assert.IsType<DateTime>(user.RefreshTokenExpiryTime);
        Assert.InRange(expiry, earliestExpiry, latestExpiry);
        Assert.Equal(DateTimeKind.Utc, expiry.Kind);
    }

    [Fact]
    public void SetRefreshToken_GivenRefreshExpiryIsFalse_KeepsExistingExpiry()
    {
        // Arrange
        var user = CreateAuthUser();
        user.SetRefreshToken("first-token", refreshExpiryTime: true);
        var originalExpiry = user.RefreshTokenExpiryTime;

        // Act
        user.SetRefreshToken("second-token", refreshExpiryTime: false);

        // Assert
        Assert.Equal("second-token", user.RefreshToken);
        Assert.Equal(originalExpiry, user.RefreshTokenExpiryTime);
    }

    [Fact]
    public void SetRefreshToken_GivenRefreshExpiryIsFalseAndNoExistingExpiry_LeavesExpiryUnset()
    {
        // Arrange
        var user = CreateAuthUser();

        // Act
        user.SetRefreshToken("first-token", refreshExpiryTime: false);

        // Assert
        Assert.Equal("first-token", user.RefreshToken);
        Assert.Null(user.RefreshTokenExpiryTime);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void SetRefreshToken_GivenBlankToken_ThrowsAndPreservesExistingToken(string? invalidValue)
    {
        // Arrange
        var user = CreateAuthUser();
        user.SetRefreshToken("first-token", refreshExpiryTime: true);
        var originalExpiry = user.RefreshTokenExpiryTime;

        // Act & Assert
        Assert.Throws<DomainException>(
            () => user.SetRefreshToken(invalidValue!, refreshExpiryTime: true));

        Assert.Equal("first-token", user.RefreshToken);
        Assert.Equal(originalExpiry, user.RefreshTokenExpiryTime);
    }

    [Fact]
    public void ChangeEmail_GivenDifferentEmail_UpdatesEmail()
    {
        // Arrange
        var user = CreateAuthUser();
        const string NewEmail = "new@example.com";
        var expectedEmail = new Email(NewEmail);

        // Act
        user.ChangeEmail(NewEmail);

        // Assert
        Assert.Equal(expectedEmail, user.Email);
    }

    [Fact]
    public void ChangeEmail_GivenSameEmail_ThrowsDomainException()
    {
        // Arrange
        var user = CreateAuthUser();

        // Act & Assert
        Assert.Throws<DomainException>(() => user.ChangeEmail(s_emailAddress));
    }

    [Fact]
    public void SetStatusActive_GivenPendingUser_SetsStatusToActive()
    {
        // Arrange
        var user = CreateAuthUser();

        // Act
        user.SetStatusActive();

        // Assert
        Assert.Equal(AuthUserStatus.Active, user.Status);
    }

    [Fact]
    public void SetStatusActive_GivenAlreadyActiveUser_ThrowsDomainException()
    {
        // Arrange
        var user = CreateAuthUser();
        user.SetStatusActive();

        // Act & Assert
        Assert.Throws<DomainException>(user.SetStatusActive);
    }

    [Fact]
    public void ChangePassword_GivenValidValue_UpdatesProperty()
    {
        // Arrange
        var user = AuthUser.Create("sue@example.com", "old-hash", []);

        // Act
        user.ChangePassword("new-hash");

        // Assert
        Assert.Equal("new-hash", user.PasswordHash);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ChangePassword_GivenBlankValue_ThrowsDomainException(string? value)
    {
        // Arrange
        var user = AuthUser.Create("sue@example.com", "old-hash", []);
        var original = user.PasswordHash;

        // Act & Assert
        Assert.Throws<DomainException>(() => user.ChangePassword(value!));
        Assert.Equal(original, user.PasswordHash);
    }

    [Fact]
    public void SetStatusPending_GivenActiveUser_ReturnsToPending()
    {
        // Arrange
        var user = AuthUser.Create("sue@example.com", "hash", []);
        user.SetStatusActive();

        // Act
        user.SetStatusPending();

        // Assert
        Assert.Equal(AuthUserStatus.Pending, user.Status);
    }

    [Fact]
    public void SetStatusPending_GivenPendingUser_ThrowsDomainException()
    {
        // Arrange
        var user = AuthUser.Create("sue@example.com", "hash", []);

        // Act & Assert
        Assert.Throws<DomainException>(user.SetStatusPending);
    }

    [Fact]
    public void AddPermission_GivenNewPermission_AddsIt()
    {
        // Arrange
        var user = AuthUser.Create("sue@example.com", "hash", []);
        var permission = new AccessPermission("Quiz.Edit");

        // Act
        user.AddPermission(permission);

        // Assert
        Assert.Same(permission, Assert.Single(user.Permissions));
    }

    [Fact]
    public void AddPermission_GivenExistingPermission_ThrowsWithoutDuplicating()
    {
        // Arrange
        var permission = new AccessPermission("Quiz.Edit");
        var user = AuthUser.Create("sue@example.com", "hash", [permission]);

        // Act & Assert
        Assert.Throws<DomainException>(() => user.AddPermission(permission));
        Assert.Same(permission, Assert.Single(user.Permissions));
    }

    [Fact]
    public void RemovePermission_GivenExistingPermission_RemovesIt()
    {
        // Arrange
        var permission = new AccessPermission("Quiz.Edit");
        var user = AuthUser.Create("sue@example.com", "hash", [permission]);

        // Act
        user.RemovePermission(permission);

        // Assert
        Assert.Empty(user.Permissions);
    }

    [Fact]
    public void RemovePermission_GivenUnknownPermission_Throws()
    {
        // Arrange
        var user = AuthUser.Create("sue@example.com", "hash", []);
        var permission = new AccessPermission("Quiz.Edit");

        // Act & Assert
        Assert.Throws<DomainException>(() => user.RemovePermission(permission));
        Assert.Empty(user.Permissions);
    }
}
