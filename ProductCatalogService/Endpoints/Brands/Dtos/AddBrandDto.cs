using System;

namespace ProductCatalogService.Endpoints.Brands.Dtos;

public class AddBrandDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IFormFile Image { get; set; }
}
