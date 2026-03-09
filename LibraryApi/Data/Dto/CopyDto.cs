namespace LibraryApi.Data.Dto;

public class CopyDto
{
    public string ISBN { get; set; }
    public bool IsBorrowed { get; set; } = false;
    public string? Borrower { get; set; } = null;
}