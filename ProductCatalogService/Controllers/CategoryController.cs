using System;
using MongoDB.Driver;

namespace ProductCatalogService.Controllers;

using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using ProductCatalogService.Constants;
using ProductCatalogService.Controllers.Common;
using ProductCatalogService.Controllers.Payload.Brands;
using ProductCatalogService.Controllers.Payload.Categories;
using ProductCatalogService.Models;
using ProductCatalogService.Models.Common;
using ProductCatalogService.Repositories.Common;
using ProductCatalogService.Services;

[ApiController]
[Route("api/categories")]
[Produces("application/json")]
public class CategoryController(
    IRepository<Category> repository,
    ICategoryService categoryService,
    IMapper mapper) : BaseController
{

    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        var categories = await repository.GetAsync();
        return OkResponse(mapper.Map<IEnumerable<CategoryDto>>(categories));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCategoryById(Guid id)
    {
        var category = await repository.GetByIdAsync(id);
        return category == null
            ? NotFound(new { Message = $"Not found category" })
            : OkResponse(mapper.Map<CategoryDto>(category));
    }

    // POST: Create a new category
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateCategory([FromForm] AddCategoryDto dto)
    {

        var category = await categoryService.AddAsync(dto);
        return CreatedAtAction(nameof(GetCategoryById),
        new
        {
            id = category.Id
        }, new ResultDto()
        {
            Data = category,
            Status = 201,
            Message = "Create category successfully"
        });
    }

    // PUT: Update an existing category
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromForm] UpdateCategoryDto dto)
    {
        var category = await categoryService.UpdateAsync(id, dto);
        return CreatedAtAction(nameof(GetCategoryById), new
        {
            id = category.Id
        }, new ResultDto()
        {
            Data = category,
            Status = 200,
            Message = "Update category successfully"
        });
    }

    // DELETE: Soft delete a category by ID
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var category = await repository.GetByIdAsync(id);
        if (category == null)
        {
            return NotFound(new { Message = $"Not found category" });
        }

        category.IsDeleted = true; // Soft delete
        await repository.UpdateAsync(category);
        return NoContent();
    }
}
