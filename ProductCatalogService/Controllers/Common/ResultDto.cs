using System;

namespace ProductCatalogService.Controllers.Common;

public class ResultDto
{
    public object? Data { get; set; }
    public string Message { get; set; } = string.Empty;
    public int Status { get; set; } = 200;
}