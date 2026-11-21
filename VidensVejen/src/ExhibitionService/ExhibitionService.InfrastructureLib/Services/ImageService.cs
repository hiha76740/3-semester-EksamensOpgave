using Dapr.Client;
using ExhibitionService.ApplicationLib.Services;
using VidensVejen.ContractsLib.Responses.ImageService;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ExhibitionService.InfrastructureLib.Services;
/// <summary>
/// 
/// </summary>
/// <param name="daprClient"></param>
public class ImageService(DaprClient daprClient) : IImageService
{
    private readonly HttpClient _client = daprClient.CreateInvokableHttpClient("imageservice-api");

    private readonly string _baseUrl = "/v1/images";

    async Task<Guid> IImageService.SaveAsync(Stream content, string extension)
    {
        using var form = new MultipartFormDataContent();

        var file = new StreamContent(content);
        file.Headers.ContentType = new MediaTypeHeaderValue(
            extension == ".png" ? "image/png" : "image/jpeg");

        form.Add(file, "imageFile", $"theme{extension}");

        using var response = await _client.PostAsync(_baseUrl, form);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<ImageIdResponse>()
            ?? throw new InvalidOperationException("Unable to deserialize ImageId");

        return result.ImageId;
    }
}
