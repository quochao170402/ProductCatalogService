using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogService.Constants;
using ProductCatalogService.Controllers.Common;
using ProductCatalogService.Controllers.Payload.Brands;
using ProductCatalogService.Models;
using ProductCatalogService.Repositories.Common;
using ProductCatalogService.Services;
using ProductCatalogService.Services.AWSServices;

namespace ProductCatalogService.Controllers;

[ApiController]
[Route("api/brands")]
public class BrandController(
    IRepository<Brand> repository,
    IBrandService brandService,
    IMapper mapper,
    IAWSS3Service s3Service) : BaseController
{

    [HttpPost("TestS3")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> TestS3([FromForm] TestS3 request)
    {
        var filePath = Path.GetTempFileName();
        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.File.CopyToAsync(stream);
        }

        var result = await s3Service.UploadFileAsync(filePath, request.File.FileName);
        return Ok(result);
    }

    // GET: Retrieve all brands
    [HttpGet]
    public async Task<IActionResult> GetAllBrands()
    {
        var brands = await repository.GetAsync();
        return OkResponse(mapper.Map<IEnumerable<BrandDto>>(brands));
    }

    // GET: Retrieve a brand by ID
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBrandById(Guid id)
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
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateBrand(Guid id, [FromForm] AddBrandDto dto)
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
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteBrand(Guid id)
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

public class TestS3
{
    public IFormFile File { get; set; }
}
