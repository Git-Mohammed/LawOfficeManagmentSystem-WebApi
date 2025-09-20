using LOMs.Domain.Common;
using LOMs.Domain.Common.Files;

namespace LOMs.Domain.Cases.Contracts
{
    public sealed class ContractFile: DbFile
    {
        public Guid ContractId { get; }
        
        public Contract? Contract { get; set; }

        private ContractFile()
        {
        }

        private ContractFile(Guid id, Guid contractId, string filename, string filepath, FileType fileType, long size,
            string extension, string contentType) : base(id, filename, filepath, fileType, size, extension, contentType)
        {
            ContractId = contractId;            
        }

        public static ContractFile Create(Guid contractId, string fileName, string filePath,FileType fileType, 
            long size, string extension, string contentType)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("File name cannot be null or whitespace.", nameof(fileName));

            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or whitespace.", nameof(filePath));

            return new ContractFile(
                Guid.NewGuid(), 
                contractId, 
                fileName, 
                filePath, 
                fileType,
                size, 
                extension, 
                contentType
            );
        }
    }
}
