using System;
using FluentValidation;
using ProductCatalogService.Controllers.Payload.Brands;

namespace ProductCatalogService.Validators;

public class BrandValidator : AbstractValidator<AddBrandDto>
{
    public BrandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .NotNull().WithMessage("Name cannot be null");

        RuleFor(x => x.Image)
            .Must(BeAValidImage).When(x => x.Image != null)
            .WithMessage("Invalid image format. Allowed formats: jpg, jpeg, png");
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
