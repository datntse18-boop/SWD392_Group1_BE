using Backend_CycleTrust.BLL.DTOs.PaymentDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Backend_CycleTrust.DAL.Data;
using Backend_CycleTrust.DAL.Entities;
using Backend_CycleTrust.DAL.Enums;
using Microsoft.EntityFrameworkCore;

namespace Backend_CycleTrust.BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly CycleTrustDbContext _context;

        public PaymentService(CycleTrustDbContext context)
        {
            _context = context;
        }

        public async Task<PaymentResponseDto> ProcessSuccessAsync(PaymentSuccessRequestDto dto)
        {
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == dto.OrderId);

            if (order == null)
                throw new InvalidOperationException("Order not found.");

            if (order.Status == OrderStatus.CANCELLED)
                throw new InvalidOperationException("Cancelled order cannot be paid.");

            if (order.Status == OrderStatus.COMPLETED)
                throw new InvalidOperationException("Completed order cannot be paid.");

            if (order.Status == OrderStatus.DEPOSITED)
            {
                return new PaymentResponseDto
                {
                    PaymentId = 0,
                    OrderId = order.OrderId,
                    Amount = order.DepositAmount ?? Math.Round(order.TotalAmount * 0.10m, 2, MidpointRounding.AwayFromZero),
                    PaymentMethod = "VietQR",
                    Status = PaymentStatus.SUCCESS.ToString(),
                    CreatedAt = DateTime.UtcNow
                };
            }

            var depositAmount = Math.Round(order.TotalAmount * 0.10m, 2, MidpointRounding.AwayFromZero);
            order.DepositAmount = depositAmount;
            order.Status = OrderStatus.DEPOSITED;

            await _context.SaveChangesAsync();

            var payment = new Payment
            {
                OrderId = order.OrderId,
                Amount = depositAmount,
                PaymentMethod = "VietQR",
                Status = PaymentStatus.SUCCESS,
                CreatedAt = DateTime.UtcNow
            };

            try
            {
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return new PaymentResponseDto
                {
                    PaymentId = 0,
                    OrderId = order.OrderId,
                    Amount = depositAmount,
                    PaymentMethod = "VietQR",
                    Status = PaymentStatus.SUCCESS.ToString(),
                    CreatedAt = DateTime.UtcNow
                };
            }

            return new PaymentResponseDto
            {
                PaymentId = payment.PaymentId,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                Status = payment.Status.ToString(),
                CreatedAt = payment.CreatedAt
            };
        }
    }
}