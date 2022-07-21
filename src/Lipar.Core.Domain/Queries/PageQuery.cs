namespace Lipar.Core.Domain.Queries;

public abstract class PageQuery : IPageQuery
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 5;
    public bool NeedTotalCount { get; set; }
    public string SortBy { get; set; }
    public bool SortAscending { get; set; } = true;
}

