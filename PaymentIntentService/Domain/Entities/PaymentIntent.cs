namespace PaymentIntentService.Domain.Entities
{
    public class PaymentIntent(decimal amount)
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public decimal Amount { get; set; } = amount;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
