using LOMs.Domain.Common.Results;

namespace LOMs.Domain.Common.Files;

public static class DbFileErrors
{
    public static readonly Error FilenameRequired =
        Error.Validation("DbFile.FilenameRequired", "Filename is required.");

    public static readonly Error InvalidFilename =
        Error.Validation("DbFile.InvalidFilename", "Filename is invalid or contains unsupported characters.");

    public static readonly Error FilepathRequired =
        Error.Validation("DbFile.FilepathRequired", "Filepath is required.");

    public static readonly Error InvalidFilepath =
        Error.Validation("DbFile.InvalidFilepath", "Filepath is invalid or inaccessible.");

    public static readonly Error FileTypeRequired =
        Error.Validation("DbFile.FileTypeRequired", "File type is required.");

    public static readonly Error InvalidFileType =
        Error.Validation("DbFile.InvalidFileType", "File type is invalid.");

    public static readonly Error SizeRequired =
        Error.Validation("DbFile.SizeRequired", "File size is required.");

    public static readonly Error InvalidSize =
        Error.Validation("DbFile.InvalidSize", "File size must be greater than zero and not exceed the allowed limit.");

    public static readonly Error ExtensionRequired =
        Error.Validation("DbFile.ExtensionRequired", "File extension is required.");

    public static readonly Error InvalidExtension =
        Error.Validation("DbFile.InvalidExtension", "File extension is not supported.");

    public static readonly Error ContentTypeRequired =
        Error.Validation("DbFile.ContentTypeRequired", "Content type is required.");

    public static readonly Error InvalidContentType =
        Error.Validation("DbFile.InvalidContentType", "Content type is not supported.");

    public static readonly Error FileAlreadyExists =
        Error.Conflict("DbFile.FileAlreadyExists", "A file with the same name or path already exists.");
}