using AutoMapper;
using LibraryApi.Data.ApiResponse;
using LibraryApi.Data.Dto;
using LibraryApi.Data.Entities;
using LibraryApi.Repository;
using LibraryApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Services.Implementations;

public class CopyService(IRepository<BookCopy> repository, IMapper mapper) : ICopyService
{
    public async Task AddAsync(CopyDto copy)
    {
        await repository.AddAsync(mapper.Map<BookCopy>(copy));
    }

    public async Task<List<BookCopy>> GetAsync()
    {
        return await repository.GetAllAsync();
    }

    public async Task<BookCopy> GetByIdAsync(string id)
    {
        return await repository.GetByIdAsync(id);
    }

    public async Task UpdateAsync(string id, CopyDto copy)
    {
        await repository.UpdateAsync(id, mapper.Map<BookCopy>(copy));
    }

    public async Task DeleteAsync(string id)
    {
        await repository.DeleteAsync(mapper.Map<BookCopy>(id));
    }

    public async Task UpdateBorrowerAsync(string user, string copy)
    {
        BookCopy book = await repository.GetByIdAsync(copy);
        if (book.IsBorrowed) throw new Exception("Book is already borrowed");
        book.IsBorrowed = true;
        book.Borrower = user;
        await repository.SaveAsync();
    }

    public async Task ReturnBookAsync(string id)
    {
        BookCopy book = await repository.GetByIdAsync(id);
        if (!book.IsBorrowed) throw new Exception("Book isn't borrowed");
        book.IsBorrowed = false;
        book.Borrower = null;
        await repository.SaveAsync();
    }

    public async Task<bool> ExistsAsync(string id)
    {
        return await repository.ExistsAsync(id);
    }

    public async Task<PagedResult<BookCopy>> GetPagedAsync(int page, int pageSize)
    {
        var query = repository.Query();
        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        var res = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return new PagedResult<BookCopy>
        {
            Items = res,
            TotalCount = totalCount,
            TotalPages = totalPages
        };
    }
}