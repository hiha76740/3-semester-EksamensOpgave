using System.Net.Http.Json;
using Dapr.Client;
using GameService.ApplicationLib.Services;
using GameService.DomainLib.Entities.Quizzes;

namespace GameService.InfrastructureLib.Services;

/// <summary>
/// 
/// </summary>
/// <param name="daprClient"></param>
public class ExhibitionService(DaprClient daprClient) : IExhibitionService
{
    private readonly HttpClient _client = daprClient.CreateInvokableHttpClient("exhibitionservice-api");

    private readonly string _baseUrl = "/v1/exhibitions";

    async Task<bool> IExhibitionService.QuizInUseAsync(QuizId quizId)
    {
        using var response = await _client.GetAsync($"{_baseUrl}/quiz/inuse?id={quizId.Value}");

        response.EnsureSuccessStatusCode();

        var exists = await response.Content.ReadFromJsonAsync<bool>();

        return exists;
    }
}
