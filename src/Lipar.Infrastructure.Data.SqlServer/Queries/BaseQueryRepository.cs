using Lipar.Core.Contract.Data;

namespace Lipar.Infrastructure.Data.SqlServer.Queries
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
