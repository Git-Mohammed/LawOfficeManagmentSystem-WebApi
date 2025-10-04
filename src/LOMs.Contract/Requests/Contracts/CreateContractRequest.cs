using LOMs.Contract.Commons.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace LOMs.Contract.Requests.Contracts
{
    public class CreateContractRequest
    {
        [Required(ErrorMessage = "Contract type is required.")]
        public ContractType ContractType { get; set; }

        public DateOnly? IssuedOn { get; set; }

        public DateOnly? ExpiresOn { get; set; }

        [Required(ErrorMessage = "Total amount is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Total amount must be greater than or equal to 0.")]
        public decimal BaseAmount { get; set; }

        [Required(ErrorMessage = "Initial payment is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Initial payment must be greater than or equal to 0.")]
        public decimal InitialPayment { get; set; }

        [Required(ErrorMessage = "File  is required.")]
        public IFormFile ContractFile { get; set; }

        public bool IsAssigned { get; set; }
    }
}
