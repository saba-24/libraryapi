using AutoMapper;
using LibraryApi.Data.ApiResponse;
using LibraryApi.Data.Db;
using LibraryApi.Data.Dto;
using LibraryApi.Data.Entities;
using LibraryApi.Repository;
using LibraryApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Services.Implementations;

public class CopyService(IRepository<BookCopy> repository, AppDbContext context, IMapper mapper) : ICopyService
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
        BookCopy book = await repository.GetByIdAsync(id);
        await repository.DeleteAsync(book);
    }

    public async Task UpdateBorrowerAsync(string user, string copy)
    {
        BookCopy book = await repository.GetByIdAsync(copy);
        User u = await context.Users.FirstOrDefaultAsync(u => u.Id == user);
        if(book.IsBorrowed) throw new Exception("Book is already borrowed");
        if(u is null) throw new Exception("User not found");
        BookCopy newBook = new(book.ISBN)
        {
            Id = book.Id,
            Borrower = user,
            IsBorrowed = true,
            CreatedAt = book.CreatedAt
        };
        u.BorrowedBooks.Add(book.ISBN);
        await repository.UpdateAsync(book.Id, newBook);
    }

    public async Task ReturnBookAsync(string id)
    {
        BookCopy book = await repository.GetByIdAsync(id);
        User u = await context.Users.FirstOrDefaultAsync(u => u.Id == book.Borrower);
        if(!book.IsBorrowed) throw new Exception("Book is not borrowed");
        BookCopy newBook = new(book.ISBN)
        {
            Id = book.Id,
            Borrower = null,
            IsBorrowed = false,
            CreatedAt = book.CreatedAt
        };
        u.BorrowedBooks.Remove(book.ISBN);
        await repository.UpdateAsync(book.Id, newBook);
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