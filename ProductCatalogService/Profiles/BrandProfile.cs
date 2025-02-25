using System;
using AutoMapper;
using ProductCatalogService.Controllers.Payload.Brands;
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
