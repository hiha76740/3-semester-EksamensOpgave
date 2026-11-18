using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using VidensVejen.ContractsLib.Responses.AuthService;
using VidensVejen.Web.ServicesInterfaces;

namespace VidensVejen.Web.Services;

/// <summary>
/// 
/// </summary>
/// <param name="httpClient"></param>
public class AuthUserService(HttpClient httpClient) : IAuthUserService
{
    private readonly string _baseUrl = "/authuser-api";

    async Task<AuthUserResponse> IAuthUserService.GetAuthUserAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/Me");

        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        using var response = await httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var authUser = await response.Content.ReadFromJsonAsync<AuthUserResponse>()
            ?? throw new InvalidOperationException("Could not deserialize authenticated user");

        return authUser;
    }
}
