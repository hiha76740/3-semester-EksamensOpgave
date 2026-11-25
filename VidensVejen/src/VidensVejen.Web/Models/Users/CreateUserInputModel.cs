using System.ComponentModel.DataAnnotations;

namespace VidensVejen.Web.Models.Users;

/// <summary>
/// Holds the form values required to register a user.
/// </summary>
public sealed class CreateUserInputModel
{
    /// <summary>Gets or sets the user's first name.</summary>
    [Required(ErrorMessage = "Indtast fornavn.")]
    public string Firstname { get; set; } = string.Empty;

    /// <summary>Gets or sets the user's surname.</summary>
    [Required(ErrorMessage = "Indtast efternavn.")]
    public string Surname { get; set; } = string.Empty;

    /// <summary>Gets or sets the user's email address.</summary>
    [Required(ErrorMessage = "Indtast e-mail.")]
    [EmailAddress(ErrorMessage = "Indtast en gyldig e-mailadresse.")]
    public string Email { get; set; } = string.Empty;

    /// <summary>Gets or sets the password to use for the new account.</summary>
    [Required(ErrorMessage = "Indtast en adgangskode.")]
    public string Password { get; set; } = string.Empty;

    /// <summary>Gets or sets the identifiers of the permissions assigned to the account.</summary>
    public IEnumerable<Guid> PermissionIds { get; set; } = [];
}
