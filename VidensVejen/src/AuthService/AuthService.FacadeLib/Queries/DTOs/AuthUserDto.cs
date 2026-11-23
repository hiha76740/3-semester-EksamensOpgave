namespace AuthService.FacadeLib.Queries.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Email"></param>
/// <param name="Permissions"></param>
/// <param name="RowVersion"></param>
public record AuthUserDto(
    Guid Id,
    string Email,
    IReadOnlyList<AccessPermissionDto> Permissions,
    byte[] RowVersion
    );
