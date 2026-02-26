using Backend_CycleTrust.BLL.DTOs.WishlistDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Backend_CycleTrust.DAL.Data;
using Backend_CycleTrust.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend_CycleTrust.BLL.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly CycleTrustDbContext _context;

        public WishlistService(CycleTrustDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<WishlistResponseDto>> GetAllAsync()
        {
            return await _context.Wishlists
                .Include(w => w.Buyer)
                .Include(w => w.Bike)
                .Select(w => new WishlistResponseDto
                {
                    WishlistId = w.WishlistId,
                    BuyerId = w.BuyerId,
                    BuyerName = w.Buyer.FullName,
                    BikeId = w.BikeId,
                    BikeTitle = w.Bike.Title,
                    CreatedAt = w.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<WishlistResponseDto?> GetByIdAsync(int id)
        {
            var wishlist = await _context.Wishlists
                .Include(w => w.Buyer)
                .Include(w => w.Bike)
                .FirstOrDefaultAsync(w => w.WishlistId == id);

            if (wishlist == null) return null;

            return new WishlistResponseDto
            {
                WishlistId = wishlist.WishlistId,
                BuyerId = wishlist.BuyerId,
                BuyerName = wishlist.Buyer.FullName,
                BikeId = wishlist.BikeId,
                BikeTitle = wishlist.Bike.Title,
                CreatedAt = wishlist.CreatedAt
            };
        }

        public async Task<WishlistResponseDto> CreateAsync(CreateWishlistDto dto)
        {
            var wishlist = new Wishlist
            {
                BuyerId = dto.BuyerId,
                BikeId = dto.BikeId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Wishlists.Add(wishlist);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(wishlist.WishlistId) ?? throw new Exception("Failed to create wishlist item");
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var wishlist = await _context.Wishlists.FindAsync(id);
            if (wishlist == null) return false;

            _context.Wishlists.Remove(wishlist);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
