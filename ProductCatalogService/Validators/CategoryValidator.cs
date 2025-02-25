using System;
using FluentValidation;
using Newtonsoft.Json;
using ProductCatalogService.Controllers.Payload.Categories;

namespace ProductCatalogService.Validators;

public class CategoryValidator : AbstractValidator<AddCategoryDto>
{
    public CategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Descriptions)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.Image)
            .Must(BeAValidImage).When(x => x.Image != null)
            .WithMessage("Invalid image format. Allowed formats: jpg, jpeg, png");

        RuleFor(x => x.SubCategories)
            .Must(BeValidJson)
            .WithMessage("Invalid JSON format for SubCategories."); ;

        RuleForEach(x => DeserializeSubCategories(x.SubCategories))
            .SetValidator(new SubCategoryValidator())
            .OverridePropertyName("SubCategories");
    }

    private List<AddSubCategoryDto> DeserializeSubCategories(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return new List<AddSubCategoryDto>();

        try
        {
            return JsonConvert.DeserializeObject<List<AddSubCategoryDto>>(json) ?? new List<AddSubCategoryDto>();
        }
        catch
        {
            return new List<AddSubCategoryDto>(); // Return empty to avoid breaking validation
        }
    }

    private bool BeValidJson(string subCategoriesJson)
    {
        if (string.IsNullOrEmpty(subCategoriesJson)) return true;
        try
        {
            JsonConvert.DeserializeObject<List<AddSubCategoryDto>>(subCategoriesJson);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Validate image if it not null
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    private bool BeAValidImage(IFormFile? file)
    {
        if (file == null) return true;

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var fileExtension = Path.GetExtension(file.FileName).ToLower();

        return allowedExtensions.Contains(fileExtension);
    }
}

public class SubCategoryValidator : AbstractValidator<AddSubCategoryDto>
{
    public SubCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Sub-category name is required.")
            .MaximumLength(100).WithMessage("Sub-category name must not exceed 100 characters.");

        RuleFor(x => x.Descriptions)
            .MaximumLength(500).WithMessage("Sub-category description must not exceed 500 characters.");
    }
}