using GameService.DomainLib.ValueObjects;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.DomainLib.Tests;

public class ChoiceTests
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Constructor_GivenValidText_SetsValues(bool isCorrect)
    {
        // Arrange
        const string Text = "Answer";

        // Act
        var choice = new Choice(Text, isCorrect);

        // Assert
        Assert.Equal(Text, choice.Text);
        Assert.Equal(isCorrect, choice.IsCorrect);
        Assert.Equal(new Choice(Text, isCorrect), choice);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_GivenBlankText_ThrowsDomainException(string? text)
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => new Choice(text!, false));
    }
}
