namespace LibraryApi.Data.Entities;

public class User : BaseEntity
{
    protected User()
    {
    }

    public User(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
    public int BorrowCount { get; set; } = 0;
    public List<string> BorrowedBooks { get; set; } = [];
}