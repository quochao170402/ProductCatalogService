using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using CloudinaryDotNet;
using ProductCatalogService.Extensions.Options;
using ProductCatalogService.Services;
using ProductCatalogService.Services.AWSServices;

namespace ProductCatalogService.Extensions;

public static class ServiceExtensions
{

    public static IServiceCollection AddStorageService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCloudinary(configuration)
            .AddScoped<IStorageService, CloudinaryService>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IBrandService, BrandService>();
        services.AddScoped<IProductService, ProductService>();
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
