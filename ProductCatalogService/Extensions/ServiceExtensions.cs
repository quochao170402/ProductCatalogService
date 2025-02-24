using CloudinaryDotNet;
using ProductCatalogService.Extensions.Options;
using ProductCatalogService.Services;

namespace ProductCatalogService.Extensions;

public static class ServiceExtensions
{

    public static IServiceCollection AddStorageService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCloudinary(configuration)
            .AddScoped<IStorageService, CloudinaryService>();

        return services;
    }


    private static IServiceCollection AddCloudinary(this IServiceCollection services, IConfiguration configuration)
    {
        var cloudinarySetting = configuration.GetSection("Cloudinary").Get<CloudinaryOption>();

        var account = new Account
        {
            Cloud = cloudinarySetting!.CloudName,
            ApiKey = cloudinarySetting.ApiKey,
            ApiSecret = cloudinarySetting.ApiSecret
        };

        ICloudinary cloudinary = new Cloudinary(account);

        services.AddSingleton(cloudinary);
        return services;
    }
}
