using ImageService.ApplicationLib.Storages;

namespace ImageService.InfrastructureLib.Storages;

/// <summary>
/// 
/// </summary>
/// <param name="imageDirectory"></param>
public class LocalImageStorage(string imageDirectory) : IImageStorage
{
    Task IImageStorage.DeleteAsync(string storageKey, string extension)
    {
        var fileName = $"{storageKey}{extension}";

        if (string.IsNullOrWhiteSpace(fileName)
        || fileName == "."
        || fileName == ".."
        || fileName != Path.GetFileName(fileName))
        {
            throw new ArgumentException("The value must be a file name.");
        }

        var filePath = Path.Combine(imageDirectory, fileName);

        File.Delete(filePath);

        return Task.CompletedTask;
    }

    async Task IImageStorage.SaveAsync(string storageKey, Stream content, string extension, CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(imageDirectory);

        var fileName = $"{storageKey}{extension}";

        var filePath = Path.Combine(imageDirectory, fileName);

        await using var fileStream = new FileStream(
            filePath,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.None,
            bufferSize: 81920,
            useAsync: true
            );

        try
        {
            await content.CopyToAsync(fileStream, cancellationToken);
        }
        catch (Exception)
        {
            await fileStream.DisposeAsync();
            File.Delete(filePath);
            throw;
        }
    }
}
