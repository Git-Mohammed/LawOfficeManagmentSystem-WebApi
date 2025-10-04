using LOMs.Domain.Common.Files;
using LOMs.Domain.Common.Results;
using LOMs.Domain.POAs;

namespace LOMs.Domain.Cases.POAs
{
    public sealed class POAFile : DbFile
    {
       public Guid POAId { get; }

         public POA? POA { get; set; }

        private POAFile()
        {

        }

        private POAFile(Guid id, string filename, string filepath, FileType fileType, long size,
            string extension, string contentType) : base(id, filename, filepath, fileType, size, extension, contentType)
        {
        }

        public static Result<POAFile> Create(string fileName, string filePath, FileType fileType,
            long size, string extension, string contentType)
        {
            var result = Validator(fileName, filePath, fileType, size, extension, contentType);
            if (result.IsError)
                return result.Errors;
            return new POAFile(
                Guid.NewGuid(),
                fileName,
                filePath,
                fileType,
                size,
                extension,
                contentType
            );
        }
    }
}
