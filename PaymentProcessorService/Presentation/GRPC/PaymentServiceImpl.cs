using Grpc.Core;
using Payment;
using PaymentProcessorService.Application.UseCases;

namespace PaymentProcessorService.Presentation.GRPC;

public class PaymentServiceImpl(FindByIdProcessedPaymentUseCase findByIdUseCase)
    : PaymentService.PaymentServiceBase
{
    public override async Task<PaymentStatusResponse> GetPaymentStatus(
        PaymentStatusRequest request,
        ServerCallContext context)
    {
        try
        {
            var payment = await findByIdUseCase.ExecuteAsync(Guid.Parse(request.Uuid));

            return new PaymentStatusResponse
            {
                Status = "processed",
                Message = "Payment processed successfully",
                PayerDocument = payment.PayerDocument,
                Amount = (double)payment.Amount,
                Description = payment.Description,
                PaymentMethod = payment.PaymentMethod,
                CreatedAt = payment.CreatedAt.ToString("o"),
                ProcessedAt = payment.ProcessedAt.ToString("o")
            };
        }
        catch (InvalidOperationException ex)
        {
            return new PaymentStatusResponse
            {
                Status = "not_found",
                Message = ex.Message
            };
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, "An error occurred while processing the request.",
                ex));
        }
    }
}