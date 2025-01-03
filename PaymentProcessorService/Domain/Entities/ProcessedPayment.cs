using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PaymentProcessorService.Domain.Entities;

public class ProcessedPayment
{
    public ProcessedPayment(Guid id, string payerDocument, decimal amount, string description, string paymentMethod,
        DateTime createdAt)
    {
        EnsureNotNullOrEmpty(payerDocument, nameof(payerDocument));
        EnsureNotNullOrEmpty(paymentMethod, nameof(paymentMethod));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        Id = id;
        PayerDocument = payerDocument;
        Amount = amount;
        Description = description;
        PaymentMethod = paymentMethod;
        CreatedAt = createdAt;
        ProcessedAt = DateTime.UtcNow;
    }

    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; }

    public string PayerDocument { get; set; }

    public decimal Amount { get; set; }

    public string Description { get; set; }

    public string PaymentMethod { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ProcessedAt { get; set; }

    private static void EnsureNotNullOrEmpty(string value, string paramName)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(paramName);
    }
}