using MongoDB.Bson.Serialization.Attributes;
using ProductCatalogService.Models.Common;

namespace ProductCatalogService.Models;

public class Product : Model
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> ImageUrls { get; set; } = [];
    public string BrandId { get; set; } = string.Empty;
    public string CategoryId { get; set; } = string.Empty;
    public string AppliedPriceId { get; set; } = string.Empty;

    [BsonIgnore]
    public ProductPrice CurrentPrice { get; set; }
}
