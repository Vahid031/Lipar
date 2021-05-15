using Lipar.Core.Domain.Entities;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Lipar.Core.Domain.Data
{
    public interface ICommandRepository<TEntity> : IUnitOfWork
       where TEntity : AggregateRoot
    {
        bool Exists(Expression<Func<TEntity, bool>> expression);
        TEntity Get(EntityId id);
        TEntity GetGraph(EntityId id);
        void Insert(TEntity entity);
        void Delete(TEntity entity);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> GetAsync(EntityId id);
        Task<TEntity> GetGraphAsync(EntityId id);
        Task InsertAsync(TEntity entity);
    }
}
