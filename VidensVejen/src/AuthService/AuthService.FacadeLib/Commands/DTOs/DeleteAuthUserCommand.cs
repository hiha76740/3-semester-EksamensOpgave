namespace AuthService.FacadeLib.Commands.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="RowVersion"></param>
public record DeleteAuthUserCommand(
    Guid Id,
    byte[] RowVersion
    );
