using ExhibitionService.ApplicationLib.Handlers;
using ExhibitionService.ApplicationLib.Repositories;
using ExhibitionService.ApplicationLib.Services;
using ExhibitionService.DomainLib.Entities;
using ExhibitionService.FacadeLib.Commands.DTOs;
using ExhibitionService.FacadeLib.Commands.Interfaces;
using Moq;
using VidensVejen.SharedKernelLib.Exceptions;

namespace ExhibitionService.ApplicationLib.Tests;

public class CreateExhibitionTests
{
    private static CreateExhibitionCommand CreateCommand(
        string exhibitionName = "Livet i havet",
        Stream? imageStream = null,
        string? extension = null)
    {
        var topic = new CreateTopicCommand(
            "Tang",
            "Tang i danske farvande",
            "Tang giver små havdyr et sted at leve.",
            "Vidste du det?",
            "Tang kan vokse på sten.",
            "Der er liv mellem tangbladene.");

        var subject = new CreateSubjectCommand(
            "Kysten",
            "Livet langs den danske kyst",
            [topic]);

        // Exercise the handler's no-image branch even though the DTO declares non-nullable fields.
        return new CreateExhibitionCommand(exhibitionName, imageStream!, extension!, [subject]);
    }

    [Fact]
    public async Task Handle_GivenValidCommandWithImage_MapsExhibitionAndSaves()
    {
        // Arrange
        using var imageStream = new MemoryStream([1, 2, 3]);
        var extension = ".png";
        var command = CreateCommand(imageStream: imageStream, extension: extension);
        var imageId = Guid.NewGuid();
        var imageService = new Mock<IImageService>();
        imageService.Setup(s => s.SaveAsync(imageStream, extension)).ReturnsAsync(imageId);

        var repository = new Mock<IExhibitionRepository>();
        Exhibition? savedExhibition = null;
        repository.Setup(r => r.AddAsync(It.IsAny<Exhibition>()))
            .Callback<Exhibition>(exhibition => savedExhibition = exhibition);

        ICreateExhibitionHandler handler = new CreateExhibitionHandler(repository.Object, imageService.Object);

        // Act
        await handler.HandleAsync(command);

        // Assert
        Assert.NotNull(savedExhibition);
        Assert.NotEqual(Guid.Empty, savedExhibition.Id.Value);
        Assert.Equal(command.ExhibtionName, savedExhibition.Name);
        Assert.Equal(imageId, savedExhibition.Theme?.Value);

        var subject = Assert.Single(savedExhibition.Subjects);
        var subjectCommand = command.Subjects[0];
        Assert.Equal(subjectCommand.Title, subject.Title);
        Assert.Equal(subjectCommand.Description, subject.Description);

        var topic = Assert.Single(subject.Topics);
        var topicCommand = subjectCommand.Topics[0];
        Assert.Equal(topicCommand.Title, topic.Title);
        Assert.Equal(topicCommand.Description, topic.Description);
        Assert.Equal(topicCommand.Body, topic.Body);
        Assert.Equal(topicCommand.CalloutLabel, topic.CalloutLabel);
        Assert.Equal(topicCommand.CalloutText, topic.CalloutText);
        Assert.Equal(topicCommand.Qoute, topic.Quote);

        imageService.Verify(s => s.SaveAsync(imageStream, extension), Times.Once);
        repository.Verify(r => r.AddAsync(savedExhibition), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenValidCommandWithoutImage_SavesWithoutTheme()
    {
        // Arrange
        var command = CreateCommand();
        var repository = new Mock<IExhibitionRepository>();
        var imageService = new Mock<IImageService>();
        ICreateExhibitionHandler handler = new CreateExhibitionHandler(repository.Object, imageService.Object);

        // Act
        await handler.HandleAsync(command);

        // Assert
        imageService.VerifyNoOtherCalls();
        repository.Verify(r => r.AddAsync(It.Is<Exhibition>(exhibition => exhibition.Theme == null)), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_GivenInvalidExhibition_ThrowsWithoutWriting()
    {
        // Arrange
        var command = CreateCommand(exhibitionName: "");
        var repository = new Mock<IExhibitionRepository>();
        var imageService = new Mock<IImageService>();
        ICreateExhibitionHandler handler = new CreateExhibitionHandler(repository.Object, imageService.Object);

        // Act
        var exception = await Record.ExceptionAsync(() => handler.HandleAsync(command));

        // Assert
        Assert.IsType<DomainException>(exception);
        repository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_GivenImageUploadFailure_RethrowsWithoutWriting()
    {
        // Arrange
        using var imageStream = new MemoryStream([1, 2, 3]);
        var extension = ".png";
        var command = CreateCommand(imageStream: imageStream, extension: extension);
        var repository = new Mock<IExhibitionRepository>();
        var imageService = new Mock<IImageService>();
        var expected = new InvalidOperationException("Image upload failed");
        imageService.Setup(s => s.SaveAsync(imageStream, extension)).ThrowsAsync(expected);
        ICreateExhibitionHandler handler = new CreateExhibitionHandler(repository.Object, imageService.Object);

        // Act
        var exception = await Record.ExceptionAsync(() => handler.HandleAsync(command));

        // Assert
        Assert.Same(expected, exception);
        imageService.Verify(s => s.SaveAsync(imageStream, extension), Times.Once);
        repository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_GivenAddFailure_RethrowsOriginalException()
    {
        // Arrange
        var command = CreateCommand();
        var repository = new Mock<IExhibitionRepository>();
        var imageService = new Mock<IImageService>();
        var expected = new InvalidOperationException("Add failed");
        repository.Setup(r => r.AddAsync(It.IsAny<Exhibition>())).ThrowsAsync(expected);
        ICreateExhibitionHandler handler = new CreateExhibitionHandler(repository.Object, imageService.Object);

        // Act
        var exception = await Record.ExceptionAsync(() => handler.HandleAsync(command));

        // Assert
        Assert.Same(expected, exception);
        repository.Verify(r => r.AddAsync(It.IsAny<Exhibition>()), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenSaveFailure_RethrowsOriginalException()
    {
        // Arrange
        var command = CreateCommand();
        var repository = new Mock<IExhibitionRepository>();
        var imageService = new Mock<IImageService>();
        var expected = new InvalidOperationException("Save failed");
        repository.Setup(r => r.SaveAsync()).ThrowsAsync(expected);
        ICreateExhibitionHandler handler = new CreateExhibitionHandler(repository.Object, imageService.Object);

        // Act
        var exception = await Record.ExceptionAsync(() => handler.HandleAsync(command));

        // Assert
        Assert.Same(expected, exception);
        repository.Verify(r => r.AddAsync(It.IsAny<Exhibition>()), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Once);
    }
}
