using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using VidensVejen.ContractsLib.Requests.ExhibitionService;
using VidensVejen.ContractsLib.Responses.ExhibitionService;
using VidensVejen.Web.ServicesInterfaces;

namespace VidensVejen.Web.Services;

/// <summary>
/// 
/// </summary>
/// <param name="httpClient"></param>
public class ExhibitionService(HttpClient httpClient) : IExhibitionService
{
    private const string BaseUrl = "/exhibitions-api";

    async Task<IReadOnlyList<ExhibitionNameResponse>> IExhibitionService.GetAllNamesAsync()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/names");

        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        using var response = await httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var names = await response.Content.ReadFromJsonAsync<IReadOnlyList<ExhibitionNameResponse>>()
            ?? throw new JsonException("Could not deserialize exhibition names");

        return names;
    }

    async Task<HttpStatusCode> IExhibitionService.CreateAsync(
        CreateExhibitionRequest exhibition)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl)
        {
            Content = JsonContent.Create(exhibition)
        };
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        using var response = await httpClient.SendAsync(request);

        return response.StatusCode;
    }
}
