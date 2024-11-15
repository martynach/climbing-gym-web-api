namespace dot_net_api.Dtos;

public class PagedResult <TR>
{
    public List<TR> Items { get; }
    public int TotalCount { get; }
    public int PageNumber { get; }
    public int PageFirstItemIndex { get; }
    public int PageLastItemIndex { get; }

    public PagedResult(List<TR> items, int pageNumber, int pageSize, int totalCount)
    {
        Items = items;
        PageNumber = pageNumber;
        TotalCount = totalCount;
        var firstItemIndex = (PageNumber - 1) * pageSize + 1;
        PageFirstItemIndex = firstItemIndex <= totalCount ? firstItemIndex : -1;
        var lastItemIndex = pageNumber * pageSize;
        PageLastItemIndex = lastItemIndex <= totalCount ? lastItemIndex : ((lastItemIndex - totalCount) < pageSize ? totalCount : -1) ;
    }
    
}