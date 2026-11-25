namespace VidensVejen.ContractsLib.Requests.ExhibitionService.Drafts;

/// <summary>Contains a complete exhibition draft, including fields that are still empty.</summary>
public sealed record SaveExhibitionDraftRequest(
    string? ExhibitionName,
    string? ExhibitionNameEn,
    Guid? ThemeImageId,
    List<SaveSubjectDraftRequest>? Subjects);
