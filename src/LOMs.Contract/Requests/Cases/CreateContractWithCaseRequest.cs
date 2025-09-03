using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Requests.Cases
{
    public class CreateContractWithCaseRequest : IValidatableObject
    {
        [Required(ErrorMessage = "نوع العقد مطلوب.")]
        public int ContractType { get; set; }

        public DateOnly? IssueDate { get; set; }
        public DateOnly? ExpiryDate { get; set; }

        [Required(ErrorMessage = "قيمة العقد مطلوبة.")]
        [Range(0, double.MaxValue, ErrorMessage = "قيمة العقد يجب أن تكون أكبر من أو تساوي صفر.")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "مقدم العقد مطلوب.")]
        [Range(0, double.MaxValue, ErrorMessage = "مقدم العقد يجب أن يكون أكبر من أو يساوي صفر.")]
        public decimal InitialPayment { get; set; }

        [Required(ErrorMessage = "مسار ملف العقد مطلوب.")]
        public string ContractFilePath { get; set; } = string.Empty;

        public bool IsAssigned { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ContractType == 1) 
            {
                if (IssueDate == null)
                    yield return new ValidationResult("تاريخ إصدار العقد مطلوب لهذا النوع من العقود.", new[] { nameof(IssueDate) });

                if (ExpiryDate == null)
                    yield return new ValidationResult("تاريخ انتهاء العقد مطلوب لهذا النوع من العقود.", new[] { nameof(ExpiryDate) });
            }
        }
    }
}
