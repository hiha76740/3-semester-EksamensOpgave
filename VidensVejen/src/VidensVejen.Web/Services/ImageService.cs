using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="imageFile"></param>
    /// <param name="maxFileSize"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
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
