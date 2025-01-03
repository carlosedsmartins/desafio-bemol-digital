using PaymentProcessorService.Domain.Entities;

namespace PaymentProcessorService.Application.Interfaces.Repositories;

public interface IProcessedPaymentRepository
{
    Task AddAsync(ProcessedPayment processedPayment);

    Task<ProcessedPayment?> GetByIdAsync(Guid id);
}