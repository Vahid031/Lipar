namespace Lipar.Core.Domain.Queries
{
    public class PageQuery : IPageQuery
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int SkipCount => (PageNumber - 1) * PageSize;
        public bool NeedTotalCount { get; set; }
        public string SortBy { get; set; }
        public bool SortAscending { get; set; }
    }
}