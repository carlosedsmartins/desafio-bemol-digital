namespace PaymentIntentService.Domain.Entities;

public class PaymentIntent
{
    public PaymentIntent(string payerDocument, decimal amount, string description, string paymentMethod)
    {
        EnsureNotNullOrEmpty(payerDocument, nameof(payerDocument));
        EnsureNotNullOrEmpty(paymentMethod, nameof(paymentMethod));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        PayerDocument = payerDocument;
        Amount = amount;
        Description = description;
        PaymentMethod = paymentMethod;
    }

    public Guid Id { get; } = Guid.NewGuid();

    public string PayerDocument { get; }

    public decimal Amount { get; }

    public string Description { get; }

    public string PaymentMethod { get; }

    public DateTime CreatedAt { get; } = DateTime.UtcNow;

    private static void EnsureNotNullOrEmpty(string value, string paramName)
    {
        if (string.IsNullOrEmpty(value)) throw new ArgumentNullException(paramName);
    }
}