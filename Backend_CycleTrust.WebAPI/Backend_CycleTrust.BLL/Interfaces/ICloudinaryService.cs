using Microsoft.AspNetCore.Http;

namespace Backend_CycleTrust.BLL.Interfaces
{
    public interface ICloudinaryService
    {
        Task<string> UploadImageAsync(IFormFile file, string folderName);
        Task<List<string>> UploadImagesAsync(IEnumerable<IFormFile> files, string folderName);
        Task<bool> DeleteImageAsync(string imageUrl);
    }
}
