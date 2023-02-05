using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Lipar.Core.Contract.Data;
using Lipar.Core.Contract.Services;
using Lipar.Core.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;
using SharpCompress.Common;

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
        var query = _collection.Find(expression);
        return query.Any();
    }

    public bool Exists(EntityId id)
    {
        var query = _collection.Find(m => m.Id == id);
        return query.Any();
    }

    public TEntity Get(EntityId id)
    {
        var data = _collection.Find(m => m.Id == id);
        return data.SingleOrDefault();
    }

    public void Insert(TEntity entity)
    {
        SetCreateDetailproperties(ref entity);
        _db.AddCommand(() => _collection.InsertOneAsync(entity));
        _db.AddEvents(entity.GetEvents());
        entity.ClearEvents();
    }

    public void Update(TEntity entity)
    {
        SetModifyDetailproperties(ref entity);
        _db.AddCommand(async () => await _collection.ReplaceOneAsync(m => m.Id == entity.Id, entity));
        _db.AddEvents(entity.GetEvents());
        entity.ClearEvents();
    }

    public void Delete(TEntity entity)
    {
        SetModifyDetailproperties(ref entity);
        _db.AddCommand(() => _collection.DeleteOneAsync(m => m.Id == entity.Id));
        _db.AddEvents(entity.GetEvents());
        entity.ClearEvents();
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
        var query = await _collection.FindAsync(m => m.Id == id);
        return await query.AnyAsync();
    }

    public async Task<TEntity> GetAsync(EntityId id)
    {
        var data = await _collection.FindAsync(m => m.Id == id);
        return data.SingleOrDefault();
    }

    public Task InsertAsync(TEntity entity)
    {
        SetCreateDetailproperties(ref entity);
        _db.AddCommand(async () => await _collection.InsertOneAsync(entity));
        _db.AddEvents(entity.GetEvents());
        entity.ClearEvents();
        return Task.CompletedTask;
    }

    public Task UpdateAsync(TEntity entity)
    {
        SetModifyDetailproperties(ref entity);
        _db.AddCommand(async () => await _collection.ReplaceOneAsync(m => m.Id == entity.Id, entity));
        _db.AddEvents(entity.GetEvents());
        entity.ClearEvents();
        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity)
    {
        SetModifyDetailproperties(ref entity);
        _db.AddCommand(() => _collection.DeleteOneAsync(m => m.Id == entity.Id));
        _db.AddEvents(entity.GetEvents());
        entity.ClearEvents();
        return Task.CompletedTask;
    }

    public async Task<int> CommitAsync() => await _db.SaveChangesAsync();


    #endregion

    #region Methods
    private void SetCreateDetailproperties(ref TEntity entity)
    {
        var userInfoService =_db.ServiceProvider.GetService<IUserInfoService>();
        var datetimeService =_db.ServiceProvider.GetService<IDateTimeService>();
        entity.SetCreateEntityDetails(userInfoService.UserId, datetimeService.Now);
    }

    private void SetModifyDetailproperties(ref TEntity entity)
    {
        var userInfoService = _db.ServiceProvider.GetService<IUserInfoService>();
        var datetimeService = _db.ServiceProvider.GetService<IDateTimeService>();
        entity.SetModifyEntityDetails(userInfoService.UserId, datetimeService.Now);
    }
    #endregion
}



