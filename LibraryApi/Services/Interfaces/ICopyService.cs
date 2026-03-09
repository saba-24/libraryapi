using LibraryApi.Data.ApiResponse;
using LibraryApi.Data.Dto;
using LibraryApi.Data.Entities;

namespace LibraryApi.Services.Interfaces;

public interface ICopyService
{
    public Task AddAsync(CopyDto copy);
    public Task<List<BookCopy>> GetAsync();
    public Task<BookCopy> GetByIdAsync(string id);
    public Task UpdateAsync(string id, CopyDto copy);
    public Task DeleteAsync(string id);
    public Task UpdateBorrowerAsync(string user, string copy);
    public Task ReturnBookAsync(string id);
    public Task<bool> ExistsAsync(string id);
    public Task<PagedResult<BookCopy>> GetPagedAsync(int page, int pageSize);
}