using System;
using AutoMapper;
using MongoDB.Bson;
using ProductCatalogService.Constants;
using ProductCatalogService.Controllers.Payload.Categories;
using ProductCatalogService.Models;
using ProductCatalogService.Repositories.Common;

namespace ProductCatalogService.Services;

public interface ICategoryService
{
    Task<CategoryDto> AddAsync(AddCategoryDto dto);
    Task<CategoryDto> UpdateAsync(string id, UpdateCategoryDto dto);
}

public class CategoryService(IRepository<Category> repository, IStorageService storageService, IMapper mapper) : ICategoryService
{

    public async Task<CategoryDto> AddAsync(AddCategoryDto dto)
    {
        var category = mapper.Map<AddCategoryDto, Category>(dto);
        if (dto.Image != null && dto.Image.Length > 0)
        {
            var url = await storageService.UploadImage(dto.Image, StorageFolderConsts.Category);
            category.ImageUrl = url;
        }

        // if (dto.SubCategories.Count > 0)
        // {
        //     var newSubCategories = dto.SubCategories.Select(x => new Category()
        //     {
        //         Id = ObjectId.GenerateNewId().ToString(),
        //         Name = x.Name,
        //         Description = x.Descriptions
        //     }).ToList();

        //     category.SubCategories = newSubCategories;
        // }

        if (string.IsNullOrEmpty(dto.SubCategories))
        {
            var subCategories = dto.GetSubCategories();
            category.SubCategories = [.. subCategories.Select(x => new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Name = x.Name,
                Description = x.Descriptions
            })];
        }

        var newId = await repository.AddAsync(category);

        return mapper.Map<CategoryDto>(category);
    }

    public async Task<CategoryDto> UpdateAsync(string id, UpdateCategoryDto dto)
    {
        var existing = await repository.GetByIdAsync(id)
            ?? throw new Exception("Not found category");


        var category = mapper.Map<AddCategoryDto, Category>(dto);
        if (dto.Image != null && dto.Image.Length > 0)
        {
            var url = await storageService.UploadImage(dto.Image, StorageFolderConsts.Category);
            category.ImageUrl = url;
        }

        if (dto.DeletedChildIds.Count > 0)
        {
            // verify products which were contained
            existing.SubCategories = existing.SubCategories.Where(x => !dto.DeletedChildIds.Contains(x.Id)).ToList();
        }

        // if (dto.SubCategories.Count > 0)
        // {
        //     var newSubCategories = dto.SubCategories.Select(x => new Category()
        //     {
        //         Id = ObjectId.GenerateNewId().ToString(),
        //         Name = x.Name,
        //         Description = x.Descriptions
        //     }).ToList();

        //     existing.SubCategories.ToList().AddRange(newSubCategories);
        // }

        if (string.IsNullOrEmpty(dto.SubCategories))
        {
            var subCategories = dto.GetSubCategories();
            var newSubCategories = subCategories.Select(x => new Category()
            {
                Id = ObjectId.GenerateNewId().ToString(),
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
