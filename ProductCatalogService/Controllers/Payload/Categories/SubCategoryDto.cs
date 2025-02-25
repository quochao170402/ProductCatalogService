using System;

namespace ProductCatalogService.Controllers.Payload.Categories;

public class SubCategoryDto : AddSubCategoryDto
{
    public string Id { get; set; } = string.Empty;
    public bool IsDeleted { get; set; }
}
