using LOMs.Domain.Common;
using LOMs.Domain.Common.Extensions;
using LOMs.Domain.Common.Files;
using LOMs.Domain.Common.Results;

namespace LOMs.Domain.Cases.Judgments;

public class Judgment : AuditableEntity
{
    public Guid CaseId { get; private set; }
    public string Number { get; private set; } = null!;
    public JudgmentType Type { get; private set; }
    public JudgmentSubType SubType { get; private set; }
    public DateOnly IssuedOn { get; private set; }
    public DateOnly ExpiresOn { get; private set; }

    #region Navigation Properties
    public Case Case { get; private set; }
    public ICollection<JudgmentFile> Files { get; private set; } = new List<JudgmentFile>();
    #endregion
    
    private Judgment(){}

    private Judgment(Guid id ,Guid caseId, string number, JudgmentType type, JudgmentSubType subType, DateOnly issuedOn,
        DateOnly expiresOn): base(id)
    {
        Number = number;
        Type = type;
        IssuedOn = issuedOn;
        ExpiresOn = expiresOn;
        CaseId = caseId;
        SubType = subType;
    }

    public static Result<Judgment> Create(Guid caseId, string number, string type, string subType,
        DateOnly issuedOn, DateOnly expiresOn)
    {
        if (caseId == Guid.Empty)
            return JudgmentErrors.CaseIdRequired;

        if (string.IsNullOrWhiteSpace(number))
            return JudgmentErrors.NumberRequired;

        var judgmentType = type.ToEnumOrDefault(JudgmentType.None);
        
        if (judgmentType == JudgmentType.None)
            return JudgmentErrors.TypeRequired;
        var subtype = subType.ToEnumOrDefault(JudgmentSubType.None); 
        if (subtype == JudgmentSubType.None)
            return JudgmentErrors.SubTypeRequired;

        if (issuedOn == default)
            return JudgmentErrors.IssuedOnRequired;

        if (expiresOn == default)
            return JudgmentErrors.ExpiresOnRequired;

        if (expiresOn <= issuedOn)
            return JudgmentErrors.InvalidDateRange;

        
        return new Judgment(Guid.NewGuid(), caseId, number, judgmentType, subtype, issuedOn, expiresOn);
    }
}

