using System;
using AutoMapper;
using ProductCatalogService.Endpoints.Brands.Dtos;
using ProductCatalogService.Models;

namespace ProductCatalogService.Profiles;

public class BrandProfile : Profile
{
    public BrandProfile()
    {
        CreateMap<Brand, BrandDto>();
        CreateMap<AddBrandDto, Brand>();
    }
}
