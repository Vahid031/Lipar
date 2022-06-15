using Lipar.Core.Domain.Queries;
using System;

namespace Market.Core.Domain.Products.QueryResults
{
    public class GetProduct
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
    }

    public interface IGetProduct : IPageQuery
    {
        public string Name { get; set; }
        public string Barcode { get; set; }
    }
}
