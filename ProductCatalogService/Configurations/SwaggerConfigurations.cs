using System;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using ProductCatalogService.Controllers.Payload.Categories;

namespace ProductCatalogService.Configurations;

public class SwaggerConfigurations
{

}


public class FormFileAndListSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(List<AddSubCategoryDto>))
        {
            schema.Type = "array";
            schema.Items = context.SchemaGenerator.GenerateSchema(typeof(AddSubCategoryDto), context.SchemaRepository);
        }
    }
}
