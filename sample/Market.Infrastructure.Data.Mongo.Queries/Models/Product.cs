namespace Market.Infrastructure.Data.Mongo.Queries.Models;

public partial class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Barcode { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public int? ModifedBy { get; set; }
    public DateTime? ModifedOn { get; set; }
}


