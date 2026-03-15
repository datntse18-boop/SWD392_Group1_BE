using Backend_CycleTrust.BLL.DTOs.PaymentDTOs;
using Backend_CycleTrust.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend_CycleTrust.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [Authorize(Roles = "BUYER")]
        [HttpPost("confirm")]
        public async Task<ActionResult<PaymentResponseDto>> ConfirmPayment(PaymentSuccessRequestDto dto)
        {
            try
            {
                var payment = await _paymentService.ProcessSuccessAsync(dto);
                return Ok(payment);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "BUYER")]
        [HttpPost("success")]
        public async Task<ActionResult<PaymentResponseDto>> ConfirmPaymentSuccess(PaymentSuccessRequestDto dto)
        {
            return await ConfirmPayment(dto);
        }
    }
}