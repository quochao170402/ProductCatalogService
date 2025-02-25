using System;

namespace ProductCatalogService.Controllers.Payload.Categories;

public class CategoryDto
{
    public string Id { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public List<SubCategoryDto> SubCategories { get; set; } = [];
}
