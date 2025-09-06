namespace LOMs.Application.Common.Interfaces;

public interface IFileService
{
    Task<string> SaveFileAsync(Stream fileStream, string fileName, string contentType);
    Task<Stream?> GetFileAsync(string filePath);
    Task DeleteFileAsync(string filePath);
    bool ValidateFile(string fileName, string contentType, long fileSize);
}