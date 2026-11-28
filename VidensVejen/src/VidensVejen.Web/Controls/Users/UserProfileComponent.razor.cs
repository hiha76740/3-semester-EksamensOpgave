using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using VidensVejen.ContractsLib.Requests.AuthService;
using VidensVejen.ContractsLib.Requests.UserService;
using VidensVejen.ContractsLib.Responses.AuthService;
using VidensVejen.ContractsLib.Responses.UserService;
using VidensVejen.Web.Models.Users;

namespace VidensVejen.Web.Controls.Users;

/// <summary>Coordinates the shared users workflow for its dedicated pages.</summary>
public partial class UserProfileComponent
{
    /// <summary>Gets or sets the identifier supplied by the page route.</summary>
    [Parameter]
    public Guid? UserId { get; set; }

    private string Title => UserId.HasValue ? (_canEdit ? "Ændre bruger" : "Brugerprofil") : "Min profil";

    [CascadingParameter]
    private Task<AuthenticationState>? AuthenticationStateTask { get; set; }

    private UserProfileInputModel _model = new();
    private UserResponse? _profile;
    private AuthUserResponse? _account;
    private string? _error;
    private string? _message;
    private bool _isLoading;
    private bool _isSaving;

    private bool _canAccess;
    private bool _canEdit;
    private bool _isOtherUser;
    private IReadOnlyList<PermissionResponse>? _availablePermissions;
    private IEnumerable<Guid> _selectedPermissionIds = [];

