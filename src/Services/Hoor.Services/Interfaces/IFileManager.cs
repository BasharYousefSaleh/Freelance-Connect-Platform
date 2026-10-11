namespace Hoor.Services.Interfaces
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public interface IFileManager
    {
        Task AddFileToContractAsync(IFormFile file, string contractId);

        Task<string> UploadAttachmentAsync(IFormFile attachment);

        Task<string> CarduploadAttachmentAsync(IFormFile attachment, string nameImage);

        Task<string> profileuploadAttachmentAsync(IFormFile attachment, string nameImage);

    }
}
