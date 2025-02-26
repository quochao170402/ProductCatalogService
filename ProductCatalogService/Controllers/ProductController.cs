using System;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ProductCatalogService.Controllers.Common;
using ProductCatalogService.Controllers.Payload.Products;
using ProductCatalogService.Models;
using ProductCatalogService.Repositories.Common;
using ProductCatalogService.Services;

namespace ProductCatalogService.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController(IRepository<Product> repository,
    IProductService productService,
    IMapper mapper) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var products = await productService.GetAsync();
        return OkResponse(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var product = await repository.GetByIdAsync(id)
            ?? throw new Exception("Not found product");

        return OkResponse(mapper.Map<Product, ProductDto>(product));
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Add([FromForm] AddProductDto dto)
    {
        var product = await productService.AddAsync(dto);
        return CreatedAtAction(nameof(GetById),
        new
        {
            id = product.Id
        }, new ResultDto()
        {
            Data = product,
            Status = 201,
            Message = "Create product successfully"
        });
    }

    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Update([FromRoute] string id,
        [FromForm] UpdateProductDto dto)
    {
        await productService.UpdateAsync(id, dto);
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult SetPrice([FromRoute] string id, [FromBody] SetPriceDto dto)
    {
        _ = Task.Run(() => productService.SetPriceAsync(id, dto));
        return NoContent();
    }

    [HttpPatch("{id}/{priceId}")]
    public IActionResult ApplyPrice([FromRoute] string id, [FromRoute] string priceId)
    {
        _ = Task.Run(() => productService.ApplyPrice(id, priceId));
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        await repository.DeleteAsync(id);
        return NoContent();
    }

}
