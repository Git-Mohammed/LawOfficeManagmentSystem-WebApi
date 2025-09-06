using LOMs.Domain.Common.Enums;

namespace LOMs.Application.Common.Interfaces
{
    public interface IImageService
    {
        /// <summary>
        /// Saves an image stream to a specified folder.
        /// </summary>
        /// <param name="fileStream">The stream of the image file.</param>
        /// <param name="fileName">The original name of the file.</param>
        /// <param name="folder">The target folder name (e.g., "products", "users").</param>
        /// <returns>The relative path to the saved image.</returns>
        Task<string> SaveImageAsync(Stream fileStream, string fileName, ImageFolder folder);

        /// <summary>
        /// Deletes an image from the file system.
        /// </summary>
        /// <param name="relativePath">The relative path of the image to delete.</param>
        void DeleteImage(string relativePath);

        /// <summary>
        /// Replaces an existing image with a new one.
        /// </summary>
        /// <param name="fileStream">The stream of the new image.</param>
        /// <param name="fileName">The original name of the new file.</param>
        /// <param name="oldRelativePath">The relative path of the image to replace.</param>
        /// <param name="folder">The target folder name.</param>
        /// <returns>The relative path to the new saved image.</returns>
        Task<string> ReplaceImageAsync(Stream fileStream, string fileName, string oldRelativePath, ImageFolder folder);

        /// <summary>
        /// Retrieves the stream of an image.
        /// </summary>
        /// <param name="relativePath">The relative path of the image to retrieve.</param>
        /// <returns>The stream of the image file.</returns>
        Stream GetImageStream(string relativePath);
    }
}