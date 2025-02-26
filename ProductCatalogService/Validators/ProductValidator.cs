using System;
using FluentValidation;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using ProductCatalogService.Controllers.Payload.Products;

namespace ProductCatalogService.Validators;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ProductCatalogService.Models;
using ProductCatalogService.Repositories.Common;

public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
{
    public UpdateProductDtoValidator()
    {

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.");

        RuleForEach(x => x.Images)
            .Must(IsValidImage)
            .WithMessage("Each image must be a valid file format (JPG, PNG, JPEG) and not exceed 5MB.")
            .When(x => x.Images != null);

        RuleFor(x => x.DeletedImages)
            .Must(BeValidJson)
            .WithMessage("DeletedImages must be a valid JSON array of strings.");
    }

    private bool IsValidImage(IFormFile file)
    {
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var maxSize = 5 * 1024 * 1024; // 5MB

        var fileExtension = Path.GetExtension(file.FileName)?.ToLower();
        return allowedExtensions.Contains(fileExtension) && file.Length <= maxSize;
    }

    private bool BeValidJson(string json)
    {
        if (string.IsNullOrEmpty(json)) return true;
        try
        {
            var list = JsonConvert.DeserializeObject<List<string>>(json) ?? [];
            return true;
        }
        catch
        {
            return false;
        }
    }
}

public class AddProductDtoValidator : AbstractValidator<AddProductDto>
{
    private readonly IRepository<Brand> _brandRepository;
    private readonly IRepository<Category> _categoryRepository;

    public AddProductDtoValidator(IRepository<Brand> brandRepository,
        IRepository<Category> categoryRepository)
    {
        _brandRepository = brandRepository;
        _categoryRepository = categoryRepository;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.Price)
            .NotNull().WithMessage("Price is required.")
            .GreaterThan(0).WithMessage("Price must be greater than zero.");

        RuleForEach(x => x.Images)
            .Must(IsValidImage)
            .WithMessage("Each image must be a valid file format (JPG, PNG, JPEG) and not exceed 5MB.")
            .When(x => x.Images != null);

        RuleFor(x => x.BrandId)
            .NotEmpty().WithMessage("BrandId is required.")
            .Must(BrandExists).WithMessage("BrandId does not exist.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("CategoryId is required.")
            .Must(CategoryExists).WithMessage("CategoryId does not exist.");
    }

    private bool IsValidImage(IFormFile file)
    {
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var maxSize = 5 * 1024 * 1024; // 5MB

        var fileExtension = Path.GetExtension(file.FileName)?.ToLower();
        return allowedExtensions.Contains(fileExtension) && file.Length <= maxSize;
    }

    private bool BrandExists(string brandId)
    {
        return _brandRepository.GetById(brandId) != null;
    }

    private bool CategoryExists(string categoryId)
    {
        return _categoryRepository.GetById(categoryId) != null;
    }
}
