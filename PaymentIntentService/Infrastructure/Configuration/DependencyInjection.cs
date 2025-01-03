using Amazon.SQS;
using PaymentIntentService.Application.Interfaces.Queue;
using PaymentIntentService.Application.UseCases;
using PaymentIntentService.Infrastructure.Configuration.Settings;
using PaymentIntentService.Infrastructure.Queue;

namespace PaymentIntentService.Infrastructure.Configuration;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<QueueSettings>(configuration.GetSection("QueueSettings"));
        services.Configure<GrpcSettings>(configuration.GetSection("GrpcSettings"));

        services.AddDefaultAWSOptions(configuration.GetAWSOptions());
        services.AddAWSService<IAmazonSQS>();

        services.AddScoped<IPaymentIntentQueueProducer, PaymentIntentQueueProducer>();
        services.AddScoped<CreatePaymentIntentUseCase>();
    }
}