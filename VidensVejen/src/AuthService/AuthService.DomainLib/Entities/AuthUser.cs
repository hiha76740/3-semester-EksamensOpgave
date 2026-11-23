using AuthService.DomainLib.Enums;
using AuthService.DomainLib.ValueObjects;
using VidensVejen.SharedKernelLib.Exceptions;

namespace AuthService.DomainLib.Entities;

/// <summary>
/// 
/// </summary>
public class AuthUser
{
    /// <summary>
    /// 
    /// </summary>
    public AuthUserId Id { get; init; } = null!;
    /// <summary>
    /// 
    /// </summary>
    public Email Email { get; private set; } = null!;
    /// <summary>
    /// 
    /// </summary>
    public string PasswordHash { get; private set; } = string.Empty;
    /// <summary>
    /// 
    /// </summary>
    public string? RefreshToken { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public DateTime? RefreshTokenExpiryTime { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public AuthUserStatus Status { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyList<AccessPermission> Permissions => _permissions.AsReadOnly();

    private readonly List<AccessPermission> _permissions = [];

    /// <summary>
    /// 
    /// </summary>
    /// <param name="emailAddress"></param>
    /// <param name="passwordHash"></param>
    /// <param name="permissions"></param>
    /// <returns></returns>
    /// <exception cref="DomainException"></exception>
    public static AuthUser Create(string emailAddress, string passwordHash, List<AccessPermission> permissions)
    {
        if (string.IsNullOrWhiteSpace(emailAddress))
            throw new DomainException("Email could not be set, no value");

        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Password hash could not be set, no value");

        var email = new Email(emailAddress);

        var user = new AuthUser(email, passwordHash, permissions);

        return user;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="permission"></param>
    /// <exception cref="DomainException"></exception>
    public void AddPermission(AccessPermission permission)
    {
        if (_permissions.Contains(permission) == true)
            throw new DomainException("User already has this permission");

        _permissions.Add(permission);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="permission"></param>
    /// <exception cref="DomainException"></exception>
    public void RemovePermission(AccessPermission permission)
    {
        if (_permissions.Contains(permission) == false)
            throw new DomainException("User does not have this permission");

        _permissions.Remove(permission);
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <param name="refreshExpiryTime"></param>
    /// <exception cref="DomainException"></exception>
    public void SetRefreshToken(string refreshToken, bool refreshExpiryTime)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            throw new DomainException("Refresh token could not be set, no value");

        if (refreshExpiryTime == true)
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        RefreshToken = refreshToken;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newEmail"></param>
    /// <exception cref="DomainException"></exception>
    public void ChangeEmail(string newEmail)
    {
        var email = new Email(newEmail);

        if (Email == email)
            throw new DomainException("New and old email can not be the same");

        Email = email;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="passwordHash"></param>
    /// <exception cref="DomainException"></exception>
    public void ChangePassword(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            throw new DomainException("Passwordhash could not be set, no value");

        PasswordHash = passwordHash;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="DomainException"></exception>
    public void SetStatusActive()
    {
        if (Status == AuthUserStatus.Active)
            throw new DomainException("User is already active");

        Status = AuthUserStatus.Active;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="DomainException"></exception>
    public void SetStatusPending()
    {
        if (Status == AuthUserStatus.Pending)
            throw new DomainException("User status is already pending");

        Status = AuthUserStatus.Pending;
    }

    private AuthUser(Email email, string passwordHash, List<AccessPermission> permissions)
    {
        Id = new AuthUserId(Guid.NewGuid());
        Email = email;
        PasswordHash = passwordHash;
        Status = AuthUserStatus.Pending;
        _permissions = permissions;
    }

    // EF core constructor
    private AuthUser() { }
}
