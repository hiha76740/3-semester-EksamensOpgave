namespace GameService.DomainLib.ValueObjects;

/// <summary>
/// 
/// </summary>
/// <param name="CorrectCount"></param>
/// <param name="TotalCount"></param>
public record SessionScore(int CorrectCount, int TotalCount)
{
    /// <summary>
    /// 
    /// </summary>
    public int Percentage => TotalCount == 0 ? 0 : (int)Math.Round(100.0 * CorrectCount / TotalCount, MidpointRounding.AwayFromZero);
}
