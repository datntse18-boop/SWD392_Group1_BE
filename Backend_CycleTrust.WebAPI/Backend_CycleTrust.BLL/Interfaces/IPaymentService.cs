using Backend_CycleTrust.BLL.DTOs.PaymentDTOs;

namespace Backend_CycleTrust.BLL.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto> ProcessSuccessAsync(PaymentSuccessRequestDto dto);
    }
}