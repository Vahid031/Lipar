using Lipar.Infrastructure.Data.Mongo.Commands;
using Lipar.Infrastructure.Tools.Utilities.Configurations;

namespace Market.Infrastructure.Data.Mongo.Commands.Common
{
    public class MarketCommandDbContext : BaseCommandDbContext
    {
        public MarketCommandDbContext(LiparOptions liparOptions) : base(liparOptions)
        {
        }
    }
}
