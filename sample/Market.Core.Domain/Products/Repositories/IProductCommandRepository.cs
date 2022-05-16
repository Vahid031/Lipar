﻿using Lipar.Core.Contract.Data;
using Market.Core.Domain.Products.Entities;

namespace Market.Core.Domain.Products.Repositories
{
    public interface IProductCommandRepository : ICommandRepository<Product>
    {
    }
}
