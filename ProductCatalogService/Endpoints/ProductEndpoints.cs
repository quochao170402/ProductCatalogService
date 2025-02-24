using Microsoft.AspNetCore.Mvc;
using ProductCatalogService.Models;
using ProductCatalogService.Repositories.Common;
using ProductCatalogService.Services;

namespace ProductCatalogService.Endpoints;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

public static class ProductEndpoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/products").WithTags("Products");

        // GET: Retrieve all products (non-deleted)
        group.MapGet("/", async (IRepository<Product> repository) =>
        {
            var products = await repository.GetAsync();
            return Results.Ok(products);
        })
        .WithName("GetAllProducts")
        .WithOpenApi();

        // GET: Retrieve a product by ID (non-deleted)
        group.MapGet("/{id}", async (string id, IRepository<Product> repository) =>
        {
            var product = await repository.GetByIdAsync(id);
            return product is not null
                ? Results.Ok(product)
                : Results.NotFound($"Product with ID {id} not found.");
        })
        .WithName("GetProductById")
        .WithOpenApi();

        // POST: Create a new product
        group.MapPost("/", async (Product product, IRepository<Product> repository) =>
        {
            // Assign IDs to nested objects if missing
            AssignIdsToNestedObjects(product);
            var newId = await repository.AddAsync(product);
            return Results.Created($"/api/products/{newId}", product);
        })
        .WithName("CreateProduct")
        .WithOpenApi();

        // PUT: Update an existing product
        group.MapPut("/{id}", async (string id, Product updatedProduct, IRepository<Product> repository) =>
        {
            var existingProduct = await repository.GetByIdAsync(id);
            if (existingProduct is null)
                return Results.NotFound($"Product with ID {id} not found.");

            updatedProduct.Id = id; // Ensure ID consistency
            AssignIdsToNestedObjects(updatedProduct); // Ensure nested objects have IDs
            await repository.UpdateAsync(updatedProduct);
            return Results.NoContent();
        })
        .WithName("UpdateProduct")
        .WithOpenApi();

        // DELETE: Soft delete a product by ID
        group.MapDelete("/{id}", async (string id, IRepository<Product> repository) =>
        {
            var product = await repository.GetByIdAsync(id);
            if (product is null)
                return Results.NotFound($"Product with ID {id} not found.");

            product.IsDeleted = true; // Soft delete
            await repository.UpdateAsync(product); // Update instead of DeleteAsync for soft delete
            return Results.NoContent();
        })
        .WithName("DeleteProduct")
        .WithOpenApi();

        // DELETE: Soft delete a product by ID
        group.MapPost("/upload-images", async (
                 IFormFileCollection images,
                IStorageService storageService) =>
            {
            })
            .WithName("UploadImages")
            .DisableAntiforgery()
            .WithOpenApi();

        return endpoints;
    }

    // Helper method to assign IDs to ProductImage and ProductPrice if missing
    private static void AssignIdsToNestedObjects(Product product)
    {
        foreach (var image in product.ImageUrls.Where(x=> string.IsNullOrEmpty(x.Id)))
        {
            image.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
        }

        foreach (var price in product.Prices.Where(x=> string.IsNullOrEmpty(x.Id)))
        {
            price.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
        }
    }
}
