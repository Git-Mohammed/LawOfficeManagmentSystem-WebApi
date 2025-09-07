using System.ComponentModel.DataAnnotations;

namespace LOMs.Domain.Cases.Enums
{
    public enum PartyRole
    {
        [Display(Name = "مدعي")]         // Plaintiff: initiates the legal action
        Plaintiff = 1,

        [Display(Name = "مدعى عليه")]    // Defendant: responds to the legal claim
        Defendant = 2
    }
}
