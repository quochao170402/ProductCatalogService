using MongoDB.Bson.Serialization.Attributes;
using ProductCatalogService.Models.Common;

namespace ProductCatalogService.Models;

public class Product : Model
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> ImageUrls { get; set; } = [];
    public Guid BrandId { get; set; }
    public Guid CategoryId { get; set; }
    public Guid AppliedPriceId { get; set; }

    [BsonIgnore]
    public ProductPrice CurrentPrice { get; set; }
}
