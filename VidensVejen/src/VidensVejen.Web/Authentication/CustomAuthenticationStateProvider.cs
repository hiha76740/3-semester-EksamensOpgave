using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using VidensVejen.ContractsLib.Authentication;
using VidensVejen.Web.ServicesInterfaces;

namespace VidensVejen.Web.Authentication;

/// <summary>
/// 
/// </summary>
/// <param name="authUserService"></param>
public class CustomAuthenticationStateProvider(IAuthUserService authUserService) : AuthenticationStateProvider
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var authUser = await authUserService.GetAuthUserAsync();

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, authUser.Id.ToString()),
                new(ClaimTypes.Name, authUser.Email)
            };

            foreach (var permission in authUser.Permissions)
            {
                claims.Add(new Claim(CustomClaimTypes.Permission, permission.Name));
            }

            var identity = new ClaimsIdentity(claims, "ApiAuthentication");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }
        catch (HttpRequestException ex)
        when (ex.StatusCode == HttpStatusCode.Unauthorized)
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());

            return new AuthenticationState(anonymousUser);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task RefreshAuthenticationState()
    {
        var state = await GetAuthenticationStateAsync();

        NotifyAuthenticationStateChanged(Task.FromResult(state));
    }
}
