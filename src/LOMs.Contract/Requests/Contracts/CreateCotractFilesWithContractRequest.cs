using Microsoft.AspNetCore.Http;

namespace LOMs.Contract.Requests.Contracts
{
    public class CreateCotractFilesWithContractRequest
    {
        public string FileName { get; set; } = string.Empty;
        public IFormFile File { get; set; } = null!;
    }
}
