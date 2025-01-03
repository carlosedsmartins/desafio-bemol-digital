using Microsoft.AspNetCore.Mvc;
using PaymentProcessorService.Application.UseCases;

namespace PaymentProcessorService.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class ProcessedPaymentController(FindByIdProcessedPaymentUseCase findByIdProcessedPaymentUseCase)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await findByIdProcessedPaymentUseCase.ExecuteAsync(id);
        return Ok(new { ProcessedPayment = result });
    }
}