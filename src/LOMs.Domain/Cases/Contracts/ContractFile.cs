using LOMs.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOMs.Domain.Cases.Contracts
{
    public sealed class ContractFile : AuditableEntity
    {
        public Guid ContractId { get; }
        public string FileName { get; private set; } = string.Empty;
        public string FilePath { get; private set; } = null!;

        public Contract? Contract { get; set; }

        private ContractFile()
        {
        }

        private ContractFile(string fileName, string filePath)
        {
            FileName = fileName;
            FilePath = filePath;
        }

        public static ContractFile Create(string fileName, string filePath)
        {

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("File name cannot be null or whitespace.", nameof(fileName));
            }
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path cannot be null or whitespace.", nameof(filePath));
            }
            return new ContractFile(fileName, filePath);
        }
    }
}
