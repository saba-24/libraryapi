using LibraryApi.Data.ApiResponse;
using LibraryApi.Data.Entities;
using LibraryApi.Data.Dto;

namespace LibraryApi.Services.Interfaces;

public interface IUserService
{
    public Task AddAsync(UserDto user);
    public Task<List<User>> GetAsync();
    public Task<User> GetByIdAsync(string id);
    public Task UpdateAsync(string id, UserDto user);
    public Task DeleteAsync(string id);
    public Task<bool> ExistsAsync(string id);
    public Task BorrowAsync(string isbn, string id);
    public Task ReturnAsync(string isbn, string id);
    public Task<PagedResult<User>> GetPagedAsync(int page, int pageSize);
}