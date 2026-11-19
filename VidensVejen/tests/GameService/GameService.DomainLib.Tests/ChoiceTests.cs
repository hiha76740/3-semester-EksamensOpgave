using GameService.DomainLib.ValueObjects;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.DomainLib.Tests;

public class ChoiceTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Constructor_GivenValidText_InitializesChoice(bool isCorrect)
    {
        // Act
        var choice = new Choice("🔧 Reparér stolen", isCorrect);

        // Assert
        Assert.Equal("🔧 Reparér stolen", choice.Text);
        Assert.Equal(isCorrect, choice.IsCorrect);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_GivenBlankText_ThrowsDomainException(string? text)
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => new Choice(text!, true));
    }

    [Fact]
    public void Equals_GivenSameValues_ChoicesAreEqual()
    {
        // Act & Assert
        Assert.Equal(new Choice("Plastik", true), new Choice("Plastik", true));
    }
}
