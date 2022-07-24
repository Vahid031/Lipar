using Lipar.Core.Domain.Entities;
using Lipar.Infrastructure.Data.SqlServer.Commands;
using Market.Core.Domain.Categories.Contracts;
using Market.Core.Domain.Categories.Entities;
using Market.Infrastructure.Data.SqlServer.Commands.Common;
using System.Threading.Tasks;

namespace Market.Infrastructure.Data.SqlServer.Commands.Categories;

internal class CategoryCommandRepository : BaseCommandRepository<Category, SqlServerMarketCommandDbContext>, ICategoryCommandRepository
{
    public CategoryCommandRepository(SqlServerMarketCommandDbContext db) : base(db)
    {
    }

    public Task<bool> MakingLoopBetweenCategories(EntityId id, EntityId newParentId)
    {
        throw new System.NotImplementedException();
    }
}
