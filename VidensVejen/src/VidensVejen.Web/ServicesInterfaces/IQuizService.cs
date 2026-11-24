using System.Net;
using VidensVejen.ContractsLib.Requests.GameService;

namespace VidensVejen.Web.ServicesInterfaces;

/// <summary>
/// 
/// </summary>
public interface IQuizService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="quiz"></param>
    /// <returns></returns>
    Task<HttpStatusCode> CreateAsync(CreateQuizRequest quiz);
}
