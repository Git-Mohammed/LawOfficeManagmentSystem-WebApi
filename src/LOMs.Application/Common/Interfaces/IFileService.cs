namespace LOMs.Application.Common.Interfaces;

public interface IFileService
{
    Task<string> SaveFileAsync(Stream fileStream, string fileName, string folder);
    Task<Stream?> GetFileAsync(string relativePath);
    Task DeleteFileAsync(string relativePath);
    Task<string> ReplaceFileAsync(Stream fileStream, string fileName, string oldRelativePath, string folder);
    public bool ValidateFile(string fileName, string contentType, long fileSize);
}