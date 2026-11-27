using ExhibitionService.DomainLib.Entities;
using ExhibitionService.DomainLib.ValueObjects;

namespace ExhibitionService.DomainLib.Tests;

public class TopicTests
{
    [Fact]
    public void Create_GivenMultipleTopics_PreservesValuesAndAssignsPositions()
    {
        // Arrange
        List<NewTopic> input =
        [
            new NewTopic("First", "First EN", "Description", "Description EN", "Body", "Body EN", "Label", "Label EN", "Text", "Text EN", "Quote", "Quote EN"),
            new NewTopic("Second", "Second EN", "Description", "Description EN", "Body", "Body EN", "Label", "Label EN", "Text", "Text EN", null, null)
        ];

        // Act
        var exhibition = Exhibition.Create(new ExhibitionId(Guid.NewGuid()), "Name", "Name EN", new ImageId(Guid.NewGuid()),
            [new NewSubject("Subject", "Subject EN", "subject", "Description", "Description EN", null, input)]);
        var topics = Assert.Single(exhibition.Subjects).Topics;

        // Assert
        Assert.Equal(2, topics.Count);
        Assert.Equal("First", topics[0].Title);
        Assert.Equal("First EN", topics[0].TitleEn);
        Assert.Equal("Quote", topics[0].Quote);
        Assert.Equal("Quote EN", topics[0].QuoteEn);
        Assert.Equal(0, topics[0].Position);
        Assert.Equal(1, topics[1].Position);
        Assert.Equal("Second", topics[1].Title);
        Assert.Null(topics[1].Quote);
    }
}
