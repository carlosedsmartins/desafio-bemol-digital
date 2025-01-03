using PaymentProcessorService.Application.Interfaces.Queue;

namespace PaymentProcessorService.Infrastructure.BackgroundServices;

public class PaymentIntentListenerService(IPaymentIntentQueueConsumer queueConsumer) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await queueConsumer.StartListeningAsync(stoppingToken);
    }
}