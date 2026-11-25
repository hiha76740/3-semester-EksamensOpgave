namespace VidensVejen.Web.Models.Users;

/// <summary>Holds the editable values on the current user's profile page.</summary>
public sealed class UserProfileInputModel
{
    /// <summary>Gets or sets the user's first name.</summary>
    public string Firstname { get; set; } = string.Empty;

    /// <summary>Gets or sets the user's surname.</summary>
    public string Surname { get; set; } = string.Empty;

    /// <summary>Gets or sets the user's email address.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the current password used to verify a password change.</summary>
    public string CurrentPassword { get; set; } = string.Empty;

    /// <summary>Gets or sets the requested new password.</summary>
    public string NewPassword { get; set; } = string.Empty;

    /// <summary>Gets or sets the confirmation of the requested new password.</summary>
    public string ConfirmPassword { get; set; } = string.Empty;
}
