using System;

namespace Market.Core.Domain.Products.QueryResults;

public class GetByIdProduct
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Barcode { get; set; }
}

public interface IGetByIdProduct
{
    public Guid Id { get; init; }
}
