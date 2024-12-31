using Amazon.SQS;
using PaymentIntentService.Application.Interfaces;
using PaymentIntentService.Application.UseCases;
using PaymentIntentService.Infrastructure.MessageQueue;
using PaymentIntentService.Infrastructure.Settings;

namespace PaymentIntentService.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());
            services.AddAWSService<IAmazonSQS>();

            services.Configure<QueueSettings>(configuration.GetSection("QueueSettings"));

            services.AddScoped<IMessageQueueService, SqsMessageQueueService>();
            services.AddScoped<CreatePaymentIntentUseCase>();

            return services;
        }
    }
}
