using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using VidensVejen.ContractsLib.Requests.AuthService;
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

    async Task<IReadOnlyList<PermissionResponse>> IAuthUserService.GetPermissionsAsync()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "/permissions-api");
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        using var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<IReadOnlyList<PermissionResponse>>()
            ?? throw new JsonException("Could not deserialize permissions.");
    }

    async Task<HttpStatusCode> IAuthUserService.RegisterAsync(RegisterUserRequest registration)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/register")
        {
            Content = JsonContent.Create(registration)
        };
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        using var response = await httpClient.SendAsync(request);
        return response.StatusCode;
    }
}
