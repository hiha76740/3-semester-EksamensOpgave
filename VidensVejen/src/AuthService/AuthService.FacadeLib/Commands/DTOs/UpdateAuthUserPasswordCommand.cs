namespace AuthService.FacadeLib.Commands.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Password"></param>
/// <param name="OldPassword"></param>
/// <param name="RowVersion"></param>
public record UpdateAuthUserPasswordCommand(Guid Id, string Password, string OldPassword, byte[] RowVersion);
