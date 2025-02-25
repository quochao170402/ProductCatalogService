using System;
using AutoMapper;
using Newtonsoft.Json;
using ProductCatalogService.Controllers.Payload.Categories;
using ProductCatalogService.Models;

namespace ProductCatalogService.Profiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<AddCategoryDto, Category>()
            .ForMember(dest => dest.SubCategories,
                src => src.MapFrom(x => x.SubCategories))
            .ForMember(dest => dest.SubCategories,
                src => src.MapFrom(x => JsonConvert.DeserializeObject<List<Category>>(x.SubCategories)));

        CreateMap<AddSubCategoryDto, Category>();
        CreateMap<Category, SubCategoryDto>();
        CreateMap<Category, CategoryDto>()
            .ForMember(dest => dest.SubCategories, src => src.MapFrom(x => x.SubCategories));
    }
}
