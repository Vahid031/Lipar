using Lipar.Core.Contract.Data;

namespace Lipar.Infrastructure.Data.Mongo.Queries
{
    public abstract class BaseQueryRepository<TDbContext> : IQueryRepository
        where TDbContext : BaseQueryDbContext
    {
        protected readonly TDbContext _db;

        public BaseQueryRepository(TDbContext db)
        {
            _db = db;
        }
    }
}
