namespace PaymentProcessorService.Infrastructure.Configuration.Settings;

public class DatabaseSettings
{
    public required string ConnectionString { get; init; }

    public required string DatabaseName { get; init; }
}