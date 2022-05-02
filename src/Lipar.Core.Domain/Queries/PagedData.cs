using System.Collections.Generic;

namespace Lipar.Core.Domain.Queries
{
    public class PagedData<T>
    {
        public List<T> Result { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public int TotalCount { get; set; }
    }
}
