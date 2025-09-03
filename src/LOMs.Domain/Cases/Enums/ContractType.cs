using System.ComponentModel.DataAnnotations;

namespace LOMs.Domain.Cases.Enums
{
    public enum ContractType
    {
        [Display(Name = "محدد المدة")]
        FixedTerm = 1,

        [Display(Name = "غير محدد المدة")]
        OpenEnded = 2
    }

}
