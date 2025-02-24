using ProductCatalogService.Models;
using ProductCatalogService.Models.Common;
using ProductCatalogService.Repositories.Common;

namespace ProductCatalogService.Endpoints;

public static class CategoryEndpoint
{
    public static IEndpointRouteBuilder MapCategoryEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/categories").WithTags("Categories");

        // GET: Retrieve all categories (non-deleted)
        group.MapGet("/", async (IRepository<Category> repository) =>
        {
            var categories = await repository.GetAsync();
            return Results.Ok(categories);
        })
        .WithName("GetAllCategories")
        .WithOpenApi();

        // GET: Retrieve a category by ID (non-deleted)
        group.MapGet("/{id}", async (string id, IRepository<Category> repository) =>
        {
            var category = await repository.GetByIdAsync(id);
            return category is not null
                ? Results.Ok(category)
                : Results.NotFound($"Category with ID {id} not found.");
        })
        .WithName("GetCategoryById")
        .WithOpenApi();

        // POST: Create a new category
        group.MapPost("/", async (Category category, IRepository<Category> repository) =>
        {
            // Ensure subcategories have IDs if provided
            AssignIdsToSubCategories(category.SubCategories);
            var newId = await repository.AddAsync(category);
            return Results.Created($"/api/categories/{newId}", category);
        })
        .WithName("CreateCategory")
        .WithOpenApi();

        // PUT: Update an existing category
        group.MapPut("/{id}", async (string id, Category updatedCategory, IRepository<Category> repository) =>
        {
            var existingCategory = await repository.GetByIdAsync(id);
            if (existingCategory is null)
                return Results.NotFound($"Category with ID {id} not found.");

            updatedCategory.Id = id; // Ensure ID consistency
            AssignIdsToSubCategories(updatedCategory.SubCategories); // Ensure subcategories have IDs
            await repository.UpdateAsync(updatedCategory);
            return Results.NoContent();
        })
        .WithName("UpdateCategory")
        .WithOpenApi();

        // DELETE: Soft delete a category by ID
        group.MapDelete("/{id}", async (string id, IRepository<Category> repository) =>
        {
            var category = await repository.GetByIdAsync(id);
            if (category is null)
                return Results.NotFound($"Category with ID {id} not found.");

            category.IsDeleted = true; // Soft delete
            await repository.UpdateAsync(category); // Update instead of DeleteAsync for soft delete
            return Results.NoContent();
        })
        .WithName("DeleteCategory")
        .WithOpenApi();

        return endpoints;
    }

    // Helper method to ensure all subcategories have IDs
    private static void AssignIdsToSubCategories(IList<Category> subCategories)
    {
        foreach (var subCategory in subCategories)
        {
            if (string.IsNullOrEmpty(subCategory.Id))
                subCategory.Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString();
            AssignIdsToSubCategories(subCategory.SubCategories); // Recursive call for nested subcategories
        }
    }
}
