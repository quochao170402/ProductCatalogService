using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProductCatalogService.Models.Common;

public interface IModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string  Id { get; set; }
    public bool IsDeleted { get; set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string LatestUpdateBy { get; set; }
    public DateTime LatestUpdatedAt { get; set; }
}
