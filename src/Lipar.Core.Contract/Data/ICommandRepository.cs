using Lipar.Core.Domain.Entities;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Lipar.Core.Contract.Data
{
    public interface ICommandRepository<TEntity> : IUnitOfWork
       where TEntity : AggregateRoot
    {
        #region sync Func
        bool Exists(Expression<Func<TEntity, bool>> expression);
        bool Exists(EntityId id);
        TEntity Get(EntityId id);
        TEntity GetGraph(EntityId id);
        void Insert(TEntity entity);
        void Delete(TEntity entity);
        #endregion

        #region Async Func
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression);
        Task<bool> ExistsAsync(EntityId id);
        Task<TEntity> GetAsync(EntityId id);
        Task<TEntity> GetGraphAsync(EntityId id);
        Task InsertAsync(TEntity entity);
        #endregion
    }
}
