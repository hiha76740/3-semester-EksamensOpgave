using GameService.DomainLib.ValueObjects;

namespace GameService.DomainLib.Tests;

public class SessionScoreTests
{
    [Theory]
    [InlineData(0, 0, 0)]
    [InlineData(5, 8, 63)]
    [InlineData(1, 3, 33)]
    [InlineData(0, 4, 0)]
    [InlineData(4, 4, 100)]
    public void Percentage_GivenScore_RoundsToWholeNumber(int correctCount, int totalCount, int expected)
    {
        // Act
        var score = new SessionScore(correctCount, totalCount);

        // Assert
        Assert.Equal(expected, score.Percentage);
    }
}
