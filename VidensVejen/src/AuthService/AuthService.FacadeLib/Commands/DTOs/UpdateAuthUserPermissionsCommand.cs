namespace AuthService.FacadeLib.Commands.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="PermissionsIds"></param>
/// <param name="RowVersion"></param>
public record UpdateAuthUserPermissionsCommand(
    Guid Id,
    List<Guid> PermissionsIds,
    byte[] RowVersion);
