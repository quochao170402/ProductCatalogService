using System;

namespace ProductCatalogService.Controllers.Payload.Products;

public class AddProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IFormFileCollection? Images { get; set; }
    public decimal? Price { get; set; }
    public string BrandId { get; set; }
    public string CategoryId { get; set; }
}
