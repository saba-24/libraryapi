using LibraryApi.Data.Entities;

namespace LibraryApi.Data.ApiResponse;

public class PagedResult<T> where T : BaseEntity
{
    public List<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
}