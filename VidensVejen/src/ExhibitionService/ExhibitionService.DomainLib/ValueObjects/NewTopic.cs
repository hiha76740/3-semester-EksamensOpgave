namespace ExhibitionService.DomainLib.ValueObjects;

/// <summary>
/// 
/// </summary>
/// <param name="Title"></param>
/// <param name="TitleEn"></param>
/// <param name="Description"></param>
/// <param name="DescriptionEn"></param>
/// <param name="Body"></param>
/// <param name="BodyEn"></param>
/// <param name="CalloutLabel"></param>
/// <param name="CalloutLabelEn"></param>
/// <param name="CalloutText"></param>
/// <param name="CalloutTextEn"></param>
/// <param name="Qoute"></param>
/// <param name="QouteEn"></param>

public record NewTopic(
    string Title,
    string TitleEn,
    string Description,
    string DescriptionEn,
    string Body,
    string BodyEn,
    string CalloutLabel,
    string CalloutLabelEn,
    string CalloutText,
    string CalloutTextEn,
    string? Qoute,
    string? QouteEn
    );
