using System;

namespace ProductCatalogService.Controllers.Payload.Products;

public class AddProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IFormFileCollection? Images { get; set; }
    public decimal? Price { get; set; }
    public Guid BrandId { get; set; }
    public Guid CategoryId { get; set; }
}
