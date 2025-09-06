using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Commons.Enums;

public enum PartyRole
{
    [Display(Name = "مدعي")]
    Plaintiff = 1,

    [Display(Name = "مدعى عليه")]
    Defendant = 2
}
