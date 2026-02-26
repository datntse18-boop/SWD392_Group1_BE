using Backend_CycleTrust.BLL.DTOs.BrandDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Backend_CycleTrust.DAL.Entities;
using Backend_CycleTrust.DAL.Interfaces;

namespace Backend_CycleTrust.BLL.Services
{
    public class BrandService : IBrandService
    {
        private readonly IGenericRepository<Brand> _repository;

        public BrandService(IGenericRepository<Brand> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<BrandResponseDto>> GetAllAsync()
        {
            var brands = await _repository.GetAllAsync();
            return brands.Select(b => new BrandResponseDto
            {
                BrandId = b.BrandId,
                BrandName = b.BrandName
            });
        }

        public async Task<BrandResponseDto?> GetByIdAsync(int id)
        {
            var brand = await _repository.GetByIdAsync(id);
            if (brand == null) return null;
            return new BrandResponseDto
            {
                BrandId = brand.BrandId,
                BrandName = brand.BrandName
            };
        }

        public async Task<BrandResponseDto> CreateAsync(CreateBrandDto dto)
        {
            var brand = new Brand { BrandName = dto.BrandName };
            await _repository.AddAsync(brand);
            return new BrandResponseDto
            {
                BrandId = brand.BrandId,
                BrandName = brand.BrandName
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateBrandDto dto)
        {
            var brand = await _repository.GetByIdAsync(id);
            if (brand == null) return false;
            brand.BrandName = dto.BrandName;
            await _repository.UpdateAsync(brand);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var brand = await _repository.GetByIdAsync(id);
            if (brand == null) return false;
            await _repository.DeleteAsync(brand);
            return true;
        }
    }
}
