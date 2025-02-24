using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProductCatalogService.Extensions.Options;

namespace ProductCatalogService.Extensions;

public static class MongoExtension
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        // Bind MongoDB options from configuration
        services.Configure<MongoOption>(configuration.GetSection("MongoDB"));

        // Register IMongoClient as a singleton (connection pooling is handled internally)
        services.AddSingleton<IMongoClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<MongoOption>>().Value;
            return new MongoClient(options.ConnectionString);
        });

        // Register IMongoDatabase as scoped
        services.AddScoped<IMongoDatabase>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            var options = sp.GetRequiredService<IOptions<MongoOption>>().Value;
            if (string.IsNullOrEmpty(options.DatabaseName))
                throw new ArgumentException("MongoDB DatabaseName cannot be null or empty.");
            return client.GetDatabase(options.DatabaseName);
        });

        return services;
    }
}
