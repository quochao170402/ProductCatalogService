using System;
using AutoMapper;
using ProductCatalogService.Constants;
using ProductCatalogService.Controllers.Payload.Brands;
using ProductCatalogService.Models;
using ProductCatalogService.Repositories.Common;

namespace ProductCatalogService.Services;


public interface IBrandService
{
    Task<BrandDto> AddAsync(AddBrandDto dto);
    Task<BrandDto> UpdateAsync(string id, AddBrandDto dto);
}

public class BrandService(IRepository<Brand> repository, IStorageService storageService, IMapper mapper) : IBrandService
{

    public async Task<BrandDto> AddAsync(AddBrandDto dto)
    {
        var brand = mapper.Map<AddBrandDto, Brand>(dto);
        if (dto.Image != null && dto.Image.Length > 0)
        {
            var url = await storageService.UploadImage(dto.Image, StorageFolderConsts.Brand);
            brand.ImageUrl = url;
        }

        await repository.AddAsync(brand);

        return mapper.Map<BrandDto>(brand);
    }

    public async Task<BrandDto> UpdateAsync(string id, AddBrandDto dto)
    {
        var existing = await repository.GetByIdAsync(id)
            ?? throw new Exception("Brand not found");

        var brand = mapper.Map<AddBrandDto, Brand>(dto);
        if (dto.Image != null && dto.Image.Length > 0)
        {
            var url = await storageService.UploadImage(dto.Image, StorageFolderConsts.Brand);
            brand.ImageUrl = url;
        }

        mapper.Map(existing, brand);
        brand.Id = existing.Id;
        await repository.UpdateAsync(brand);
        return mapper.Map<BrandDto>(brand);
    }
}