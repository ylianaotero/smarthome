using IDataAccess;

namespace Model.In;

public class PageDataRequest
{
    public int? Page { get; set; }
    public int? PageSize { get; set; }
    
    public PageData ToPageData()
    {
        int pageNumber = this.Page ?? PageData.DefaultPageNumber;
        int pageSize = this.PageSize ?? PageData.DefaultPageSize;
        
        return new PageData { PageNumber = pageNumber, PageSize = pageSize };
    }
}