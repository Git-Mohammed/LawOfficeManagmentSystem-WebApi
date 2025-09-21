using System.Text.RegularExpressions;
using LOMs.Domain.Common.Results;

namespace LOMs.Domain.Common.Files;

public abstract class DbFile : AuditableEntity
{
    public string Filename { get; private set; }
    public string Filepath { get; private set; }
    public long Size { get; private set; }
    public string Extension { get; private set; }
    public string ContentType { get; private set; }
    public FileType FileType { get; private set; }

    protected DbFile(){}

    protected DbFile(Guid id, string filename, string filepath, FileType fileType,long size, string extension, string contentType)
        : base(id)
    {
        Filename = filename;
        Filepath = filepath;
        FileType = fileType;
        Size = size;
        Extension = extension;
        ContentType = contentType;
    }

    protected static Result<bool> Validator(string filename, string filepath, FileType fileType, long size, string extension,
        string contentType)
    {
        if (string.IsNullOrWhiteSpace(filename))
            return DbFileErrors.FilenameRequired;

        if (!Regex.IsMatch(filename, @"^[\w\-. ]+$"))
            return DbFileErrors.InvalidFilename;

        if (string.IsNullOrWhiteSpace(filepath))
            return DbFileErrors.FilepathRequired;

        if (!Uri.IsWellFormedUriString(filepath, UriKind.RelativeOrAbsolute))
            return DbFileErrors.InvalidFilepath;

        if (fileType == default)
            return DbFileErrors.FileTypeRequired;

        if (size <= 0)
            return DbFileErrors.InvalidSize;

        // Example: limit size to 100 MB
        const long maxSize = 100 * 1024 * 1024;
        if (size > maxSize)
            return DbFileErrors.InvalidSize;

        if (string.IsNullOrWhiteSpace(extension))
            return DbFileErrors.ExtensionRequired;

        if (!Regex.IsMatch(extension, @"^\.[a-zA-Z0-9]+$"))
            return DbFileErrors.InvalidExtension;

        if (string.IsNullOrWhiteSpace(contentType))
            return DbFileErrors.ContentTypeRequired;

        if (!Regex.IsMatch(contentType, @"^[\w\-]+\/[\w\-.+]+$"))
            return DbFileErrors.InvalidContentType;

        return true;
    }
    
}