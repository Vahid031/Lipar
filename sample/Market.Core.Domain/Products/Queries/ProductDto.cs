using Lipar.Core.Domain.Queries;
using System;

namespace Market.Core.Domain.Products.Queries
{
    public class ProductDto 
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
    }

    public class ProductVM : PageQuery
    {
        public string Name { get; set; }
        public string Barcode { get; set; }
    }
}
