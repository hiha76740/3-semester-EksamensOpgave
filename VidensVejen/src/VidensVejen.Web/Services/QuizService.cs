using System.Text.Json;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using VidensVejen.ContractsLib.Requests.GameService.Drafts;
using VidensVejen.ContractsLib.Requests.GameService.Quizzes;
using VidensVejen.Web.ServicesInterfaces;
using VidensVejen.ContractsLib.Responses.GameService;

namespace VidensVejen.Web.Services;

/// <summary>
/// 
/// </summary>
public class QuizService(HttpClient httpClient) : IQuizService
{
    private const string BaseUrl = "/quizzes-api";

    private const string DraftUrl = "/quiz-drafts-api";

    async Task<HttpStatusCode> IQuizService.PublishAsync(Guid draftId, CreateQuizRequest quiz)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseUrl}?quizDraftId={draftId:D}")
        {
            Content = JsonContent.Create(quiz)
        };
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        using var response = await httpClient.SendAsync(request);

        return response.StatusCode;
    }

    async Task<Guid> IQuizService.CreateDraftAsync()
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, DraftUrl);
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        using var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var draft = await response.Content.ReadFromJsonAsync<QuizDraftIdResponse>();
        if (draft == null || draft.QuizDraftId == Guid.Empty)
        {
            throw new JsonException("The quiz draft response did not contain an identifier.");
        }

        return draft.QuizDraftId;
    }

    async Task IQuizService.SaveDraftAsync(Guid draftId, UpdateQuizDraftRequest quiz)
    {
        using var request = new HttpRequestMessage(HttpMethod.Put, $"{DraftUrl}/{draftId:D}")
        {
            Content = JsonContent.Create(quiz)
        };
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        using var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }

    Task<IReadOnlyList<QuizDraftResponse>> IQuizService.GetDraftsAsync()
    {
        return GetAsync<IReadOnlyList<QuizDraftResponse>>(DraftUrl);
    }

    Task<IReadOnlyList<QuizResponse>> IQuizService.GetPublishedAsync()
    {
        return GetAsync<IReadOnlyList<QuizResponse>>($"{BaseUrl}/admin");
    }

    Task<QuizDraftResponse> IQuizService.GetDraftAsync(Guid id)
    {
        return GetAsync<QuizDraftResponse>($"{DraftUrl}/{id:D}");
    }

    Task<QuizResponse> IQuizService.GetPublishedAsync(Guid id)
    {
        return GetAsync<QuizResponse>($"{BaseUrl}/{id:D}");
    }

    Task IQuizService.DeleteDraftAsync(Guid id)
    {
        return DeleteAsync($"{DraftUrl}/{id:D}");
    }

    Task IQuizService.DeletePublishedAsync(Guid id)
    {
        return DeleteAsync($"{BaseUrl}/{id:D}");
    }

    private async Task<T> GetAsync<T>(string url) where T : class
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        using var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<T>()
            ?? throw new JsonException("The quiz response was empty.");
    }

    private async Task DeleteAsync(string url)
    {
        using var request = new HttpRequestMessage(HttpMethod.Delete, url);
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
        using var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
    }
}
