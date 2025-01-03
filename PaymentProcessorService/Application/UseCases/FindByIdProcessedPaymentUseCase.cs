using PaymentProcessorService.Application.Interfaces.Repositories;
using PaymentProcessorService.Domain.Entities;

namespace PaymentProcessorService.Application.UseCases;

public class FindByIdProcessedPaymentUseCase(IProcessedPaymentRepository repository)
{
    public async Task<ProcessedPayment> ExecuteAsync(Guid id)
    {
        try
        {
            var processedPayment = await repository.GetByIdAsync(id);
            return processedPayment ??
                   throw new InvalidOperationException(
                       "Payment not found. Please check the provided details and try again.");
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error while processing the PaymentIntent.", ex);
        }
    }
}