    /// <summary>
    /// Initializes the user profile state and loads the profile when the authenticated user has access.
    /// </summary>
    /// <returns>A task that represents the asynchronous parameter update operation.</returns>
    protected override async Task OnParametersSetAsync()
    {
        _canAccess = false;
        _canEdit = false;
        _isOtherUser = false;
        _availablePermissions = null;
        _selectedPermissionIds = [];
        _profile = null;
        _account = null;
        _model = new UserProfileInputModel();

        if (AuthenticationStateTask == null)
        {
            return;
        }

        var state = await AuthenticationStateTask;
        var user = state.User;
        var isAuthenticated = user.Identity?.IsAuthenticated == true;
        _canEdit = isAuthenticated && (!UserId.HasValue || (await Authorization.AuthorizeAsync(user, "User.Edit")).Succeeded);
        _canAccess = _canEdit || (isAuthenticated && UserId.HasValue
            && (await Authorization.AuthorizeAsync(user, "User.Read")).Succeeded);
        _isOtherUser = UserId is Guid targetId
            && Guid.TryParse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var currentUserId)
            && targetId != currentUserId;
        if (_canAccess)
        {
            await LoadProfileAsync();
        }
    }

    private Task<AuthUserResponse> LoadAccountAsync()
    {
        return UserId is Guid id ? AuthUsers.GetByIdAsync(id) : AuthUsers.GetAuthUserAsync();
    }

    private async Task LoadProfileAsync()
    {
        if (!_canAccess)
        {
            return;
        }

        _isLoading = true;
        _availablePermissions = null;
        _error = null;
        _message = null;
        try
        {
            var currentAccount = await LoadAccountAsync();
            var currentProfile = await Users.GetByIdAsync(currentAccount.Id);
            _account = currentAccount;
            _profile = currentProfile;
            _model = new UserProfileInputModel
            {
                Firstname = currentProfile.Firstname,
                Surname = currentProfile.Surname,
                Email = currentAccount.Email
            };
            if (_isOtherUser && _canEdit)
            {
                var options = await AuthUsers.GetPermissionsAsync();
                _availablePermissions = options.Concat(currentAccount.Permissions)
                    .DistinctBy(permission => permission.Id)
                    .OrderBy(permission => permission.Name)
                    .ToArray();
                _selectedPermissionIds = currentAccount.Permissions.Select(permission => permission.Id).ToArray();
            }
        }
        catch (Exception exception) when (IsServiceError(exception))
        {
            _error = _profile != null && _account != null && _isOtherUser && _canEdit
                ? "Rettighederne kunne ikke hentes. Genindlæs profilen for at prøve igen."
                : "Profilen kunne ikke hentes.";
        }
        finally
        {
            _isLoading = false;
        }
    }

    private Task SaveNameAsync()
    {
        return SaveAsync(async () =>
    {
        var currentProfile = _profile;
        if (currentProfile == null)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(_model.Firstname) || string.IsNullOrWhiteSpace(_model.Surname))
        {
            _error = "Udfyld både fornavn og efternavn.";
            return;
        }

        var updated = await Users.UpdateAsync(currentProfile.Id,
            new UpdateUserRequest(_model.Firstname.Trim(), _model.Surname.Trim(), currentProfile.RowVersion));
        _profile = updated;
        _model.Firstname = updated.Firstname;
        _model.Surname = updated.Surname;
        _message = "Navnet er gemt.";
    });
    }

    private Task SaveEmailAsync()
    {
        return SaveAsync(async () =>
    {
        var currentAccount = _account;
        if (currentAccount == null)
        {
            return;
        }

        var email = _model.Email.Trim();
        if (!new EmailAddressAttribute().IsValid(email) || email.Length == 0)
        {
            _error = "Indtast en gyldig e-mailadresse.";
            return;
        }

        if (string.Equals(email, currentAccount.Email, StringComparison.OrdinalIgnoreCase))
        {
            _model.Email = currentAccount.Email;
            _message = "E-mailadressen er allerede gemt.";
            return;
        }

        var updated = await AuthUsers.UpdateEmailAsync(currentAccount.Id,
            new UpdateAuthUserEmailRequest(email, currentAccount.RowVersion));
        _account = currentAccount with { Email = updated.Email, RowVersion = updated.RowVersion };
        _model.Email = updated.Email;
        _message = "E-mailadressen er gemt.";
        if (!_isOtherUser)
        {
            await AuthStateProvider.RefreshAuthenticationState();
        }
    });
    }

    private Task SavePasswordAsync()
    {
        return SaveAsync(async () =>
    {
        var currentAccount = _account;
        if (currentAccount == null)
        {
            return;
        }

        if ((!_isOtherUser && string.IsNullOrWhiteSpace(_model.CurrentPassword))
            || string.IsNullOrWhiteSpace(_model.NewPassword))
        {
            _error = _isOtherUser
                ? "Indtast en ny adgangskode."
                : "Indtast din nuværende adgangskode og en ny adgangskode.";
            return;
        }

        if (!string.Equals(_model.NewPassword, _model.ConfirmPassword, StringComparison.Ordinal))
        {
            _error = "De to nye adgangskoder skal være ens.";
            return;
        }

        if (_isOtherUser)
        {
            await AuthUsers.ResetPasswordAsync(currentAccount.Id,
                new ResetUserPasswordRequest(_model.NewPassword, currentAccount.RowVersion));
        }
        else
        {
            await AuthUsers.UpdatePasswordAsync(currentAccount.Id,
                new UpdateAuthUserPasswordRequest(_model.NewPassword, _model.CurrentPassword, currentAccount.RowVersion));
        }

        _model.CurrentPassword = string.Empty;
        _model.NewPassword = string.Empty;
        _model.ConfirmPassword = string.Empty;
        _message = "Adgangskoden er ændret.";

        _account = null;
        _account = await LoadAccountAsync();
    });
    }

    private Task SavePermissionsAsync()
    {
        return SaveAsync(async () =>
    {
        if (!_isOtherUser || _account == null || _availablePermissions == null)
        {
            return;
        }

        var updated = await AuthUsers.UpdatePermissionsAsync(_account.Id,
            new UpdateAuthUserPermissionsRequest(_selectedPermissionIds.Distinct().ToList(), _account.RowVersion));
        _account = updated;
        _selectedPermissionIds = updated.Permissions.Select(permission => permission.Id).ToArray();
        _message = "Rettighederne er gemt.";
    });
    }

    private async Task SaveAsync(Func<Task> save)
    {
        if (!_canEdit || _isSaving || _isLoading)
        {
            return;
        }

        _isSaving = true;
        _error = null;
        _message = null;
        try
        {
            await save();
        }
        catch (Exception exception) when (IsServiceError(exception))
        {
            _error = "Profilen kunne ikke opdateres. Kontrollér oplysningerne, eller genindlæs profilen og prøv igen.";
            if (_message != null)
            {
                _error = "Ændringen er gemt, men profilen kunne ikke opdateres i visningen. Genindlæs profilen.";
            }
        }
        finally
        {
            _isSaving = false;
        }
    }

    private static bool IsServiceError(Exception exception)
    {
        return exception is HttpRequestException or JsonException or InvalidOperationException or OperationCanceledException;
    }
}
