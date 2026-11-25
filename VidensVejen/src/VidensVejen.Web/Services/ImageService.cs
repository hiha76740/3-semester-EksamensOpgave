using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using VidensVejen.ContractsLib.Requests.ImageService;
using VidensVejen.ContractsLib.Responses.ImageService;
using VidensVejen.Web.ServicesInterfaces;

namespace VidensVejen.Web.Services;

/// <summary>
/// 
/// </summary>
/// <param name="httpClient"></param>
public class ImageService(HttpClient httpClient) : IImageService
{
    private readonly string _baseUrl = "/images-api";

    async Task<(HttpStatusCode Status, bool InUse)> IImageService.DeleteAsync(Guid id, DeleteImageRequest deletion)
    {
        using var request = new HttpRequestMessage(HttpMethod.Delete, $"{_baseUrl}/{id}")
        {
            Content = JsonContent.Create(deletion)
        };
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        using var response = await httpClient.SendAsync(request);
        var inUse = response.StatusCode == HttpStatusCode.BadRequest
            && (await response.Content.ReadAsStringAsync()).Trim().Trim('"')
                == "Can not delete image that is still in use";

        return (response.StatusCode, inUse);
    }

    async Task<IReadOnlyList<ImageResponse>> IImageService.GetAllAsync()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}");
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        using var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<IReadOnlyList<ImageResponse>>()
            ?? throw new System.Text.Json.JsonException("Could not deserialize the image list.");
    }

    async Task<ImageIdResponse> IImageService.UploadImage(IBrowserFile imageFile, long maxFileSize)
    {
        using var content = new MultipartFormDataContent();

        var fileContent = new StreamContent(imageFile.OpenReadStream(maxFileSize));
        content.Add(fileContent, "imageFile", imageFile.Name);

        var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}")
        {
            Content = content
        };

        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        using var response = await httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var imageId = await response.Content.ReadFromJsonAsync<ImageIdResponse>()
            ?? throw new InvalidOperationException("Could not deserialize image id");

        return imageId;
    }
}
