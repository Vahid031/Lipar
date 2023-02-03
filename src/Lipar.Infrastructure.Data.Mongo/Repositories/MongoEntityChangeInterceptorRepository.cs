using Lipar.Core.Contract.Data;
using Lipar.Core.Contract.Services;
using Lipar.Core.Domain.Events;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lipar.Infrastructure.Data.Mongo.Repositories;

public class MongoEntityChangeInterceptorRepository : IEntityChangesInterceptorRepository
{
    private readonly SqlServerOptions sqlServer;
    private readonly IUserInfoService userInfoService;
    private readonly IDateTimeService dateTimeService;
    private readonly IJsonService jsonService;
    private readonly string InsertCommand;

    public MongoEntityChangeInterceptorRepository(LiparOptions liparOptions, IUserInfoService userInfoService, IJsonService jsonService, IDateTimeService dateTimeService)
    {
        sqlServer = liparOptions.EntityChangesInterception.SqlServer;
        this.userInfoService = userInfoService;
        this.jsonService = jsonService;
        this.dateTimeService = dateTimeService;

        InsertCommand = string.Format("INSERT INTO {0}.{1}(Id, Date, State, EntityId, EntityType, UserId, Payload) Values(@Id, @Date, @State, @EntityId, @EntityType, @UserId, @Payload)",
         sqlServer.SchemaName,
         sqlServer.TableName
        );

        if (sqlServer.AutoCreateSqlTable)
            CreateTableIfNeeded();
    }

    private void CreateTableIfNeeded()
    {

    }

    public Task AddEntityChanges(IEnumerable<EntityChangesInterception> entities)
    {
        return Task.CompletedTask;
    }
}


