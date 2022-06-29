//using Lipar.Core.Domain.Entities;
//using Lipar.Infrastructure.Data.Mongo.Commands;
//using Market.Core.Domain.Categories.Contracts;
//using Market.Core.Domain.Categories.Entities;
//using Market.Infrastructure.Data.Mongo.Commands.Common;
//using System.Threading.Tasks;

//namespace Market.Infrastructure.Data.Mongo.Commands.Categories
//{
//    public class CategoryCommandRepository : BaseCommandRepository<Category, MarketCommandDbContext>,
//        ICategoryCommandRepository
//    {
//        public CategoryCommandRepository(MarketCommandDbContext db) : base(db)
//        {
//        }

//        public Task<bool> MakingLoopBetweenCategories(EntityId id, EntityId newParentId)
//        {
//            throw new System.NotImplementedException();
//        }
//    }
//}
