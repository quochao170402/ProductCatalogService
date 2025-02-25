using System;

namespace ProductCatalogService.Controllers.Payload.Categories;

public class UpdateCategoryDto : AddCategoryDto
{
    public List<string> DeletedChildIds { get; set; } = [];
}
