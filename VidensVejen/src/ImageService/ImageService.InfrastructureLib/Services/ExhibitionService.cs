using System.Net.Http.Json;
using Dapr.Client;
using ImageService.ApplicationLib.Services;
using ImageService.DomainLib.Entities;

namespace ImageService.InfrastructureLib.Services;

/// <summary>
/// 
/// </summary>
/// <param name="daprClient"></param>
public class ExhibitionService(DaprClient daprClient) : IExhibitionService
{
    private readonly HttpClient _client = daprClient.CreateInvokableHttpClient("exhibition-api");

    private readonly string _baseUrl = "/v1/exhibition";

    async Task<bool> IExhibitionService.ImageInUseAsync(ImageId imageId)
    {
        using var response = await _client.GetAsync($"{_baseUrl}/image/inuse?id={imageId.Value}");

        response.EnsureSuccessStatusCode();

        var exists = await response.Content.ReadFromJsonAsync<bool>();

        return exists;
    }
}
