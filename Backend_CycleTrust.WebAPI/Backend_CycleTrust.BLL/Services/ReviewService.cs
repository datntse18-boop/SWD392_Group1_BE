using Backend_CycleTrust.BLL.DTOs.ReviewDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Backend_CycleTrust.DAL.Data;
using Backend_CycleTrust.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend_CycleTrust.BLL.Services
{
    public class ReviewService : IReviewService
    {
        private readonly CycleTrustDbContext _context;

        public ReviewService(CycleTrustDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ReviewResponseDto>> GetAllAsync()
        {
            return await _context.Reviews
                .Include(r => r.Buyer)
                .Include(r => r.Seller)
                .Select(r => new ReviewResponseDto
                {
                    ReviewId = r.ReviewId,
                    OrderId = r.OrderId,
                    BuyerId = r.BuyerId,
                    BuyerName = r.Buyer.FullName,
                    SellerId = r.SellerId,
                    SellerName = r.Seller.FullName,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<ReviewResponseDto?> GetByIdAsync(int id)
        {
            var review = await _context.Reviews
                .Include(r => r.Buyer)
                .Include(r => r.Seller)
                .FirstOrDefaultAsync(r => r.ReviewId == id);

            if (review == null) return null;

            return new ReviewResponseDto
            {
                ReviewId = review.ReviewId,
                OrderId = review.OrderId,
                BuyerId = review.BuyerId,
                BuyerName = review.Buyer.FullName,
                SellerId = review.SellerId,
                SellerName = review.Seller.FullName,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };
        }

        public async Task<ReviewResponseDto> CreateAsync(CreateReviewDto dto)
        {
            var review = new Review
            {
                OrderId = dto.OrderId,
                BuyerId = dto.BuyerId,
                SellerId = dto.SellerId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(review.ReviewId) ?? throw new Exception("Failed to create review");
        }

        public async Task<bool> UpdateAsync(int id, UpdateReviewDto dto)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return false;

            review.Rating = dto.Rating;
            review.Comment = dto.Comment;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return false;

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
