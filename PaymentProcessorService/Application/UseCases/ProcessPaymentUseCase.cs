using PaymentProcessorService.Application.Interfaces.Repositories;
using PaymentProcessorService.Domain.Entities;

namespace PaymentProcessorService.Application.UseCases;

public class ProcessPaymentUseCase(IProcessedPaymentRepository repository)
{
    public async Task ExecuteAsync(
        Guid id,
        string payerDocument,
        decimal amount,
        string description,
        string paymentMethod,
        DateTime createdAt)
    {
        try
        {
            var processedPayment = new ProcessedPayment(
                id,
                payerDocument,
                amount,
                description,
                paymentMethod,
                createdAt
            );

            await repository.AddAsync(processedPayment);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error while processing the PaymentIntent.", ex);
        }
    }
}