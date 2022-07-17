using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Lipar.Core.Contract.Data;
using Lipar.Core.Domain.Entities;
using MongoDB.Driver;

namespace Lipar.Infrastructure.Data.Mongo.Commands;

public abstract class BaseCommandRepository<TEntity, TDbContext> : ICommandRepository<TEntity>
where TEntity : AggregateRoot
where TDbContext : BaseCommandDbContext
{
    private readonly TDbContext _db;
    private readonly IMongoCollection<TEntity> _collection;

    protected BaseCommandRepository(TDbContext db)
    {
        _db = db;
        _collection = db.GetCollection<TEntity>(typeof(TEntity).Name);
    }

    #region Sync Func
    public bool Exists(Expression<Func<TEntity, bool>> expression)
    {
        var query = _collection.Find(Builders<TEntity>.Filter.Where(expression));
        return query.Any();
    }

    public bool Exists(EntityId id)
    {
        var query = _collection.Find(Builders<TEntity>.Filter.Eq("_id", id));
        return query.Any();
    }

    public TEntity Get(EntityId id)
    {
        var data = _collection.Find(Builders<TEntity>.Filter.Eq("_id", id));
        return data.SingleOrDefault();
    }

    public void Insert(TEntity entity)
    {
        _db.AddCommand(() => _collection.InsertOneAsync(entity));
    }

    public void Update(TEntity entity)
    {
        _db.AddCommand(async () => await _collection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", entity.Id), entity));
    }

    public void Delete(TEntity entity)
    {
        _db.AddCommand(() => _collection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", entity.Id)));
    }

    public int Commit() => _db.SaveChanges();
    #endregion

    #region Async Func

    public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression)
    {
        var query = await _collection.FindAsync(Builders<TEntity>.Filter.Where(expression));
        return query.Any();
    }

    public async Task<bool> ExistsAsync(EntityId id)
    {
        var query = await _collection.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));
        return query.Any();
    }

    public async Task<TEntity> GetAsync(EntityId id)
    {
        var data = await _collection.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));
        return data.SingleOrDefault();
    }

    public Task InsertAsync(TEntity entity)
    {
        _db.AddCommand(async () => await _collection.InsertOneAsync(entity));

        return Task.CompletedTask;
    }

    public Task UpdateAsync(TEntity entity)
    {
        _db.AddCommand(async () => await _collection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("_id", entity.Id), entity));

        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity)
    {
        _db.AddCommand(() => _collection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", entity.Id)));

        return Task.CompletedTask;
    }

    public async Task<int> CommitAsync() => await _db.SaveChangesAsync();
    #endregion

}



