namespace PaymentIntentService.Application.Interfaces
{
    public interface IMessageQueueService
    {
        Task SendMessageAsync(string message);
    }
}
