using Lipar.Core.Domain.Queries;
using System;
using System.Collections.Generic;

namespace Market.Core.Domain.Categories.QueryResults
{
    public class GetCategory
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<GetCategory> Children { get; set; }
    }

    public interface IGetCategory : IPageQuery
    {
        public string Name { get; set; }
    }
}
