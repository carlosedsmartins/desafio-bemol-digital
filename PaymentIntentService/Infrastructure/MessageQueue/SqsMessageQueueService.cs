using Amazon.SQS;
using Microsoft.Extensions.Options;
using PaymentIntentService.Application.Interfaces;
using PaymentIntentService.Infrastructure.Settings;

namespace PaymentIntentService.Infrastructure.MessageQueue
{
    public class SqsMessageQueueService(IAmazonSQS amazonSQS, IOptions<QueueSettings> queueSettings) : IMessageQueueService
    {
        private readonly IAmazonSQS _amazonSQS = amazonSQS;
        private readonly QueueSettings _queueSettings = queueSettings.Value;

        public async Task SendMessageAsync(string message)
        {
            try
            {
                await _amazonSQS.SendMessageAsync(_queueSettings.QueueUrl, message);
            }
            catch (AmazonSQSException ex)
            {
                Console.WriteLine(ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
