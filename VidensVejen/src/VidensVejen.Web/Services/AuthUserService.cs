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

    async Task<UserEmailResponse> IAuthUserService.UpdateEmailAsync(Guid id, UpdateAuthUserEmailRequest update)
    {
        using var request = new HttpRequestMessage(HttpMethod.Put, $"{_baseUrl}/{id}/email")
        {
            Content = JsonContent.Create(update)
        };
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        using var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<UserEmailResponse>()
            ?? throw new JsonException("Could not deserialize the updated email.");
    }

    async Task IAuthUserService.UpdatePasswordAsync(Guid id, UpdateAuthUserPasswordRequest update)
    {
        using var request = new HttpRequestMessage(HttpMethod.Put, $"{_baseUrl}/{id}/password")
        {
            Content = JsonContent.Create(update)
        };
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        using var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    async Task<AuthUserResponse> IAuthUserService.GetByIdAsync(Guid id)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/{id}");
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        using var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<AuthUserResponse>()
            ?? throw new JsonException("Could not deserialize the user account.");
    }

    Task IAuthUserService.ResetPasswordAsync(Guid id, ResetUserPasswordRequest request)
    {
        return SendAccountChangeAsync(HttpMethod.Put, $"{_baseUrl}/{id}/password/reset", request);
    }

    Task IAuthUserService.DeleteAsync(Guid id, DeleteUserRequest request)
    {
        return SendAccountChangeAsync(HttpMethod.Delete, $"{_baseUrl}/{id}", request);
    }

    private async Task SendAccountChangeAsync<TRequest>(HttpMethod method, string path, TRequest body)
    {
        using var request = new HttpRequestMessage(method, path) { Content = JsonContent.Create(body) };
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        using var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}
