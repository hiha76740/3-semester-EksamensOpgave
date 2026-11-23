namespace AuthService.FacadeLib.Commands.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Email"></param>
/// <param name="RowVersion"></param>
public record UpdateAuthUserEmailCommand(
    Guid Id,
    string Email,
    byte[] RowVersion);
