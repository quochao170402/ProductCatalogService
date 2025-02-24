using System;

namespace ProductCatalogService.Endpoints.Common;

public class ResultDto
{
    public object? Data { get; set; }
    public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }
}
