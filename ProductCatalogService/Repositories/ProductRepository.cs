using System;
using MongoDB.Driver;
using ProductCatalogService.Models;
using ProductCatalogService.Repositories.Common;

namespace ProductCatalogService.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Task<List<Product>> GetByIdsAsync(IEnumerable<string> ids);
}

public class ProductRepository(IMongoDatabase database) : Repository<Product>(database), IProductRepository
{
    public async Task<List<Product>> GetByIdsAsync(IEnumerable<string> ids)
    {
        var filter = Builders<Product>.Filter.In(x => x.Id, ids);
        filter &= Builders<Product>.Filter.Eq(x => x.IsDeleted, false);

        var products = await _collection.Find(filter).ToListAsync();
        return products;
    }
}
