using Backend_CycleTrust.BLL.DTOs.OrderDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Backend_CycleTrust.DAL.Data;
using Backend_CycleTrust.DAL.Entities;
using Backend_CycleTrust.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace Backend_CycleTrust.BLL.Services
{
    public class OrderService : IOrderService
    {
        private readonly CycleTrustDbContext _context;

        public OrderService(CycleTrustDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Bike)
                .Include(o => o.Buyer)
                .Include(o => o.Seller)
                .Select(o => new OrderResponseDto
                {
                    OrderId = o.OrderId,
                    BikeId = o.BikeId,
                    BikeTitle = o.Bike.Title,
                    BuyerId = o.BuyerId,
                    BuyerName = o.Buyer.FullName,
                    SellerId = o.SellerId,
                    SellerName = o.Seller.FullName,
                    TotalAmount = o.TotalAmount,
                    DepositAmount = o.DepositAmount,
                    Status = o.Status.ToString(),
                    CreatedAt = o.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<OrderResponseDto?> GetByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Bike)
                .Include(o => o.Buyer)
                .Include(o => o.Seller)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) return null;

            return new OrderResponseDto
            {
                OrderId = order.OrderId,
                BikeId = order.BikeId,
                BikeTitle = order.Bike.Title,
                BuyerId = order.BuyerId,
                BuyerName = order.Buyer.FullName,
                SellerId = order.SellerId,
                SellerName = order.Seller.FullName,
                TotalAmount = order.TotalAmount,
                DepositAmount = order.DepositAmount,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt
            };
        }

        public async Task<OrderResponseDto> CreateAsync(CreateOrderDto dto)
        {
            var order = new Order
            {
                BikeId = dto.BikeId,
                BuyerId = dto.BuyerId,
                SellerId = dto.SellerId,
                TotalAmount = dto.TotalAmount,
                DepositAmount = dto.DepositAmount,
                Status = OrderStatus.PENDING,
                CreatedAt = DateTime.UtcNow
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(order.OrderId) ?? throw new Exception("Failed to create order");
        }

        public async Task<bool> UpdateAsync(int id, UpdateOrderDto dto)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            order.DepositAmount = dto.DepositAmount;

            if (Enum.TryParse<OrderStatus>(dto.Status, out var status))
                order.Status = status;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return false;

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
