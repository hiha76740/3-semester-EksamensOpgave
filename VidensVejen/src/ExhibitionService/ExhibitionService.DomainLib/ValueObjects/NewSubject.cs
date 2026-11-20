namespace ExhibitionService.DomainLib.ValueObjects;

/// <summary>
/// 
/// </summary>
/// <param name="Title"></param>
/// <param name="Description"></param>
/// <param name="Topics"></param>
public record NewSubject(string Title,
        string Description,
        List<NewTopic> Topics);
