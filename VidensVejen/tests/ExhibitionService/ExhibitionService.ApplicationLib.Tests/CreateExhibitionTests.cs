//using ExhibitionService.ApplicationLib.Handlers;
//using ExhibitionService.ApplicationLib.Repositories;
//using ExhibitionService.ApplicationLib.Services;
//using ExhibitionService.DomainLib.Entities;
//using ExhibitionService.DomainLib.ValueObjects;
//using ExhibitionService.FacadeLib.Commands.DTOs;
//using ExhibitionService.FacadeLib.Commands.Interfaces;
//using Microsoft.Extensions.Logging;
//using Moq;
//using VidensVejen.SharedKernelLib.Exceptions;

//namespace ExhibitionService.ApplicationLib.Tests;

//public class CreateExhibitionTests
//{

//    private static CreateExhibitionCommand CreateCommand()
//    {
//        return new CreateExhibitionCommand("Ocean", "Ocean EN", Guid.NewGuid(),
//            [new CreateSubjectCommand("Coast", "Coast EN", "coast", "Description", "Description EN",
//                [new CreateTopicCommand("Seaweed", "Seaweed EN", "Description", "Description EN", "Body", "Body EN", "Label", "Label EN", "Text", "Text EN", "Quote", "Quote EN")])]);
//    }

//    [Theory]
//    [InlineData(true)]
//    [InlineData(false)]
//    public async Task Handle_GivenImageLookup_OnlyCreatesWhenImageExists(bool exists)
//    {
//        // Arrange
//        var command = CreateCommand();
//        var repository = new Mock<IExhibitionRepository>();
//        var images = new Mock<IImageService>();
//        var logger = new Mock<ILogger<CreateExhibitionHandler>>();
//        images.Setup(s => s.ExistsAsync(new ImageId(command.ThemeImageId))).ReturnsAsync(exists);
//        Exhibition? captured = null;
//        repository.Setup(r => r.AddAsync(It.IsAny<Exhibition>())).Callback<Exhibition>(e => captured = e);
//        ICreateExhibitionHandler handler = new CreateExhibitionHandler(repository.Object, images.Object, logger.Object);

//        // Act & Assert
//        if (exists == false)
//        {
//            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));
//            repository.VerifyNoOtherCalls();
//            return;
//        }

//        await handler.HandleAsync(command);
//        Assert.NotNull(captured);
//        Assert.Equal(command.ExhibtionName, captured.Name);
//        Assert.Equal(command.ExhibitionNameEn, captured.NameEn);
//        Assert.Equal(command.ThemeImageId, captured.ThemeImageId.Value);
//        var subject = Assert.Single(captured.Subjects);
//        Assert.Equal("Coast", subject.Title);
//        Assert.Equal("Coast EN", subject.TitleEn);
//        Assert.Equal("Description EN", subject.DescriptionEn);
//        Assert.Equal("coast", subject.Slug);
//        Assert.Equal("Description", subject.Description);
//        var topic = Assert.Single(subject.Topics);
//        Assert.Equal("Seaweed", topic.Title);
//        Assert.Equal("Description", topic.Description);
//        Assert.Equal("Body", topic.Body);
//        Assert.Equal("Label", topic.CalloutLabel);
//        Assert.Equal("Text", topic.CalloutText);
//        Assert.Equal("Quote", topic.Quote);
//        Assert.Equal("Seaweed EN", topic.TitleEn);
//        Assert.Equal("Description EN", topic.DescriptionEn);
//        Assert.Equal("Body EN", topic.BodyEn);
//        Assert.Equal("Label EN", topic.CalloutLabelEn);
//        Assert.Equal("Text EN", topic.CalloutTextEn);
//        Assert.Equal("Quote EN", topic.QuoteEn);
//        repository.Verify(r => r.AddAsync(captured), Times.Once);
//        repository.Verify(r => r.SaveAsync(), Times.Once);
//    }

//    [Fact]
//    public async Task Handle_GivenSaveFailure_Rethrows()
//    {
//        // Arrange
//        var command = CreateCommand();
//        var repository = new Mock<IExhibitionRepository>();
//        var images = new Mock<IImageService>();
//        images.Setup(s => s.ExistsAsync(It.IsAny<ImageId>())).ReturnsAsync(true);
//        var expected = new InvalidOperationException("Save failed");
//        repository.Setup(r => r.SaveAsync()).ThrowsAsync(expected);
//        ICreateExhibitionHandler handler = new CreateExhibitionHandler(
//            repository.Object, images.Object, new Mock<ILogger<CreateExhibitionHandler>>().Object);

//        // Act & Assert
//        var actual = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));
//        Assert.Same(expected, actual);
//        repository.Verify(r => r.AddAsync(It.IsAny<Exhibition>()), Times.Once);
//    }

//    [Fact]
//    public async Task Handle_GivenInvalidEnglishName_ThrowsWithoutWriting()
//    {
//        // Arrange
//        var command = CreateCommand() with { ExhibitionNameEn = "" };
//        var repository = new Mock<IExhibitionRepository>();
//        var images = new Mock<IImageService>();
//        images.Setup(s => s.ExistsAsync(It.IsAny<ImageId>())).ReturnsAsync(true);
//        ICreateExhibitionHandler handler = new CreateExhibitionHandler(
//            repository.Object, images.Object, new Mock<ILogger<CreateExhibitionHandler>>().Object);

//        // Act & Assert
//        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(command));
//        repository.VerifyNoOtherCalls();
//    }

//    [Fact]
//    public async Task Handle_GivenAddFailure_RethrowsWithoutSaving()
//    {
//        // Arrange
//        var command = CreateCommand();
//        var repository = new Mock<IExhibitionRepository>();
//        var images = new Mock<IImageService>();
//        images.Setup(s => s.ExistsAsync(It.IsAny<ImageId>())).ReturnsAsync(true);
//        var expected = new InvalidOperationException("Add failed");
//        repository.Setup(r => r.AddAsync(It.IsAny<Exhibition>())).ThrowsAsync(expected);
//        ICreateExhibitionHandler handler = new CreateExhibitionHandler(
//            repository.Object, images.Object, new Mock<ILogger<CreateExhibitionHandler>>().Object);

//        // Act & Assert
//        var actual = await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));
//        Assert.Same(expected, actual);
//        repository.Verify(r => r.SaveAsync(), Times.Never);
//    }
//}
