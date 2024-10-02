namespace IDataAccess;

public class PageData
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    
    public static int DefaultPageSize => 10;
    public static int DefaultPageNumber => 1;
    
    public static PageData Default => new PageData { PageNumber = DefaultPageNumber, PageSize = DefaultPageSize };
}