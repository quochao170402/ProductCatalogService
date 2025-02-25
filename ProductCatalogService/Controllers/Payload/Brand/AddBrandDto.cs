using System;

namespace ProductCatalogService.Controllers.Payload.Brands;

public class AddBrandDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IFormFile? Image { get; set; }
}
