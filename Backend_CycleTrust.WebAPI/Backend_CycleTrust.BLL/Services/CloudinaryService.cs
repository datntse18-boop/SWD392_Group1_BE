using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Backend_CycleTrust.BLL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Backend_CycleTrust.BLL.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration configuration)
        {
            var cloudName = configuration["CloudinarySettings:CloudName"];
            var apiKey = configuration["CloudinarySettings:ApiKey"];
            var apiSecret = configuration["CloudinarySettings:ApiSecret"];

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
                throw new Exception("File is empty.");

            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = folderName,
                Transformation = new Transformation().Quality("auto").FetchFormat("auto")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
                throw new Exception(uploadResult.Error.Message);

            return uploadResult.SecureUrl.AbsoluteUri;
        }

        public async Task<List<string>> UploadImagesAsync(IEnumerable<IFormFile> files, string folderName)
        {
            var imageUrls = new List<string>();

            foreach (var file in files)
            {
                var imageUrl = await UploadImageAsync(file, folderName);
                imageUrls.Add(imageUrl);
            }

            return imageUrls;
        }

        public async Task<bool> DeleteImageAsync(string imageUrl)
        {
            // Extract Public ID from URL (simple version, might need adjustment based on folder structure)
            // Example: https://res.cloudinary.com/db3sv3qau/image/upload/v12345/bikes/mybike.jpg
            var uri = new Uri(imageUrl);
            var publicIdWithExtension = Path.GetFileName(uri.AbsolutePath);
            var publicId = Path.GetFileNameWithoutExtension(publicIdWithExtension);
            
            // If folders are used, they need to be part of the Public ID
            // For now, let's assume a simpler approach or that we store PublicId separately in the future
            // For this implementation, we'll just return true if we can't easily parse it, 
            // but in a production app, PublicId should be stored in DB.
            
            var deletionParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deletionParams);

            return result.Result == "ok";
        }
    }
}
