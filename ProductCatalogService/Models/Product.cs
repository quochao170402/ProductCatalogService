using ProductCatalogService.Models.Common;

namespace ProductCatalogService.Models;

public class Product : Model
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IList<ProductImage> ImageUrls { get; set; } = [];
    public IList<ProductPrice> Prices { get; set; } = [];
    public string BrandId { get; set; } = string.Empty;
    public string CategoryId { get; set; } = string.Empty;
}

public class ProductImage
{
    public string Id { get; set; }
    public string Url { get; set; }
}

public class ProductPrice
{
    public string Id { get; set; }
    public bool IsActive { get; set; }
    public decimal Price { get; set; }
}
