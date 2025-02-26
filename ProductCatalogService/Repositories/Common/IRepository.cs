using MongoDB.Bson;
using MongoDB.Driver;
using ProductCatalogService.Models.Common;

namespace ProductCatalogService.Repositories.Common;

public interface IRepository<T> where T : IModel
{

    // Create
    Task<string> AddAsync(T entity);

    // Read
    Task<T?> GetByIdAsync(string id);
    T? GetById(string id);
    Task<List<T>> GetAsync();

    // Update
    Task UpdateAsync(T entity);

    // Delete
    Task DeleteAsync(string id);
}
