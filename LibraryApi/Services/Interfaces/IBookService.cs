using LibraryApi.Data.ApiResponse;
using LibraryApi.Data.Dto;
using LibraryApi.Data.Entities;

namespace LibraryApi.Services.Interfaces;

public interface IBookService
{
    public Task AddAsync(BookDto book);
    public Task<List<Book>> GetAsync();
    public Task<Book> GetByIdAsync(string id);
    public Task UpdateAsync(string id, BookDto book);
    public Task DeleteAsync(string id);
    public Task<bool> ExistsAsync(string id);
    public Task<PagedResult<Book>> GetPagedAsync(int page, int pageSize);
}