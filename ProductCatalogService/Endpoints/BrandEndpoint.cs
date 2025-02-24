using ProductCatalogService.Models;
using ProductCatalogService.Repositories.Common;

namespace ProductCatalogService.Endpoints;

public static class BrandEndpoint
{
    public static IEndpointRouteBuilder MapBrandEndpoints(this IEndpointRouteBuilder endpoints)
    {
        // Group endpoints under "/api/brands" for better organization
        var group = endpoints.MapGroup("/api/brands").WithTags("Brands");

        // GET: Retrieve all brands
        group.MapGet("/", async (IRepository<Brand> repository) =>
        {
            var brands = await repository.GetAsync();
            return Results.Ok(brands);
        })
        .WithName("GetAllBrands")
        .WithOpenApi();

        // GET: Retrieve a brand by ID
        group.MapGet("/{id}", async (string id, IRepository<Brand> repository) =>
            {
                var brand = await repository.GetByIdAsync(id);

            return brand is not null
                ? Results.Ok(brand)
                : Results.NotFound($"Brand with ID {id} not found.");
        })
        .WithName("GetBrandById")
        .WithOpenApi();

        // POST: Create a new brand
        group.MapPost("/", async (Brand brand, IRepository<Brand> repository) =>
        {
            await repository.AddAsync(brand);
            return Results.Created($"/api/brands/{brand.Id}", brand);
        })
        .WithName("CreateBrand")
        .WithOpenApi();

        // PUT: Update an existing brand
        group.MapPut("/{id}", async (string id, Brand updatedBrand, IRepository<Brand> repository) =>
        {
            var existingBrand = await repository.GetByIdAsync(id);
            if (existingBrand is null)
                return Results.NotFound($"Brand with ID {id} not found.");

            updatedBrand.Id = id;
            await repository.UpdateAsync(updatedBrand);
            return Results.NoContent();
        })
        .WithName("UpdateBrand")
        .WithOpenApi();

        // DELETE: Delete a brand by ID
        group.MapDelete("/{id}", async (string id, IRepository<Brand> repository) =>
        {
            var brand = await repository.GetByIdAsync(id);
            if (brand is null)
                return Results.NotFound($"Brand with ID {id} not found.");

            await repository.DeleteAsync(id);
            return Results.NoContent();
        })
        .WithName("DeleteBrand")
        .WithOpenApi();

        return endpoints;
    }
}
