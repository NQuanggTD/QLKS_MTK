using Microsoft.AspNetCore.Mvc;
using QLKS.Services;

namespace QLKS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("summary/{bookingId}")]
        public IActionResult GetPaymentSummary(string bookingId)
        {
            try
            {
                var summary = _paymentService.GeneratePaymentSummary(bookingId);
                return Ok(summary);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
