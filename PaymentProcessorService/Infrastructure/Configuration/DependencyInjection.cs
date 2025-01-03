using Amazon.SQS;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using PaymentProcessorService.Application.Interfaces.Queue;
using PaymentProcessorService.Application.Interfaces.Repositories;
using PaymentProcessorService.Application.UseCases;
using PaymentProcessorService.Infrastructure.BackgroundServices;
using PaymentProcessorService.Infrastructure.Configuration.Settings;
using PaymentProcessorService.Infrastructure.Queue;
using PaymentProcessorService.Infrastructure.Repositories;

namespace PaymentProcessorService.Infrastructure.Configuration;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<QueueSettings>(configuration.GetSection("QueueSettings"));
        services.Configure<DatabaseSettings>(configuration.GetSection("DatabaseSettings"));

        services.AddGrpc();

        services.AddDefaultAWSOptions(configuration.GetAWSOptions());
        services.AddAWSService<IAmazonSQS>();

        services.AddSingleton<IMongoClient>(sp =>
        {
            var databaseSettings = sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            return new MongoClient(databaseSettings.ConnectionString);
        });

        services.AddScoped<IMongoDatabase>(sp =>
        {
            var databaseSettings = sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(databaseSettings.DatabaseName);
        });

        services.AddScoped<IProcessedPaymentRepository, ProcessedPaymentRepository>();

        services.AddScoped<ProcessPaymentUseCase>();
        services.AddScoped<FindByIdProcessedPaymentUseCase>();

        services.AddSingleton<IPaymentIntentQueueConsumer, PaymentIntentQueueConsumer>();

        services.AddHostedService<PaymentIntentListenerService>();
    }
}