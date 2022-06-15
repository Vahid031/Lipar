using Lipar.Core.Domain.Queries;
using System;

namespace Market.Core.Domain.Products.QueryResults
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
    }

    public interface IProductDto : IPageQuery
    {
        public string Name { get; set; }
        public string Barcode { get; set; }
    }
}
