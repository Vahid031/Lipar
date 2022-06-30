using System;

namespace Market.Core.Domain.Categories.QueryResults;

public class GetAllCategory
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid? ParentId { get; set; }
}

public interface IGetAllCategory
{
}
