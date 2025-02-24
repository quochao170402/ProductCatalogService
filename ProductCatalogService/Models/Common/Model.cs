using MongoDB.Bson;

namespace ProductCatalogService.Models.Common;

public class Model : IModel
{
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
    public bool IsDeleted { get; set; } = false;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string LatestUpdateBy { get; set; } = string.Empty;
    public DateTime LatestUpdatedAt { get; set; } = DateTime.Now;
}
