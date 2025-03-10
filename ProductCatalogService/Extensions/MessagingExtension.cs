using MassTransit;
using Microsoft.Extensions.Options;
using ProductCatalogService.Events;
using ProductCatalogService.Extensions.Options;

namespace ProductCatalogService.Extensions;

public static class MessagingExtension
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KafkaOption>(configuration.GetSection("Messaging"));

        services.AddMassTransit(cfg =>
        {
            cfg.UsingInMemory();

            cfg.AddRider(rider =>
            {
                rider.AddProducer<StockUpdatedEvent>("stock-update");

                rider.UsingKafka((context, configurator) =>
                {
                    var kafkaOptions = context.GetRequiredService<IOptions<KafkaOption>>().Value;

                    configurator.Host(kafkaOptions.Host);
                });
            });
        });

        return services;
    }
}
