using PaymentIntentService.Domain.Entities;

namespace PaymentIntentService.Application.Interfaces.Queue;

public interface IPaymentIntentQueueProducer
{
    Task SendMessageAsync(PaymentIntent paymentIntent);
}