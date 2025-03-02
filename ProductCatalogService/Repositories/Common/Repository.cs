using MongoDB.Bson;
using MongoDB.Driver;
using ProductCatalogService.Models.Common;

namespace ProductCatalogService.Repositories.Common;

public class Repository<T> : IRepository<T> where T : IModel
{
    protected readonly IMongoCollection<T> _collection;

    public Repository(IMongoDatabase database)
    {
        _collection = database.GetCollection<T>(typeof(T).Name);
    }

    public async Task<Guid> AddAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);

        return entity.Id;
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        var filter = Builders<T>.Filter.Eq(x => x.IsDeleted, false);
        filter &= Builders<T>.Filter.Eq(x => x.Id, id);
        var entity = await _collection.Find(filter).FirstOrDefaultAsync();

        return entity;
    }

    public T? GetById(Guid id)
    {
        var filter = Builders<T>.Filter.Eq(x => x.IsDeleted, false);
        filter &= Builders<T>.Filter.Eq(x => x.Id, id);
        var entity = _collection.Find(filter).FirstOrDefault();

        return entity;
    }

    public async Task<List<T>> GetAsync()
    {
        var filter = Builders<T>.Filter.Eq(x => x.IsDeleted, false);
        var entities = await _collection.Find(filter).ToListAsync();
        return entities;
    }

    public async Task UpdateAsync(T entity)
    {
        await _collection.ReplaceOneAsync(e => e.Id == entity.Id, entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _collection.DeleteOneAsync(e => e.Id == id);
    }
}
