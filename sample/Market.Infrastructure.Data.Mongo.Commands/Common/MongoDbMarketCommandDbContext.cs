using Lipar.Infrastructure.Data.Mongo.Commands;
using System;

namespace Market.Infrastructure.Data.Mongo.Commands.Common;

public class MongoDbMarketCommandDbContext : BaseCommandDbContext
{
    private static bool registeredAllSerializer = false;
    public MongoDbMarketCommandDbContext(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        if (!registeredAllSerializer)
        {

            registeredAllSerializer = true;
        }

    }
}


