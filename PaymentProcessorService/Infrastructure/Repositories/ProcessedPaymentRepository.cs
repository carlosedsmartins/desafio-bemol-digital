using MongoDB.Driver;
using PaymentProcessorService.Application.Interfaces.Repositories;
using PaymentProcessorService.Domain.Entities;

namespace PaymentProcessorService.Infrastructure.Repositories;

public class ProcessedPaymentRepository(IMongoDatabase database) : IProcessedPaymentRepository
{
    private readonly IMongoCollection<ProcessedPayment> _collection =
        database.GetCollection<ProcessedPayment>("processed_payments");

    public async Task AddAsync(ProcessedPayment processedPayment)
    {
        await _collection.InsertOneAsync(processedPayment);
    }

    public async Task<ProcessedPayment?> GetByIdAsync(Guid id)
    {
        return await _collection.Find(processedPayment => processedPayment.Id == id).FirstOrDefaultAsync();
    }
}