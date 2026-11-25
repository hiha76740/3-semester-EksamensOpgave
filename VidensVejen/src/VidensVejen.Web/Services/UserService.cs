using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using VidensVejen.ContractsLib.Requests.UserService;
using VidensVejen.ContractsLib.Responses.UserService;
using VidensVejen.Web.ServicesInterfaces;

namespace VidensVejen.Web.Services;

/// <summary>Calls the UserService profile endpoints.</summary>
/// <param name="httpClient">The HTTP client configured for the API gateway.</param>
public class UserService(HttpClient httpClient) : IUserService
{
    private readonly string _baseUrl = "/users-api";


    Task<IReadOnlyList<UserResponse>> IUserService.GetAllAsync()
    {
        return SendAsync<IReadOnlyList<UserResponse>>(new HttpRequestMessage(HttpMethod.Get, _baseUrl));
    }

    Task<UserResponse> IUserService.GetByIdAsync(Guid id)
    {
        return SendAsync<UserResponse>(new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/{id}"));
    }

    Task<UserResponse> IUserService.UpdateAsync(Guid id, UpdateUserRequest update)
    {
        return SendAsync<UserResponse>(new HttpRequestMessage(HttpMethod.Put, $"{_baseUrl}/{id}")
        {
            Content = JsonContent.Create(update)
        });
    }

    private async Task<TResponse> SendAsync<TResponse>(HttpRequestMessage request) where TResponse : class
    {
        using (request)
        {
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            using var response = await httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>()
                ?? throw new JsonException("Could not deserialize the user service response.");
        }
    }
}
