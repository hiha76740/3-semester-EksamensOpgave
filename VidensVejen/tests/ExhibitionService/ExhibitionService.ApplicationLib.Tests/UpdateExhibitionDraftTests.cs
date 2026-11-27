using ExhibitionService.ApplicationLib.Handlers;
using ExhibitionService.ApplicationLib.Repositories;
using ExhibitionService.DomainLib.Entities;
using ExhibitionService.DomainLib.ValueObjects;
using ExhibitionService.FacadeLib.Commands.DTOs;
using ExhibitionService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using VidensVejen.SharedKernelLib.Exceptions;

namespace ExhibitionService.ApplicationLib.Tests;


public class UpdateExhibitionDraftTests
{
    [Fact]
    public async Task Handle_GivenContent_MapsFieldsAndPreservesRevisionLink()
    {
        // Arrange
        var source = new ExhibitionId(Guid.NewGuid());
        var draft = ExhibitionDraft.CreateRevision(source, [4, 5, 6]);
        var command = ExhibitionTestData.DraftContent(draft.Id.Value);
        var drafts = new Mock<IExhibitionDraftRepository>();
        drafts.Setup(r => r.GetExhibitionDraftByIdAsync(draft.Id)).ReturnsAsync(draft);
        IUpdateExhibitionDraftHandler handler = new UpdateExhibitionDraftHandler(drafts.Object, NullLogger<UpdateExhibitionDraftHandler>.Instance);

        // Act
        var version = await handler.HandleAsync(command);

        // Assert
        Assert.Equal(draft.RowVersion, version);
        Assert.Equal(new byte[] { 4, 5, 6 }, draft.SourceRowVersion);
        Assert.Equal(source, draft.SourceExhibitionId);
        Assert.Equal(command.ExhibtionName, draft.Name);
        Assert.Equal(command.ExhibitionNameEn, draft.NameEn);
        Assert.Equal(command.ThemeImageId, draft.ThemeImageId?.Value);
        var subject = Assert.Single(draft.SubjectDrafts);
        Assert.Equal("Quiz", subject.Title);
        Assert.Equal("Quiz EN", subject.TitleEn);
        Assert.Equal("quiz", subject.Slug);
        Assert.Equal("Description", subject.Description);
        Assert.Equal("Description EN", subject.DescriptionEn);
        Assert.Equal(command.SubjectDrafts[0].QuizId, subject.QuizId?.Value);
        Assert.Equal(command.SubjectDrafts[0].QuizDraftId, subject.QuizDraftId?.Value);
        Assert.True(subject.IsQuiz);
        Assert.Equal(0, subject.Position);
        var topic = Assert.Single(subject.TopicDrafts);
        Assert.Equal("Topic", topic.Title);
        Assert.Equal("Topic EN", topic.TitleEn);
        Assert.Equal("Body", topic.Body);
        Assert.Equal("Body EN", topic.BodyEn);
        Assert.Equal("Label", topic.CalloutLabel);
        Assert.Equal("Label EN", topic.CalloutLabelEn);
        Assert.Equal("Text", topic.CalloutText);
        Assert.Equal("Text EN", topic.CalloutTextEn);
        Assert.Equal("Quote", topic.Quote);
        Assert.Equal("Quote EN", topic.QuoteEn);
        Assert.Equal(0, topic.Position);
        drafts.Verify(r => r.SetOriginalRowVersion(draft, command.RowVersion), Times.Once);
        drafts.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenEmptyFields_ClearsExistingContent()
    {
        // Arrange
        var draft = ExhibitionDraft.Create();
        draft.UpdateExhibitionDraft("Old", "Old EN", new ImageId(Guid.NewGuid()),
            [new NewSubjectDraft("Old", null, null, null, null, null, null, false, [])]);
        var drafts = new Mock<IExhibitionDraftRepository>();
        drafts.Setup(r => r.GetExhibitionDraftByIdAsync(draft.Id)).ReturnsAsync(draft);
        IUpdateExhibitionDraftHandler handler = new UpdateExhibitionDraftHandler(drafts.Object, NullLogger<UpdateExhibitionDraftHandler>.Instance);
        var command = new UpdateExhibitionDraftCommand(draft.Id.Value, null, null, null, [], [1, 2, 3]);

        // Act
        var version = await handler.HandleAsync(command);

        // Assert
        Assert.Equal(draft.RowVersion, version);
        Assert.Null(draft.Name);
        Assert.Null(draft.NameEn);
        Assert.Null(draft.ThemeImageId);
        Assert.Empty(draft.SubjectDrafts);
        drafts.Verify(r => r.SetOriginalRowVersion(draft, command.RowVersion), Times.Once);
        drafts.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenUnknownDraft_ThrowsWithoutSaving()
    {
        // Arrange
        var drafts = new Mock<IExhibitionDraftRepository>();
        IUpdateExhibitionDraftHandler handler = new UpdateExhibitionDraftHandler(drafts.Object, NullLogger<UpdateExhibitionDraftHandler>.Instance);

        // Act
        await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(ExhibitionTestData.DraftContent(Guid.NewGuid())));

        // Assert
        drafts.Verify(r => r.SetOriginalRowVersion(It.IsAny<ExhibitionDraft>(), It.IsAny<byte[]>()), Times.Never);
        drafts.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenConcurrencyConflict_RethrowsWithoutReplacingException()
    {
        // Arrange
        var draft = ExhibitionDraft.Create();
        var command = ExhibitionTestData.DraftContent(draft.Id.Value);
        var repository = new Mock<IExhibitionDraftRepository>();
        repository.Setup(r => r.GetExhibitionDraftByIdAsync(draft.Id)).ReturnsAsync(draft);
        var expected = new ConcurrencyConflictException("Changed by another request", new InvalidOperationException());
        repository.Setup(r => r.SaveAsync()).ThrowsAsync(expected);
        IUpdateExhibitionDraftHandler handler = new UpdateExhibitionDraftHandler(repository.Object, NullLogger<UpdateExhibitionDraftHandler>.Instance);

        // Act
        var actual = await Assert.ThrowsAsync<ConcurrencyConflictException>(() => handler.HandleAsync(command));

        // Assert
        Assert.Same(expected, actual);
        repository.Verify(r => r.SetOriginalRowVersion(draft, command.RowVersion), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenRowVersionFailure_DoesNotSave()
    {
        // Arrange
        var draft = ExhibitionDraft.Create();
        var command = ExhibitionTestData.DraftContent(draft.Id.Value);
        var repository = new Mock<IExhibitionDraftRepository>();
        repository.Setup(r => r.GetExhibitionDraftByIdAsync(draft.Id)).ReturnsAsync(draft);
        var expected = new InvalidOperationException("RowVersion failed");
        repository.Setup(r => r.SetOriginalRowVersion(draft, command.RowVersion)).Throws(expected);
        IUpdateExhibitionDraftHandler handler = new UpdateExhibitionDraftHandler(repository.Object, NullLogger<UpdateExhibitionDraftHandler>.Instance);

        // Act
        var actual = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));

        // Assert
        Assert.Same(expected, actual);
        repository.Verify(r => r.SaveAsync(), Times.Never);
    }
}
