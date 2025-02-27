using AutoMapper;
using ProductCatalogService.Constants;
using ProductCatalogService.Controllers.Payload.Categories;
using ProductCatalogService.Exceptions;
using ProductCatalogService.Models;
using ProductCatalogService.Repositories.Common;

namespace ProductCatalogService.Services;

public interface ICategoryService
{
    Task<CategoryDto> AddAsync(AddCategoryDto dto);
    Task<CategoryDto> UpdateAsync(Guid id, UpdateCategoryDto dto);
}

public class CategoryService(IRepository<Category> repository, IStorageService storageService, IMapper mapper) : ICategoryService
{

    public async Task<CategoryDto> AddAsync(AddCategoryDto dto)
    {
        var category = mapper.Map<AddCategoryDto, Category>(dto);
        if (dto.Image is { Length: > 0 })
        {
            var url = await storageService.UploadImage(dto.Image, StorageFolderConsts.Category);
            category.ImageUrl = url;
        }

        if (string.IsNullOrEmpty(dto.SubCategories))
        {
            var subCategories = dto.GetSubCategories();
            category.SubCategories = [.. subCategories.Select(x => new Category()
            {
                Id = Guid.NewGuid(),
                Name = x.Name,
                Description = x.Descriptions
            })];
        }

        await repository.AddAsync(category);

        return mapper.Map<CategoryDto>(category);
    }

    public async Task<CategoryDto> UpdateAsync(Guid id, UpdateCategoryDto dto)
    {
        var existing = await repository.GetByIdAsync(id)
            ?? throw new EntityNotFoundException("Not found category");


        var category = mapper.Map<AddCategoryDto, Category>(dto);
        if (dto.Image is { Length: > 0 })
        {
            var url = await storageService.UploadImage(dto.Image, StorageFolderConsts.Category);
            category.ImageUrl = url;
        }

        if (dto.DeletedChildIds.Count > 0)
        {
            // verify products which were contained
            existing.SubCategories = existing.SubCategories.Where(x => !dto.DeletedChildIds.Contains(x.Id)).ToList();
        }

        if (string.IsNullOrEmpty(dto.SubCategories))
        {
            var subCategories = dto.GetSubCategories();
            var newSubCategories = subCategories.Select(x => new Category()
            {
                Id = Guid.NewGuid(),
                Name = x.Name,
                Description = x.Descriptions
            }).ToList();

            existing.SubCategories.ToList().AddRange(newSubCategories);
        }

        mapper.Map(existing, category);

        await repository.UpdateAsync(category);

        return mapper.Map<CategoryDto>(category);
    }
}
