using System;
using ProductCatalogService.Endpoints;
using ProductCatalogService.Endpoints.Brands;

namespace ProductCatalogService.Extensions;

public static class MapEndpoints
{
    public static IEndpointRouteBuilder MapProductServiceEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapBrandEndpoints();
        endpoints.MapCategoryEndpoints();
        endpoints.MapProductEndpoints();

        return endpoints;
    }
}
