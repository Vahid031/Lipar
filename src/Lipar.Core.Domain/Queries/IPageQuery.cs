namespace Lipar.Core.Domain.Queries;

public interface IPageQuery
{
public int PageNumber { get; set; }
public int PageSize { get; set; }
public bool NeedTotalCount { get; set; }
public string SortBy { get; set; }
public bool SortAscending { get; set; }
}

