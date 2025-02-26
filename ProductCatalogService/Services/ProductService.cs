using System;
using AutoMapper;
using ProductCatalogService.Constants;
using ProductCatalogService.Controllers.Payload.Products;
using ProductCatalogService.Models;
using ProductCatalogService.Repositories;
using ProductCatalogService.Repositories.Common;

namespace ProductCatalogService.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAsync();
    Task<ProductDto> GetByIdAsync(string id);
    Task<ProductDto> AddAsync(AddProductDto dto);
    Task UpdateAsync(string id, UpdateProductDto dto);
    Task ApplyPrice(string productId, string priceId);
    Task SetPriceAsync(string productId, SetPriceDto dto);
    Task<List<ProductDto>> GetByIdsAsync(List<string> productIds);
}

public class ProductService(IProductRepository repository,
    IProductPriceRepository priceRepository,
    IStorageService storageService,
    IMapper mapper) : IProductService
{
    public async Task<List<ProductDto>> GetByIdsAsync(List<string> productIds)
    {
        var products = await repository.GetByIdsAsync(productIds);
        var prices = await priceRepository.GetActiveByProductIdsAsync(products.Select(x => x.Id));
        var priceMap = prices.ToDictionary(x => x.ProductId);

        products.ForEach(x =>
        {
            if (priceMap.TryGetValue(x.Id, out var price))
            {
                x.CurrentPrice = price;
            }
        });

        return mapper.Map<List<Product>, List<ProductDto>>(products);
    }
    
    public async Task<IEnumerable<ProductDto>> GetAsync()
    {
        var products = await repository.GetAsync();
        var prices = await priceRepository.GetActiveByProductIdsAsync(products.Select(x => x.Id));
        var priceMap = prices.ToDictionary(x => x.ProductId);

        products.ForEach(x =>
        {
            if (priceMap.TryGetValue(x.Id, out var price))
            {
                x.CurrentPrice = price;
            }
        });

        return mapper.Map<List<Product>, List<ProductDto>>(products);
    }

    public async Task<ProductDto> GetByIdAsync(string id)
    {
        var product = await repository.GetByIdAsync(id)
            ?? throw new Exception("Not found product");

        var price = await priceRepository.GetByIdAsync(product.AppliedPriceId)
            ?? throw new Exception("Applied price was deleted");

        product.CurrentPrice = price;
        return mapper.Map<Product, ProductDto>(product);
    }

    public async Task<ProductDto> AddAsync(AddProductDto dto)
    {
        var product = mapper.Map<AddProductDto, Product>(dto);
        if (dto.Images != null && dto.Images.Count > 0)
        {
            var urls = await storageService.UploadImages(dto.Images, StorageFolderConsts.Product);
            product.ImageUrls = urls;
        }

        await repository.AddAsync(product);

        if (dto.Price != null)
        {
            var price = new ProductPrice()
            {
                AppliedAt = DateTime.Now,
                IsActive = true,
                Price = (decimal)dto.Price,
                ProductId = product.Id
            };

            product.AppliedPriceId = await priceRepository.AddAsync(price);

            product.CurrentPrice = price;

            await repository.UpdateAsync(product);
        }

        return mapper.Map<Product, ProductDto>(product);
    }

    public async Task UpdateAsync(string id, UpdateProductDto dto)
    {
        var existing = await repository.GetByIdAsync(id)
            ?? throw new Exception("Not found product");

        var deletedImages = dto.GetDeletedImages();
        if (deletedImages.Count > 0)
        {
            await storageService.DeleteImages(deletedImages);
            existing.ImageUrls = [.. existing.ImageUrls.Except(deletedImages)];
        }

        if (dto.Images != null && dto.Images.Count > 0)
        {
            var urls = await storageService.UploadImages(dto.Images, StorageFolderConsts.Product);
            existing.ImageUrls.ToList().AddRange(urls);
        }

        mapper.Map(dto, existing);

        await repository.UpdateAsync(existing);
    }

    public async Task ApplyPrice(string productId, string priceId)
    {
        var product = await repository.GetByIdAsync(productId)
            ?? throw new Exception("Not found product");

        var price = await priceRepository.GetByIdAsync(priceId)
            ?? throw new Exception("Not found price");

        var otherPricesOfProduct = await priceRepository.GetByProductIdAsync(productId);
        await priceRepository.InactiveByByIds(otherPricesOfProduct.Select(x => x.Id));

        price.AppliedAt = DateTime.Now;
        price.IsActive = true;

        product.AppliedPriceId = price.Id;

        _ = Task.WhenAll([
            Task.Run(() => repository.UpdateAsync(product)),
            Task.Run(() => priceRepository.UpdateAsync(price))
        ]);
    }

    public async Task SetPriceAsync(string productId, SetPriceDto dto)
    {

        var product = await repository.GetByIdAsync(productId)
            ?? throw new Exception("Not found product");

        var otherPricesOfProduct = await priceRepository.GetByProductIdAsync(productId);
        await priceRepository.InactiveByByIds(otherPricesOfProduct.Select(x => x.Id));

        var price = new ProductPrice()
        {
            Price = dto.Price,
            AppliedAt = DateTime.Now,
            ProductId = product.Id,
            IsActive = true
        };

        await priceRepository.AddAsync(price);
        product.AppliedPriceId = price.Id;
        await repository.UpdateAsync(product);
    }

}
