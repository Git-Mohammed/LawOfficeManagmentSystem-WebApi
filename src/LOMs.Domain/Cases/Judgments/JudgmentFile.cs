using LOMs.Domain.Common.Files;
using LOMs.Domain.Common.Results;

namespace LOMs.Domain.Cases.Judgments;

public sealed class JudgmentFile : DbFile
{
    public Guid JudgmentId { get; private set; }

    #region Navigation Properties
    public Judgment Judgment { get; private set; }
    #endregion

    private JudgmentFile() { }

    private JudgmentFile(Guid id, Guid judgmentId, string filename, string filepath, FileType fileType, long size,
        string extension, string contentType)
        : base(id, filename, filepath, fileType, size, extension, contentType)
    {
        JudgmentId = judgmentId;
    }

    public static Result<JudgmentFile> Create(Guid judgmentId, string fileName, string filePath, FileType fileType,
        long size, string extension, string contentType)
    {
        var result = Validator(fileName, filePath, fileType, size, extension, contentType);
        if (result.IsError)
            return result.Errors;

        return new JudgmentFile(
            Guid.NewGuid(),
            judgmentId,
            fileName,
            filePath,
            fileType,
            size,
            extension,
            contentType
        );
    }
}