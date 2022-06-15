using Lipar.Core.Contract.Data;
using Lipar.Core.Domain.Entities;
using Market.Core.Domain.Categories.Entities;
using System.Threading.Tasks;

namespace Market.Core.Domain.Categories.Contracts
{
    public interface ICategoryCommandRepository : ICommandRepository<Category>
    {
        Task<bool> MakingLoopBetweenCategories(EntityId id, EntityId newParentId);
    }
}
