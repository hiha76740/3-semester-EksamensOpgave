using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using VidensVejen.ContractsLib.Requests.GameService;
using VidensVejen.Web.ServicesInterfaces;

namespace VidensVejen.Web.Services;

/// <summary>
/// 
/// </summary>
public class QuizService(HttpClient httpClient) : IQuizService
{
    private const string BaseUrl = "/quizzes-api";

    async Task<HttpStatusCode> IQuizService.CreateAsync(CreateQuizRequest quiz)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl)
        {
            Content = JsonContent.Create(quiz)
        };
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        using var response = await httpClient.SendAsync(request);

        return response.StatusCode;
    }
}
