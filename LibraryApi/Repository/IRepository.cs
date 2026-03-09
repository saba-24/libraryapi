using LibraryApi.Data.Entities;

namespace LibraryApi.Repository;

public interface IRepository<T> where T : BaseEntity
{
    public Task<T> GetByIdAsync(string id);
    public Task<List<T>> GetAllAsync();
    public Task AddAsync(T entity);
    public Task DeleteAsync(T entity);
    public Task UpdateAsync(string id, T entity);
    public Task<bool> ExistsAsync(string id);
    public Task SaveAsync();
    public IQueryable<T> Query();
}