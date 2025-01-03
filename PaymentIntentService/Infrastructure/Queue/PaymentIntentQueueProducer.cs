using System.Text.Json;
using Amazon.SQS;
using Microsoft.Extensions.Options;
using PaymentIntentService.Application.DTOs;
using PaymentIntentService.Application.Interfaces.Queue;
using PaymentIntentService.Domain.Entities;
using PaymentIntentService.Infrastructure.Configuration.Settings;

namespace PaymentIntentService.Infrastructure.Queue;

public class PaymentIntentQueueProducer(
    IAmazonSQS amazonSqs,
    IOptions<QueueSettings> queueSettings)
    : IPaymentIntentQueueProducer
{
    private readonly QueueSettings _queueSettings = queueSettings.Value;

    public async Task SendMessageAsync(PaymentIntent paymentIntent)
    {
        try
        {
            var paymentIntentQueueDto = new PaymentIntentQueueDto
            {
                Id = paymentIntent.Id,
                PayerDocument = paymentIntent.PayerDocument,
                Amount = paymentIntent.Amount,
                Description = paymentIntent.Description,
                PaymentMethod = paymentIntent.PaymentMethod,
                CreatedAt = paymentIntent.CreatedAt
            };

            var messageBody = JsonSerializer.Serialize(paymentIntentQueueDto);

            await amazonSqs.SendMessageAsync(_queueSettings.QueueUrl, messageBody);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error while sending the PaymentIntent message to the queue.", ex);
        }
    }
}