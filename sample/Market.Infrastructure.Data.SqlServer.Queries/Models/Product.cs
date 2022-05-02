using System;
using System.Collections.Generic;

#nullable disable

namespace Market.Infrastructure.Data.SqlServer.Queries.Models
{
    public partial class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? ModifedBy { get; set; }
        public DateTime? ModifedDate { get; set; }
    }
}
