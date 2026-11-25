using System.Text.Json;

namespace VidensVejen.ContractsLib.Requests.ExhibitionService.Drafts;

/// <summary>Contains the editable content and quiz selection of a subject slot.</summary>
public sealed record SaveSubjectDraftRequest(
    string? Title, string? TitleEn, string? Slug, string? Description, string? DescriptionEn,
    List<SaveTopicDraftRequest>? Topics, bool IsQuiz, Guid? QuizId, JsonElement? NewQuiz);
