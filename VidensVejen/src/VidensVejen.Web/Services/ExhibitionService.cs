using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using VidensVejen.ContractsLib.Requests.ExhibitionService.Drafts;
using VidensVejen.ContractsLib.Requests.ExhibitionService.Exhibitions;
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

    private const string DraftBaseUrl = "/exhibitions-drafts-api";

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

    async Task<IReadOnlyList<ExhibitionDraftResponse>> IExhibitionService.GetDraftsAsync()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, DraftBaseUrl);
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        using var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<IReadOnlyList<ExhibitionDraftResponse>>()
            ?? throw new JsonException("The exhibition drafts response was empty.");
    }

    Task IExhibitionService.DeleteDraftAsync(Guid draftId)
    {
        return DeleteAsync($"{DraftBaseUrl}/{draftId:D}");
    }

    Task IExhibitionService.DeletePublishedAsync(Guid exhibitionId)
    {
        return DeleteAsync($"{BaseUrl}/{exhibitionId:D}");
    }

    private async Task DeleteAsync(string url)
    {
        using var request = new HttpRequestMessage(HttpMethod.Delete, url);
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        using var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    async Task<HttpStatusCode> IExhibitionService.CreateAsync(CreateExhibitionRequest exhibition)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl)
        {
            Content = JsonContent.Create(exhibition)
        };
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        using var response = await httpClient.SendAsync(request);

        return response.StatusCode;
    }

    Task<Guid> IExhibitionService.CreateDraftAsync()
    {
        return CreateDraftAsync(DraftBaseUrl);
    }

    async Task<ExhibitionResponse> IExhibitionService.GetPublishedAsync(Guid exhibitionId)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{BaseUrl}/{exhibitionId:D}");
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        using var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<ExhibitionResponse>()
            ?? throw new JsonException("The published exhibition response was empty.");
    }

    Task<Guid> IExhibitionService.CreateRevisionDraftAsync(Guid exhibitionId, UpdateExhibitionDraftRequest draft)
    {
        return CreateDraftAsync($"{DraftBaseUrl}/from-exhibition/{exhibitionId:D}", draft);
    }

    async Task IExhibitionService.PublishRevisionAsync(Guid draftId, CreateExhibitionRequest exhibition)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"{DraftBaseUrl}/{draftId:D}/publish-revision")
        {
            Content = JsonContent.Create(exhibition)
        };
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        using var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    private async Task<Guid> CreateDraftAsync(string url, UpdateExhibitionDraftRequest? draft = null)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        if (draft != null)
        {
            request.Content = JsonContent.Create(draft);
        }

        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        using var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var id = await response.Content.ReadFromJsonAsync<Guid>();
        if (id == Guid.Empty)
        {
            throw new JsonException("The draft identifier was empty.");
        }

        return id;
    }

    async Task<ExhibitionDraftResponse> IExhibitionService.GetDraftAsync(Guid draftId)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, $"{DraftBaseUrl}/{draftId:D}");
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        using var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<ExhibitionDraftResponse>()
            ?? throw new JsonException("The exhibition draft response was empty.");
    }

    async Task IExhibitionService.SaveDraftAsync(Guid draftId, UpdateExhibitionDraftRequest draft)
    {
        using var request = new HttpRequestMessage(HttpMethod.Put, $"{DraftBaseUrl}/{draftId}")
        {
            Content = JsonContent.Create(draft)
        };
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        using var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}
