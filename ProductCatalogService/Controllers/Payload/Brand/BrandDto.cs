using System;

namespace ProductCatalogService.Controllers.Payload.Brands;

public class BrandDto
{
    public string Id { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
