using System;
using AutoMapper;
using ProductCatalogService.Controllers.Payload.Products;
using ProductCatalogService.Models;
using ZstdSharp.Unsafe;

namespace ProductCatalogService.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<AddProductDto, Product>();
        CreateMap<UpdateProductDto, Product>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                srcMember != null && (srcMember is not string str || !string.IsNullOrWhiteSpace(str))));

        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Price, src => src.MapFrom(x => x.CurrentPrice.Price));
    }
}
