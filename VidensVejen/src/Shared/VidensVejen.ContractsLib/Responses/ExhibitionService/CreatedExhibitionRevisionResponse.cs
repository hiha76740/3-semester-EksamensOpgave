namespace VidensVejen.ContractsLib.Responses.ExhibitionService;

/// <summary>Identifies a newly created revision draft and its version.</summary>
/// <param name="Id">The draft identifier.</param>
/// <param name="RowVersion">The saved draft version.</param>
public record CreatedExhibitionRevisionResponse(Guid Id, byte[] RowVersion);
