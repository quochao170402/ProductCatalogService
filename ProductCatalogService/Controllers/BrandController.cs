using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogService.Constants;
using ProductCatalogService.Controllers.Common;
using ProductCatalogService.Controllers.Payload.Brands;
using ProductCatalogService.Models;
using ProductCatalogService.Repositories.Common;
using ProductCatalogService.Services;

namespace ProductCatalogService.Controllers;

[ApiController]
[Route("api/brands")]
public class BrandController(
    IRepository<Brand> repository,
    IBrandService brandService,
    IMapper mapper) : BaseController
{

    // GET: Retrieve all brands
    [HttpGet]
    public async Task<IActionResult> GetAllBrands()
    {
        var brands = await repository.GetAsync();
        return OkResponse(mapper.Map<IEnumerable<BrandDto>>(brands));
    }

    // GET: Retrieve a brand by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBrandById(string id)
    {
        var brand = await repository.GetByIdAsync(id);
        if (brand == null)
        {
            return NotFound(new { Message = "Brand not found" });
        }

        return OkResponse(mapper.Map<BrandDto>(brand));
    }

    // POST: Create a new brand
    [HttpPost]
    public async Task<IActionResult> CreateBrand([FromForm] AddBrandDto dto)
    {

        var brand = await brandService.AddAsync(dto);

        return CreatedAtAction(nameof(GetBrandById), new { id = brand.Id }, new ResultDto
        {
            Data = brand,
            Message = $"Created brand {brand.Name} successfully",
            Status = StatusCodes.Status201Created
        });
    }

    // PUT: Update an existing brand
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBrand(string id, [FromForm] AddBrandDto dto)
    {

        var brand = await brandService.UpdateAsync(id, dto);

        return CreatedAtAction(nameof(GetBrandById), new { id = brand.Id }, new ResultDto
        {
            Data = brand,
            Message = $"Update brand {brand.Name} successfully",
            Status = StatusCodes.Status201Created
        });
    }

    // DELETE: Delete a brand by ID
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBrand(string id)
    {
        var brand = await repository.GetByIdAsync(id);
        if (brand == null)
        {
            return NotFound(new { Message = "Brand not found" });
        }

        await repository.DeleteAsync(id);
        return NoContent();
    }
}
