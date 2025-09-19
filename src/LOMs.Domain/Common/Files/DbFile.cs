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
    
}