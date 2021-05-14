using Lipar.Core.DomainModels.Data;

namespace Lipar.Infrastructure.Data.SqlServer.Queries
{
    public abstract class QueryRepository<TDbContext> : IQueryRepository
        where TDbContext : QueryDbContext
    {
        protected readonly TDbContext db;

        public QueryRepository(TDbContext db)
        {
            this.db = db;
        }
    }
}
