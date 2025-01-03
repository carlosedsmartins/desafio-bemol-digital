using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Payment;
using PaymentIntentService.Application.DTOs;
using PaymentIntentService.Application.UseCases;
using PaymentIntentService.Infrastructure.Configuration.Settings;

namespace PaymentIntentService.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class PaymentIntentController(
    CreatePaymentIntentUseCase createPaymentIntentUseCase,
    IOptions<GrpcSettings> grpcSettings
  ) : ControllerBase
{
    private readonly GrpcSettings _grpcSettings = grpcSettings.Value;

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePaymentIntentDto paymentIntentCreateDto)
    {
        var result = await createPaymentIntentUseCase.ExecuteAsync(
            paymentIntentCreateDto.PayerDocument,
            paymentIntentCreateDto.Amount,
            paymentIntentCreateDto.Description ?? string.Empty,
            paymentIntentCreateDto.PaymentMethod
        );

        return Ok(new { PaymentIntentId = result.Id });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetStatus(Guid id)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_grpcSettings.PaymentServiceUrl);
            var client = new PaymentService.PaymentServiceClient(channel);

            var request = new PaymentStatusRequest { Uuid = id.ToString() };
            var response = await client.GetPaymentStatusAsync(request);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = ex.Message });
        }
    }
}