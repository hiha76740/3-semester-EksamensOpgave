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
}
