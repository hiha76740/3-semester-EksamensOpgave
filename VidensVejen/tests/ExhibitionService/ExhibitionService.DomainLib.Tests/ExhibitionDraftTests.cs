using ExhibitionService.DomainLib.Entities;
using ExhibitionService.DomainLib.ValueObjects;

namespace ExhibitionService.DomainLib.Tests;

public class ExhibitionDraftTests
{
    [Fact]
    public void Create_CreatesEmptyDraftWithUniqueId()
    {
        // Act
        var draft = ExhibitionDraft.Create();
        var second = ExhibitionDraft.Create();

        // Assert
        Assert.NotEqual(Guid.Empty, draft.Id.Value);
        Assert.NotEqual(draft.Id, second.Id);
        Assert.Null(draft.Name);
        Assert.Null(draft.ThemeImageId);
        Assert.Null(draft.SourceExhibitionId);
        Assert.Empty(draft.SubjectDrafts);
    }

    [Fact]
    public void Update_GivenEmptyFields_ClearsContentAndPreservesRevisionLink()
    {
        // Arrange
        var sourceId = new ExhibitionId(Guid.NewGuid());
        var draft = ExhibitionDraft.CreateRevision(sourceId, []);
        var id = draft.Id;
        draft.UpdateExhibitionDraft("Name", "Name EN", new ImageId(Guid.NewGuid()),
            [new NewSubjectDraft(null, null, null, null, null, null, null, false, [])]);

        // Act
        draft.UpdateExhibitionDraft(null, null, null, null);

        // Assert
        Assert.Equal(id, draft.Id);
        Assert.Equal(sourceId, draft.SourceExhibitionId);
        Assert.Null(draft.Name);
        Assert.Null(draft.NameEn);
        Assert.Null(draft.ThemeImageId);
        Assert.Empty(draft.SubjectDrafts);
    }

    [Fact]
    public void Update_GivenNewSubjects_ReplacesOldSubjectsAndRestartsPositions()
    {
        // Arrange
        var draft = ExhibitionDraft.Create();
        draft.UpdateExhibitionDraft("Old", null, null,
            [new NewSubjectDraft("Old subject", null, null, null, null, null, null, false, [])]);

        // Act
        draft.UpdateExhibitionDraft("New", "New EN", null,
            [new NewSubjectDraft("First", null, null, null, null, null, null, false, []),
             new NewSubjectDraft("Second", null, null, null, null, null, null, false, [])]);

        // Assert
        Assert.Equal("New", draft.Name);
        Assert.Equal("New EN", draft.NameEn);
        Assert.Equal(2, draft.SubjectDrafts.Count);
        Assert.Equal("First", draft.SubjectDrafts[0].Title);
        Assert.Equal(0, draft.SubjectDrafts[0].Position);
        Assert.Equal("Second", draft.SubjectDrafts[1].Title);
        Assert.Equal(1, draft.SubjectDrafts[1].Position);
    }
}
