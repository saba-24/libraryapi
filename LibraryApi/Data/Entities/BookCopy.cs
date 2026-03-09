namespace LibraryApi.Data.Entities;

public class BookCopy : BaseEntity
{
    protected BookCopy()
    {
    }

    public BookCopy(string ISBN)
    {
        this.ISBN = ISBN;
    }

    public string ISBN { get; set; }
    public bool IsBorrowed { get; set; } = false;
    public string? Borrower { get; set; } = null;
}