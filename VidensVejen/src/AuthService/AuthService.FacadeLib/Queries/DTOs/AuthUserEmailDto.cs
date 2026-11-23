namespace AuthService.FacadeLib.Queries.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Email"></param>
/// <param name="RowVersion"></param>
public record AuthUserEmailDto(string Email, byte[] RowVersion);
