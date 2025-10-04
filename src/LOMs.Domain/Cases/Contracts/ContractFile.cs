using LOMs.Domain.Common;
using LOMs.Domain.Common.Files;
using LOMs.Domain.Common.Results;

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

        public static Result<ContractFile> Create(Guid contractId, string fileName, string filePath,FileType fileType, 
            long size, string extension, string contentType)
        {
            var result = Validator(fileName, filePath, fileType, size, extension, contentType);
            if (result.IsError)
                return result.Errors;
            
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
