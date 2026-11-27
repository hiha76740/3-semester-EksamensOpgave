namespace ExhibitionService.FacadeLib.Queries.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="RowVersion">The version of the published exhibition.</param>
public record ExhibitionNameDto(Guid Id, string Name, byte[] RowVersion);
