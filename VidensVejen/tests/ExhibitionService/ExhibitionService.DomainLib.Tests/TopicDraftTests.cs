using ExhibitionService.DomainLib.Entities;
using ExhibitionService.DomainLib.ValueObjects;

namespace ExhibitionService.DomainLib.Tests;

public class TopicDraftTests
{
    [Fact]
    public void Create_GivenMultipleTopics_PreservesValuesAndAssignsPositions()
    {
        // Arrange
        var draft = ExhibitionDraft.Create();

        // Act
        draft.UpdateExhibitionDraft(null, null, null,
            [new NewSubjectDraft(null, null, null, null, null, null, null, false,
                [new NewTopicDraft("First", "First EN", null, null, null, null, null, null, null, null, "Quote", "Quote EN"),
                 new NewTopicDraft(null, null, null, null, null, null, null, null, null, null, null, null)])]);
        var topics = Assert.Single(draft.SubjectDrafts).TopicDrafts;

        // Assert
        Assert.Equal(2, topics.Count);
        Assert.Equal("First", topics[0].Title);
        Assert.Equal("First EN", topics[0].TitleEn);
        Assert.Equal("Quote", topics[0].Quote);
        Assert.Equal("Quote EN", topics[0].QuoteEn);
        Assert.Equal(0, topics[0].Position);
        Assert.Equal(1, topics[1].Position);
        Assert.Null(topics[1].Title);
    }
}
