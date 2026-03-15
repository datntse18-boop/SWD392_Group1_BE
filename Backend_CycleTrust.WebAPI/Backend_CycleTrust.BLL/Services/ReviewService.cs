using Backend_CycleTrust.BLL.DTOs.ReviewDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Backend_CycleTrust.DAL.Data;
using Backend_CycleTrust.DAL.Entities;
using Backend_CycleTrust.DAL.Enums;
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
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == dto.OrderId);

            if (order == null)
                throw new InvalidOperationException("Order not found.");

            if (order.BuyerId != dto.BuyerId)
                throw new InvalidOperationException("Buyer does not match this order.");

            if (order.SellerId != dto.SellerId)
                throw new InvalidOperationException("Seller does not match this order.");

            if (order.Status != OrderStatus.RECEIVED && order.Status != OrderStatus.COMPLETED)
                throw new InvalidOperationException("Review is only allowed after order completion.");

            var alreadyReviewed = await _context.Reviews.AnyAsync(r =>
                r.OrderId == dto.OrderId && r.BuyerId == dto.BuyerId);

            if (alreadyReviewed)
                throw new InvalidOperationException("You have already reviewed this order.");

            if (dto.Rating.HasValue && (dto.Rating < 1 || dto.Rating > 5))
                throw new InvalidOperationException("Rating must be between 1 and 5.");

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

        public async Task<bool> UpdateAsync(int id, UpdateReviewDto dto, int actorUserId, bool isAdmin = false)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return false;

            if (!isAdmin && review.BuyerId != actorUserId)
                throw new InvalidOperationException("You can only edit your own review.");

            if (dto.Rating.HasValue && (dto.Rating < 1 || dto.Rating > 5))
                throw new InvalidOperationException("Rating must be between 1 and 5.");

            review.Rating = dto.Rating;
            review.Comment = dto.Comment;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id, int actorUserId, bool isAdmin = false)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null) return false;

            if (!isAdmin && review.BuyerId != actorUserId)
                throw new InvalidOperationException("You can only delete your own review.");

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
