using System;

namespace ProductCatalogService.Controllers.Payload.Categories;

public class AddSubCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string Descriptions { get; set; } = string.Empty;
}
