using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ProductCatalogService.Controllers.Payload.Products;

public class UpdateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IFormFileCollection? Images { get; set; }

    [FromForm]
    public string DeletedImages { get; set; } = "[]";

    public List<string> GetDeletedImages()
    {
        return JsonConvert.DeserializeObject<List<string>>(DeletedImages) ?? [];
    }
}
