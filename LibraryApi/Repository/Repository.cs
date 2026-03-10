using LibraryApi.Data.Db;
using LibraryApi.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Repository;

public class Repository<T>(AppDbContext context) : IRepository<T> where T : BaseEntity
{
    public Task<T> GetByIdAsync(string id)
    {
        return context.Set<T>().Any(s => s.Id == id) ? context.Set<T>().FirstOrDefaultAsync(s => s.Id == id) : null;
    }

    public async Task<List<T>> GetAllAsync()
    {
        return context.Set<T>().ToList();
    }

    public async Task AddAsync(T entity)
    {
        await context.Set<T>().AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        context.Set<T>().Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(string id, T entity)
    {
        entity.Id = id;
        var existingEntity = await context.Set<T>().FindAsync(id);
        if (existingEntity != null)
        {
            context.Entry(existingEntity).CurrentValues.SetValues(entity);
            await context.SaveChangesAsync();
        }
    }

    public Task<bool> ExistsAsync(string id)
    {
        return context.Set<T>().AnyAsync(s => s.Id == id);
    }

    public async Task SaveAsync()
    {
        context.SaveChangesAsync();
    }

    public IQueryable<T> Query()
    {
        return context.Set<T>().AsQueryable();
    }
}