using MongoDB.Driver;
using ProductCatalogService.Models;
using ProductCatalogService.Repositories.Common;

namespace ProductCatalogService.Repositories;

public interface IProductPriceRepository : IRepository<ProductPrice>
{
    Task<List<ProductPrice>> GetByProductIdAsync(Guid productId);
    Task<List<ProductPrice>> GetActiveByProductIdsAsync(IEnumerable<Guid> productIds);
    Task InactiveByByIds(IEnumerable<Guid> ids);
}

public class ProductPriceRepository(IMongoDatabase database) : Repository<ProductPrice>(database), IProductPriceRepository
{
    public async Task<List<ProductPrice>> GetByProductIdAsync(Guid productId)
    {
        var filter = Builders<ProductPrice>.Filter.Eq(x => x.ProductId, productId);
        filter &= Builders<ProductPrice>.Filter.Eq(x => x.IsDeleted, false);

        var prices = await _collection.Find(filter).ToListAsync();
        return prices;
    }

    public async Task<List<ProductPrice>> GetActiveByProductIdsAsync(IEnumerable<Guid> productIds)
    {
        var filter = Builders<ProductPrice>.Filter.In(x => x.ProductId, productIds);
        filter &= Builders<ProductPrice>.Filter.Eq(x => x.IsDeleted, false);
        filter &= Builders<ProductPrice>.Filter.Eq(x => x.IsActive, true);

        var prices = await _collection.Find(filter).ToListAsync();
        return prices;
    }

    public async Task InactiveByByIds(IEnumerable<Guid> ids)
    {
        var filter = Builders<ProductPrice>.Filter.In(x => x.Id, ids);
        filter &= Builders<ProductPrice>.Filter.Eq(x => x.IsDeleted, false);

        var update = Builders<ProductPrice>.Update.Set(x => x.IsActive, false);

        await _collection.UpdateManyAsync(filter, update);
    }
}
