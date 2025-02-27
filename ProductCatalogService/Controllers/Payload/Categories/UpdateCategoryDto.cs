using System;

namespace ProductCatalogService.Controllers.Payload.Categories;

public class UpdateCategoryDto : AddCategoryDto
{
    public List<Guid> DeletedChildIds { get; set; } = [];
}
