namespace PaymentProcessorService.Application.DTOs;

public class PaymentIntentQueueDto
{
    public required Guid Id { get; set; }

    public required string PayerDocument { get; set; }

    public required decimal Amount { get; set; }

    public required string? Description { get; set; }

    public required string PaymentMethod { get; set; }

    public required DateTime CreatedAt { get; set; }
}