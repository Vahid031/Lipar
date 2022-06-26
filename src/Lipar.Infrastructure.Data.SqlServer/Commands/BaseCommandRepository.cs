using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Lipar.Core.Contract.Data;
using Lipar.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lipar.Infrastructure.Data.SqlServer.Commands
{
    public abstract class BaseCommandRepository<TEntity, TDbContext> : ICommandRepository<TEntity>, IUnitOfWork
        where TEntity : AggregateRoot
        where TDbContext : BaseCommandDbContext
    {
        protected readonly TDbContext _db;
        protected readonly DbSet<TEntity> _collection;

        public BaseCommandRepository(TDbContext db)
        {
            _db = db;
            _collection = _db.Set<TEntity>();
        }


        #region sync Func
        public void Delete(TEntity entity)
        {
            _collection.Remove(entity);
        }

        public TEntity Get(EntityId id)
        {
            return _collection.SingleOrDefault(c => c.Id == id);
        }

        public void Insert(TEntity entity)
        {
            _collection.Add(entity);
        }

        public bool Exists(Expression<Func<TEntity, bool>> expression)
        {
            return _collection.Any(expression);
        }

        public TEntity GetGraph(EntityId id)
        {
            var graphPath = _db.GetIncludePaths(typeof(TEntity));
            IQueryable<TEntity> query = _collection.AsQueryable();
            var temp = graphPath.ToList();
            foreach (var item in graphPath)
            {
                query = query.Include(item);
            }
            return query.SingleOrDefault(c => c.Id == id);
        }

        public int Commit()
        {
            return _db.SaveChanges();
        }

        public bool Exists(EntityId id)
        {
            return _collection.Any(m => m.Id == id);
        }
        #endregion

        #region Async Func

        public async Task InsertAsync(TEntity entity)
        {
            await _collection.AddAsync(entity);
        }

        public async Task<TEntity> GetAsync(EntityId id)
        {
            return await _collection.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<TEntity> GetGraphAsync(EntityId id)
        {
            var graphPath = _db.GetIncludePaths(typeof(TEntity));
            IQueryable<TEntity> query = _collection.AsQueryable();
            var temp = graphPath.ToList();
            foreach (var item in graphPath)
            {
                query = query.Include(item);
            }
            return await query.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await _collection.AnyAsync(expression);
        }

        public async Task<bool> ExistsAsync(EntityId id)
        {
            return await _collection.AnyAsync(m => m.Id == id);
        }

        public async Task<int> CommitAsync()
        {
            return await _db.SaveChangesAsync();
        }
        #endregion
    }
}

