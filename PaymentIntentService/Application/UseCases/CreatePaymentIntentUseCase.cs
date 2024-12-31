using System.Text.Json;
using PaymentIntentService.Application.Interfaces;
using PaymentIntentService.Domain.Entities;

namespace PaymentIntentService.Application.UseCases
{
    public class CreatePaymentIntentUseCase(IMessageQueueService messageQueueService)
    {
        private readonly IMessageQueueService _messageQueueService = messageQueueService;

        public async Task<PaymentIntent> ExecuteAsync(decimal amount)
        {
            var paymentIntent = new PaymentIntent(amount);

            await _messageQueueService.SendMessageAsync(JsonSerializer.Serialize(paymentIntent));

            return paymentIntent;
        }
    }
}
