using MongoDB.Bson;
using MongoDB.Driver;
using ProductCatalogService.Models.Common;

namespace ProductCatalogService.Repositories.Common;

public interface IRepository<T> where T : IModel
{

    // Create
    Task<Guid> AddAsync(T entity);

    // Read
    Task<T?> GetByIdAsync(Guid id);
    T? GetById(Guid id);
    Task<List<T>> GetAsync();

    // Update
    Task UpdateAsync(T entity);

    // Delete
    Task DeleteAsync(Guid id);
}
