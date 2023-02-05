using Lipar.Infrastructure.Data.Mongo.Commands;
using System;

namespace Market.Infrastructure.Data.Mongo.Commands.Common;

public class MarketCommandDbContext : BaseCommandDbContext
{
    private static bool registeredAllSerializer = false;
    public MarketCommandDbContext(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        if (!registeredAllSerializer)
        {

            registeredAllSerializer = true;
        }

    }
}


