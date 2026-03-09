namespace LibraryApi.Data.Entities;

public class Book : BaseEntity
{
    protected Book()
    {
    }

    public Book(string ISBN, string Title, string Author, List<string> Copies, int Pages)
    {
        this.ISBN = ISBN;
        this.Title = Title;
        this.Author = Author;
        this.Copies = Copies;
        this.Pages = Pages;
    }

    public string ISBN { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public List<string> Copies { get; set; } = [];
    public int Pages { get; set; }
}