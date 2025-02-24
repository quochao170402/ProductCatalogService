using ProductCatalogService.Models.Common;

namespace ProductCatalogService.Models;

public class Category : Model
{
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public IList<Category> SubCategories { get; set; } = [];
}
