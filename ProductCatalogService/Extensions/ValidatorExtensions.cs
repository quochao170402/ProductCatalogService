using System;
using FluentValidation;
using FluentValidation.AspNetCore;
using ProductCatalogService.Validators;

namespace ProductCatalogService.Extensions;

public static class ValidatorExtensions
{
    public static IServiceCollection AddValidator(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<BrandValidator>();
        services.AddValidatorsFromAssemblyContaining<CategoryValidator>();
        services.AddValidatorsFromAssemblyContaining<SubCategoryValidator>();
        services.AddValidatorsFromAssemblyContaining<AddProductDtoValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateProductDtoValidator>();
        services.AddFluentValidationAutoValidation();
        return services;

    }
}
