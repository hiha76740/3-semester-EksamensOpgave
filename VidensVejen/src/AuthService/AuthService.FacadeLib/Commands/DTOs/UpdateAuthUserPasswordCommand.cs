namespace AuthService.FacadeLib.Commands.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Password"></param>
/// <param name="OldPassword"></param>
public record UpdateAuthUserPasswordCommand(Guid Id, string Password, string OldPassword);
