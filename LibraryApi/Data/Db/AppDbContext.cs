using LibraryApi.Data.Entities;
using Microsoft.EntityFrameworkCore;
using User = LibraryApi.Data.Auth.User;

namespace LibraryApi.Data.Db;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<BookCopy> Copies { get; set; }
    public DbSet<LibraryApi.Data.Entities.User> Users { get; set; }
    public DbSet<User> ApiUsers { get; set; }
}