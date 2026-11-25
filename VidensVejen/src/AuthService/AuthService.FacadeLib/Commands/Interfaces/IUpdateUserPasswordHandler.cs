using AuthService.FacadeLib.Commands.DTOs;

namespace AuthService.FacadeLib.Commands.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IUpdateUserPasswordHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task HandleAsync(UpdateAuthUserPasswordCommand command);

    /// <summary>Resets the password of an account selected by an authorized administrator.</summary>
    Task ResetAsync(Guid id, string password, byte[] rowVersion);
}
