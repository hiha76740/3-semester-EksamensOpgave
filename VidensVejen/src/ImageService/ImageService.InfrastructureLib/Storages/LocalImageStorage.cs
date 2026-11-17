using ImageService.ApplicationLib.Storages;

namespace ImageService.InfrastructureLib.Storages;

/// <summary>
/// 
/// </summary>
/// <param name="imageDirectory"></param>
public class LocalImageStorage(string imageDirectory) : IImageStorage
{
    async Task IImageStorage.SaveAsync(string storageKey, Stream content, string extension, CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(imageDirectory);

        var filename = $"{storageKey}{extension}";

        var filePath = Path.Combine(imageDirectory, filename);

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
