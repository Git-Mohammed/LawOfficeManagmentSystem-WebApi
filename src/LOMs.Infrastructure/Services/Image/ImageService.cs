using LOMs.Application.Common.Interfaces;
using LOMs.Domain.Common.Enums;
using Microsoft.AspNetCore.Hosting;

namespace LOMs.Infrastructure.Services.Image
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ImageService(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
        }

        private string GetFullPath(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                throw new ArgumentException("Relative path cannot be null or empty.", nameof(relativePath));

            if (string.IsNullOrWhiteSpace(_hostingEnvironment.WebRootPath))
                throw new InvalidOperationException("WebRootPath is not configured. Please ensure UseWebRoot(\"wwwroot\") is set in Program.cs.");

            return Path.Combine(_hostingEnvironment.WebRootPath, relativePath.TrimStart('/', '\\'));
        }

        private string GetRelativePath(string folder, string fileName)
        {
            if (string.IsNullOrWhiteSpace(folder))
                throw new ArgumentException("Folder cannot be null or empty.", nameof(folder));

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name cannot be null or empty.", nameof(fileName));

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);

            // نجعل المسار ثابت ويبدأ بالمجلد
            return Path.Combine(folder, uniqueFileName).Replace("\\", "/");
        }

        public async Task<string> SaveImageAsync(Stream fileStream, string fileName, ImageFolder folder)
        {
            if (fileStream == null || fileStream.Length == 0)
                throw new ArgumentException("File stream is invalid.", nameof(fileStream));

            var relativePath = GetRelativePath(folder.ToString(), fileName);
            var fullPath = GetFullPath(relativePath);

            var directory = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await using (var fileStreamOutput = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                await fileStream.CopyToAsync(fileStreamOutput);
            }

            return relativePath;
        }

        public void DeleteImage(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath)) return;

            var fullPath = GetFullPath(relativePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public async Task<string> ReplaceImageAsync(Stream fileStream, string fileName, string oldRelativePath, ImageFolder folder)
        {
            DeleteImage(oldRelativePath);
            return await SaveImageAsync(fileStream, fileName, folder);
        }

        public Stream GetImageStream(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                throw new FileNotFoundException("Image path is not valid.");

            var fullPath = GetFullPath(relativePath);
            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Image not found at path: {fullPath}");

            return new FileStream(fullPath, FileMode.Open, FileAccess.Read);
        }
    }
}
