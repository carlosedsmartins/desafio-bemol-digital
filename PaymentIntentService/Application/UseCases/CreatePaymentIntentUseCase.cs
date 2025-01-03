using PaymentIntentService.Application.Interfaces.Queue;
using PaymentIntentService.Domain.Entities;

namespace PaymentIntentService.Application.UseCases;

public class CreatePaymentIntentUseCase(IPaymentIntentQueueProducer queueProducer)
{
    public async Task<PaymentIntent> ExecuteAsync(string payerDocument, decimal amount, string description,
        string paymentMethod)
    {
        try
        {
            var paymentIntent = new PaymentIntent(payerDocument, amount, description, paymentMethod);

            await queueProducer.SendMessageAsync(paymentIntent);

            return paymentIntent;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error while creating the PaymentIntent.", ex);
        }
    }
}