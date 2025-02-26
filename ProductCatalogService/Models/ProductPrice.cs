using System;
using ProductCatalogService.Models.Common;

namespace ProductCatalogService.Models;

public class ProductPrice : Model
{
    public DateTime AppliedAt { get; set; }
    public bool IsActive { get; set; } = false;
    public decimal Price { get; set; }
    public string ProductId { get; set; } = string.Empty;
}

