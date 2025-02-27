using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductCatalogService.Models.Common;

public class Model : IModel
{
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; } = false;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string LatestUpdateBy { get; set; } = string.Empty;
    public DateTime LatestUpdatedAt { get; set; } = DateTime.Now;
}
