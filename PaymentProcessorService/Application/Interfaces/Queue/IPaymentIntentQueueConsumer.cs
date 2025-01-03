namespace PaymentProcessorService.Application.Interfaces.Queue;

public interface IPaymentIntentQueueConsumer
{
    Task StartListeningAsync(CancellationToken stoppingToken);
}