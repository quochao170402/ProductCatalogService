using System;

namespace ProductCatalogService.Endpoints.Brands.Dtos;

public class BrandDto
{
    public string Id { get; set; }
    public bool IsDeleted { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
