using LOMs.Application.Common.Interfaces;

namespace LOMs.Infrastructure.Services.FileServices;

public class LocalFileService(string rootPath, IFileValidator validator) : IFileService
{
    private readonly string _rootPath = rootPath;
    private readonly IFileValidator _validator = validator;

    public bool ValidateFile(string fileName, string contentType, long fileSize)
        => _validator.Validate(fileName, contentType, fileSize);

    public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string contentType)
    {
        var folder = Path.Combine(_rootPath, DateTime.UtcNow.ToString("yyyyMMdd"));
        Directory.CreateDirectory(folder);

        var filePath = Path.Combine(folder, $"{Guid.NewGuid()}_{fileName}");

        using (var fileStreamOut = new FileStream(filePath, FileMode.Create))
        {
            await fileStream.CopyToAsync(fileStreamOut);
        }

        return filePath;
    }

    public async Task<Stream?> GetFileAsync(string filePath)
    {
        if (!File.Exists(filePath))
            return null;

        return await Task.FromResult(new FileStream(filePath, FileMode.Open, FileAccess.Read));
    }

    public Task DeleteFileAsync(string filePath)
    {
        if (File.Exists(filePath))
            File.Delete(filePath);

        return Task.CompletedTask;
    }
}