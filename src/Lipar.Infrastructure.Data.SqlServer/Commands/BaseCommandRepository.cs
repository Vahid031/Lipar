using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Lipar.Core.Contract.Data;
using Lipar.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lipar.Infrastructure.Data.SqlServer.Commands;

public abstract class BaseCommandRepository<TEntity, TDbContext> : ICommandRepository<TEntity>, IUnitOfWork
where TEntity : AggregateRoot
where TDbContext : BaseCommandDbContext
{
    protected readonly TDbContext _db;

    public BaseCommandRepository(TDbContext db)
    {
        _db = db;
    }


    #region sync Func
    public bool Exists(EntityId id) => _db.Set<TEntity>().Any(m => m.Id == id);

    public bool Exists(Expression<Func<TEntity, bool>> expression) => _db.Set<TEntity>().Any(expression);

    public TEntity Get(EntityId id)
    {
        var graphPath = _db.GetIncludePaths(typeof(TEntity));
        IQueryable<TEntity> query = _db.Set<TEntity>().AsQueryable();
        var temp = graphPath.ToList();
        foreach (var item in graphPath)
        {
            query = query.Include(item);
        }
        return query.SingleOrDefault(c => c.Id == id);
    }

    public void Insert(TEntity entity) => _db.Set<TEntity>().Add(entity);

    public void Update(TEntity entity) => _db.Set<TEntity>().Update(entity);

    public void Delete(TEntity entity) => _db.Set<TEntity>().Remove(entity);

    public int Commit() => _db.SaveChanges();
    #endregion

    #region Async Func
    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await _db.Set<TEntity>().AnyAsync(expression);
    }

    public async Task<bool> ExistsAsync(EntityId id)
    {
        return await _db.Set<TEntity>().AnyAsync(m => m.Id == id);
    }

    public async Task<TEntity> GetAsync(EntityId id)
    {
        var graphPath = _db.GetIncludePaths(typeof(TEntity));
        IQueryable<TEntity> query = _db.Set<TEntity>().AsQueryable();
        var temp = graphPath.ToList();
        foreach (var item in graphPath)
        {
            query = query.Include(item);
        }
        return await query.SingleOrDefaultAsync(c => c.Id == id);
    }

    public async Task InsertAsync(TEntity entity)
    {
        await _db.Set<TEntity>().AddAsync(entity);
    }

    public Task UpdateAsync(TEntity entity)
    {
        _db.Set<TEntity>().Update(entity);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity)
    {
        _db.Set<TEntity>().Remove(entity);

        return Task.CompletedTask;
    }

    public async Task<int> CommitAsync()
    {
        return await _db.SaveChangesAsync();
    }
    #endregion
}



