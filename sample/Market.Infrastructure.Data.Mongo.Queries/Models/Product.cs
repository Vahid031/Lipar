namespace Market.Infrastructure.Data.Mongo.Queries.Models;

public record Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;
    public int CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public int? ModifedBy { get; set; }
    public DateTime? ModifedOn { get; set; }
}


