using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Lipar.Core.Domain.Data;
using Lipar.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lipar.Infrastructure.Data.SqlServer.Commands
{
    public class CommandRepository<TEntity, TDbContext> : ICommandRepository<TEntity>, IUnitOfWork
        where TEntity : AggregateRoot
        where TDbContext : CommandDbContext
    {
        protected readonly TDbContext db;

        public CommandRepository(TDbContext db)
        {
            this.db = db;
        }

        public void Delete(TEntity entity)
        {
            db.Set<TEntity>().Remove(entity);
        }

        public TEntity Get(EntityId id)
        {
            return db.Set<TEntity>().FirstOrDefault(c => c.Id == id);
        }

        public void Insert(TEntity entity)
        {
            db.Set<TEntity>().Add(entity);
        }

        public bool Exists(Expression<Func<TEntity, bool>> expression)
        {
            return db.Set<TEntity>().Any(expression);
        }

        public TEntity GetGraph(EntityId id)
        {
            var graphPath = db.GetIncludePaths(typeof(TEntity));
            IQueryable<TEntity> query = db.Set<TEntity>().AsQueryable();
            var temp = graphPath.ToList();
            foreach (var item in graphPath)
            {
                query = query.Include(item);
            }
            return query.FirstOrDefault(c => c.Id == id);
        }

        public async Task InsertAsync(TEntity entity)
        {
            await db.Set<TEntity>().AddAsync(entity);
        }

        public async Task<TEntity> GetAsync(EntityId id)
        {
            return await db.Set<TEntity>().FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<TEntity> GetGraphAsync(EntityId id)
        {
            var graphPath = db.GetIncludePaths(typeof(TEntity));
            IQueryable<TEntity> query = db.Set<TEntity>().AsQueryable();
            var temp = graphPath.ToList();
            foreach (var item in graphPath)
            {
                query = query.Include(item);
            }
            return await query.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await db.Set<TEntity>().AnyAsync(expression);
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public async Task<int> CommitAsync()
        {
            return await db.SaveChangesAsync();
        }
    }
}

