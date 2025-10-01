using LOMs.Application.Common.Interfaces;
using LOMs.Application.Features.Contracts.Dtos;
using LOMs.Domain.Common.Results;
using Microsoft.AspNetCore.StaticFiles;

namespace LOMs.Infrastructure.Services.FileServices;

public class LocalFileService(string rootPath, IFileValidator validator) : IFileService
{
    private readonly string _rootPath = rootPath;
    private readonly IFileValidator _validator = validator;

    public bool ValidateFile(string fileName, string contentType, long fileSize)
        => _validator.Validate(fileName, contentType, fileSize);

    private string GetFullPath(string relativePath)
        => Path.Combine(_rootPath, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));

    private string GenerateRelativePath(string folder, string fileName)
    {
        var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
        return Path.Combine(folder, uniqueFileName).Replace("\\", "/");
    }

    public async Task<Result<string>> SaveFileAsync(Stream fileStream, string fileName, string folder)
    {
        try
        {
            var relativePath = GenerateRelativePath(folder, fileName);
            var fullPath = GetFullPath(relativePath);

            var directory = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            await using var output = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
            await fileStream.CopyToAsync(output);

            return relativePath;

        } catch(Exception ex)
        {
            // Log the exception (not implemented here)
            return Error.Unexpected("An error occurred while saving the file.");
        }
    }

    public async Task<Stream?> GetFileAsync(string relativePath)
    {
        var fullPath = GetFullPath(relativePath);
        if (!File.Exists(fullPath)) return null;

        return await Task.FromResult(new FileStream(fullPath, FileMode.Open, FileAccess.Read));
    }

    public Task DeleteFileAsync(string relativePath)
    {
        var fullPath = GetFullPath(relativePath);
        if (File.Exists(fullPath))
            File.Delete(fullPath);

        return Task.CompletedTask;
    }

public async Task<Result<FileDownloadResult>> DownloadFileAsync(string relativePath)
{
    if (string.IsNullOrWhiteSpace(relativePath) || relativePath.Contains(".."))
        return Error.Conflict("Invalid file path.");

    var fullPath = GetFullPath(relativePath);
    if (!File.Exists(fullPath))
        return Error.NotFound("File not found.");

    var fileName = Path.GetFileName(fullPath);
    var contentTypeProvider = new FileExtensionContentTypeProvider();
    var contentType = contentTypeProvider.TryGetContentType(fileName, out var type)
        ? type
        : "application/octet-stream";

    try
    {
        var stream = await GetFileAsync(relativePath);
        if (stream is null)
            return Error.NotFound("File stream could not be opened.");

        var result = new FileDownloadResult(stream, fileName, contentType);
        return result;
    }
    catch (Exception ex)
    {
        // Log the exception if needed
        return Error.Unexpected($"Failed to download file: {ex.Message}");
    }
}

public async Task<string> ReplaceFileAsync(Stream fileStream, string fileName, string oldRelativePath, string folder)
    {
        await DeleteFileAsync(oldRelativePath);
        return (await SaveFileAsync(fileStream, fileName, folder)).Value;
    }
}
