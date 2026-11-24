using Dapr.Client;
using ExhibitionService.ApplicationLib.Services;
using System.Net.Http.Json;
using ExhibitionService.DomainLib.ValueObjects;

namespace ExhibitionService.InfrastructureLib.Services;
/// <summary>
/// 
/// </summary>
/// <param name="daprClient"></param>
public class ImageService(DaprClient daprClient) : IImageService
{
    private readonly HttpClient _client = daprClient.CreateInvokableHttpClient("imageservice-api");

    private readonly string _baseUrl = "/v1/images";

    async Task<bool> IImageService.ExistsAsync(ImageId imageId)
    {
        using var response = await _client.GetAsync($"{_baseUrl}/exists?id={imageId.Value}");

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<bool>();

        return result;
    }
}
