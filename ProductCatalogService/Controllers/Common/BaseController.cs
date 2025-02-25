using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace ProductCatalogService.Controllers.Common;

public class BaseController : ControllerBase
{
    internal IActionResult OkResponse(object? data = null, string message = "", int status = 200)
    {
        return Ok(new ResultDto()
        {
            Data = data,
            Status = status,
            Message = message
        });
    }

    internal IActionResult FailResponse(string message, int status = 400)
    {
        return status switch
        {
            500 => StatusCode(status, new ResultDto()
            {
                Message = message,
                Status = status
            }),
            _ => BadRequest(new ResultDto()
            {
                Message = message,
                Status = status
            }),
        };
    }
}
