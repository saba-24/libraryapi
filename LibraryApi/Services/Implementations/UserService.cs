using AutoMapper;
using LibraryApi.Data.ApiResponse;
using LibraryApi.Data.Dto;
using LibraryApi.Data.Entities;
using LibraryApi.Repository;
using LibraryApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Services.Implementations;

public class UserService(IRepository<User> repository, IMapper mapper) : IUserService
{
    public async Task AddAsync(UserDto user)
    {
        await repository.AddAsync(mapper.Map<User>(user));
    }

    public async Task<List<User>> GetAsync()
    {
        return await repository.GetAllAsync();
    }

    public async Task<User> GetByIdAsync(string id)
    {
        return await repository.GetByIdAsync(id);
    }

    public async Task UpdateAsync(string id, UserDto user)
    {
        await repository.UpdateAsync(id, mapper.Map<User>(user));
    }

    public async Task DeleteAsync(string id)
    {
        await repository.DeleteAsync(await repository.GetByIdAsync(id));
    }

    public async Task<bool> ExistsAsync(string id)
    {
        return await repository.ExistsAsync(id);
    }

    public async Task BorrowAsync(string isbn, string id)
    {
        User u = await repository.GetByIdAsync(id);
        u.BorrowedBooks.Add(isbn);
        await repository.SaveAsync();
    }

    public async Task ReturnAsync(string isbn, string id)
    {
        User u = await repository.GetByIdAsync(id);
        u.BorrowedBooks.Remove(isbn);
        await repository.SaveAsync();
    }

    public async Task<PagedResult<User>> GetPagedAsync(int page, int pageSize)
    {
        var query = repository.Query();
        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        var res = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PagedResult<User>
        {
            Items = res,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }
}