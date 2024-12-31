using Microsoft.AspNetCore.Mvc;
using PaymentIntentService.Application.DTOs;
using PaymentIntentService.Application.UseCases;

namespace PaymentIntentService.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentIntentController(CreatePaymentIntentUseCase createPaymentIntentUseCase) : ControllerBase
    {
        private readonly CreatePaymentIntentUseCase _createPaymentIntentUseCase = createPaymentIntentUseCase;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PaymentIntentCreateDTO paymentIntentCreateDTO)
        {
            var result = await _createPaymentIntentUseCase.ExecuteAsync(paymentIntentCreateDTO.Amount);
            return Ok(result.Id);
        }
    }
}
