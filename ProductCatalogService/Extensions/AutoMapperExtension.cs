using System;
using ProductCatalogService.Profiles;

namespace ProductCatalogService.Extensions;

public static class AutoMapperExtension
{
    public static IServiceCollection AddAutoMapperProfiles(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(BrandProfile));
        services.AddAutoMapper(typeof(CategoryProfile));
        services.AddAutoMapper(typeof(ProductProfile));

        return services;
    }
}
