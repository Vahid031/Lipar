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

    public int Commit()
    {
        return _db.SaveChanges();
    }

    public async Task<int> CommitAsync()
    {
        return await _db.SaveChangesAsync();
    }

    public void Delete(TEntity entity)
    {
        _db.AddCommand(() => _collection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("_id", entity.Id)));
    }

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

    public TEntity Get(EntityId id)
    {
        var data = _collection.Find(Builders<TEntity>.Filter.Eq("_id", id));
        return data.SingleOrDefault();
    }

    public async Task<TEntity> GetAsync(EntityId id)
    {
        var data = await _collection.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));
        return data.SingleOrDefault();
    }

    public TEntity GetGraph(EntityId id)
    {
        var data = _collection.Find(Builders<TEntity>.Filter.Eq("_id", id));
        return data.SingleOrDefault();
    }

    public async Task<TEntity> GetGraphAsync(EntityId id)
    {
        var data = await _collection.FindAsync(Builders<TEntity>.Filter.Eq("_id", id));
        return data.SingleOrDefault();
    }

    public void Insert(TEntity entity)
    {
        _db.AddCommand(() => _collection.InsertOneAsync(entity));
    }

    public Task InsertAsync(TEntity entity)
    {
        _db.AddCommand(async () => await _collection.InsertOneAsync(entity));

        return Task.CompletedTask;
    }
}



