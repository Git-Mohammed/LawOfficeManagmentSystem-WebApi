using LOMs.Contract.Commons.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Requests.Cases
{
    public class CreateContractWithCaseRequest 
    {
        [Required(ErrorMessage = "نوع العقد مطلوب.")]
        public ContractType ContractType { get; set; }

        public DateOnly? IssueDate { get; set; }
        public DateOnly? ExpiryDate { get; set; }

        [Required(ErrorMessage = "قيمة العقد مطلوبة.")]
        [Range(0, double.MaxValue, ErrorMessage = "قيمة العقد يجب أن تكون أكبر من أو تساوي صفر.")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "مقدم العقد مطلوب.")]
        [Range(0, double.MaxValue, ErrorMessage = "مقدم العقد يجب أن يكون أكبر من أو يساوي صفر.")]
        public decimal InitialPayment { get; set; }

        [Required(ErrorMessage = " مرفق العقد مطلوب.")]
        public IFormFile ContractFile { get; set; } = null!;

        public bool IsAssigned { get; set; }

      
    }
}
