using Lipar.Core.DomainModels.Data;
using Lipar.Infrastructure.Data.SqlServer.Queries;

namespace Zamin.Infra.Data.Sql.Queries
{
    public abstract class BaseQueryRepository<TDbContext> : IQueryRepository
        where TDbContext : BaseQueryDbContext
    {
        protected readonly TDbContext db;

        public BaseQueryRepository(TDbContext db)
        {
            this.db = db;
        }
    }
}
