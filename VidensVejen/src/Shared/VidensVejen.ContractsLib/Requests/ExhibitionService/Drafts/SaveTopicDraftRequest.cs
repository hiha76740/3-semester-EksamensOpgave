namespace VidensVejen.ContractsLib.Requests.ExhibitionService.Drafts;

/// <summary>Contains the editable Danish and English text of a draft topic.</summary>
public sealed record SaveTopicDraftRequest(
    string? Title, string? TitleEn, string? Description, string? DescriptionEn,
    string? Body, string? BodyEn, string? CalloutLabel, string? CalloutLabelEn,
    string? CalloutText, string? CalloutTextEn, string? Quote, string? QuoteEn);
