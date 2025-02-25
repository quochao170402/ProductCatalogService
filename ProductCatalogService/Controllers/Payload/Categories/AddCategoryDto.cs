using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ProductCatalogService.Controllers.Payload.Categories;

public class AddCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string Descriptions { get; set; } = string.Empty;
    public IFormFile? Image { get; set; }

    [FromForm]
    public string SubCategories { get; set; } = "[]";

    public List<AddSubCategoryDto> GetSubCategories()
    {
        return JsonConvert.DeserializeObject<List<AddSubCategoryDto>>(SubCategories) ?? [];
    }
}
