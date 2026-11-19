using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using VidensVejen.ContractsLib.Requests.AuthService;
using VidensVejen.Web.ServicesInterfaces;

namespace VidensVejen.Web.Services;

/// <summary>
/// 
/// </summary>
/// <param name="httpClient"></param>
public class AuthService(HttpClient httpClient) : IAuthService
{
    private readonly string _baseUrl = "/auth-api";

    async Task<int> IAuthService.Login(string username, string password)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/login")
        {
            Content = JsonContent.Create(new LoginUserRequest(username, password))
        };

        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        using var response = await httpClient.SendAsync(request);

        return (int)response.StatusCode;
    }

    async Task IAuthService.LogOut()
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/logout");

        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        using var response = await httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();
    }
}
