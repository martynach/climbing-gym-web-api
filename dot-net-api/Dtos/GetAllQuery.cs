namespace dot_net_api.Dtos;

public class GetAllQuery
{
    public string SortBy { get; set; }
    public SortDirection SortDirection { get; set; }
    public string SearchBy { get; set; }
    public int PageSize { get; set; }
    
    
}