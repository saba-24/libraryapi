using AutoMapper;
using LibraryApi.Data.ApiResponse;
using LibraryApi.Data.Db;
using LibraryApi.Data.Dto;
using LibraryApi.Data.Entities;
using LibraryApi.Repository;
using LibraryApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Services.Implementations;

public class BookService(IRepository<Book> repository, AppDbContext context, IMapper mapper) : IBookService
{
    public async Task AddAsync(BookDto book)
    {
        await repository.AddAsync(mapper.Map<Book>(book));
    }

    public async Task<List<Book>> GetAsync()
    {
        return await repository.GetAllAsync();
    }

    public async Task<Book> GetByIdAsync(string id)
    {
        if (!await repository.ExistsAsync(id)) throw new ArgumentException("Invalid Book ID");
        return await repository.GetByIdAsync(id);
    }

    public async Task UpdateAsync(string id, BookDto book)
    {
        await repository.UpdateAsync(id, mapper.Map<Book>(book));
    }

    public async Task DeleteAsync(string id)
    {
        await repository.DeleteAsync(await repository.GetByIdAsync(id));
    }

    public async Task<bool> ExistsAsync(string id)
    {
        return await repository.ExistsAsync(id);
    }

    public async Task<PagedResult<Book>> GetPagedAsync(int page, int pageSize)
    {
        var query = repository.Query();
        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        var res = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PagedResult<Book>
        {
            Items = res,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }
}