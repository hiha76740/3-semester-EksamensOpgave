using AuthService.FacadeLib.Commands.DTOs;

namespace AuthService.FacadeLib.Commands.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IDeleteUserHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task HandleAsync(DeleteAuthUserCommand command);
}
