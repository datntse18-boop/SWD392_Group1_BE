using Backend_CycleTrust.BLL.DTOs.BikeImageDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Backend_CycleTrust.DAL.Entities;
using Backend_CycleTrust.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend_CycleTrust.BLL.Services
{
    public class BikeImageService : IBikeImageService
    {
        private readonly IGenericRepository<BikeImage> _repository;

        public BikeImageService(IGenericRepository<BikeImage> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<BikeImageResponseDto>> GetAllAsync()
        {
            var images = await _repository.GetAllAsync();
            return images.Select(i => new BikeImageResponseDto
            {
                ImageId = i.ImageId,
                BikeId = i.BikeId,
                ImageUrl = i.ImageUrl
            });
        }

        public async Task<BikeImageResponseDto?> GetByIdAsync(int id)
        {
            var image = await _repository.GetByIdAsync(id);
            if (image == null) return null;
            return new BikeImageResponseDto
            {
                ImageId = image.ImageId,
                BikeId = image.BikeId,
                ImageUrl = image.ImageUrl
            };
        }

        public async Task<BikeImageResponseDto> CreateAsync(CreateBikeImageDto dto)
        {
            var image = new BikeImage
            {
                BikeId = dto.BikeId,
                ImageUrl = dto.ImageUrl
            };
            await _repository.AddAsync(image);
            return new BikeImageResponseDto
            {
                ImageId = image.ImageId,
                BikeId = image.BikeId,
                ImageUrl = image.ImageUrl
            };
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var image = await _repository.GetByIdAsync(id);
            if (image == null) return false;
            await _repository.DeleteAsync(image);
            return true;
        }

        public async Task<bool> DeleteByUrlAsync(int bikeId, string imageUrl)
        {
            // _repository is generic, so we can access Context but it's cleaner if we had specific repo.
            // However we can fetch all and filter since images are few per bike, or use raw if exposed.
            var images = await _repository.GetAllAsync();
            var image = images.FirstOrDefault(i => i.BikeId == bikeId && i.ImageUrl == imageUrl);
            
            if (image == null) return false;
            
            await _repository.DeleteAsync(image);
            return true;
        }
    }
}
