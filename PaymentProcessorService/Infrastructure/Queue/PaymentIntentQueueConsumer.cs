using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;
using PaymentProcessorService.Application.DTOs;
using PaymentProcessorService.Application.Interfaces.Queue;
using PaymentProcessorService.Application.UseCases;
using PaymentProcessorService.Infrastructure.Configuration.Settings;

namespace PaymentProcessorService.Infrastructure.Queue;

public class PaymentIntentQueueConsumer(
    IAmazonSQS amazonSqs,
    IOptions<QueueSettings> queueSettings,
    IServiceProvider serviceProvider) : IPaymentIntentQueueConsumer
{
    private readonly QueueSettings _queueSettings = queueSettings.Value;

    public async Task StartListeningAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
            try
            {
                var receiveMessageRequest = new ReceiveMessageRequest
                {
                    QueueUrl = _queueSettings.QueueUrl,
                    MaxNumberOfMessages = 10,
                    WaitTimeSeconds = 10
                };

                var response = await amazonSqs.ReceiveMessageAsync(receiveMessageRequest, stoppingToken);

                foreach (var message in response.Messages)
                {
                    var paymentIntentMessage = JsonSerializer.Deserialize<PaymentIntentQueueDto>(message.Body);

                    if (paymentIntentMessage == null) continue;

                    using (var scope = serviceProvider.CreateScope())
                    {
                        var processPaymentUseCase = scope.ServiceProvider.GetRequiredService<ProcessPaymentUseCase>();
                        await processPaymentUseCase.ExecuteAsync(
                            paymentIntentMessage.Id,
                            paymentIntentMessage.PayerDocument,
                            paymentIntentMessage.Amount,
                            paymentIntentMessage.Description ?? string.Empty,
                            paymentIntentMessage.PaymentMethod,
                            paymentIntentMessage.CreatedAt
                        );
                    }

                    var deleteMessageRequest = new DeleteMessageRequest
                    {
                        QueueUrl = _queueSettings.QueueUrl,
                        ReceiptHandle = message.ReceiptHandle
                    };

                    await amazonSqs.DeleteMessageAsync(deleteMessageRequest, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error while receiving the PaymentIntent message to the queue.", ex);
            }
    }
}