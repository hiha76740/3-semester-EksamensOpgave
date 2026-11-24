using System.Net;
using VidensVejen.ContractsLib.Requests.GameService;
using VidensVejen.Web.ServicesInterfaces;

namespace VidensVejen.Web.Services;

/// <summary>
/// 
/// </summary>
public class QuizService : IQuizService
{
    Task<HttpStatusCode> IQuizService.CreateAsync(CreateQuizRequest request)
    {
        throw new NotImplementedException();
    }
}
