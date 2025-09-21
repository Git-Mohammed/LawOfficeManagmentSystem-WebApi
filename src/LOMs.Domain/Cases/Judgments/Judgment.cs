using LOMs.Domain.Common;
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

    public static Result<Judgment> Create(Guid caseId, string number, JudgmentType type, JudgmentSubType subType,
        DateOnly issuedOn, DateOnly expiresOn)
    {
        if (caseId == Guid.Empty)
            return JudgmentErrors.CaseIdRequired;

        if (string.IsNullOrWhiteSpace(number))
            return JudgmentErrors.NumberRequired;

        if (type == JudgmentType.None)
            return JudgmentErrors.TypeRequired;

        if (subType == JudgmentSubType.None)
            return JudgmentErrors.SubTypeRequired;

        if (issuedOn == default)
            return JudgmentErrors.IssuedOnRequired;

        if (expiresOn == default)
            return JudgmentErrors.ExpiresOnRequired;

        if (expiresOn <= issuedOn)
            return JudgmentErrors.InvalidDateRange;

        
        return new Judgment(Guid.NewGuid(), caseId, number, type, subType, issuedOn, expiresOn);
    }
}

public enum JudgmentSubType
{
    None,
    Vip,
    Normal
}

public enum JudgmentType
{
    None,
    JudgmentOne,
    JudgmentTwo,
    JudgmentThree,
    JudgmentFour,
}