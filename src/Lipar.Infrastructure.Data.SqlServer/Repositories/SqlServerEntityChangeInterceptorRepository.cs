using Lipar.Core.Contract.Data;
using Lipar.Core.Contract.Services;
using Lipar.Core.Domain.Events;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Dapper;

namespace Lipar.Infrastructure.Data.SqlServer.Repositories;

public class SqlServerEntityChangeInterceptorRepository : IEntityChangesInterceptorRepository
{
    private readonly SqlServerOptions sqlServer;
    private readonly IUserInfoService userInfoService;
    private readonly IDateTimeService dateTimeService;
    private readonly IJsonService jsonService;
    private readonly string InsertCommand;

    public SqlServerEntityChangeInterceptorRepository(LiparOptions liparOptions, IUserInfoService userInfoService, IJsonService jsonService, IDateTimeService dateTimeService)
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
        string createTableQuery =
                $" IF (NOT EXISTS (SELECT *  FROM INFORMATION_SCHEMA.TABLES WHERE " +
                $" TABLE_SCHEMA = '{sqlServer.SchemaName}' AND  TABLE_NAME = '{sqlServer.TableName}' ))" +
                $" Begin" +
                $" CREATE TABLE {sqlServer.SchemaName}.{sqlServer.TableName}(" +
                $" [Id][uniqueidentifier] NOT NULL," +
                $" [EntityType] nvarchar(50) NULL," +
                $" [EntityId] uniqueidentifier NOT NULL," +
                $" [State] nvarchar(10) NULL," +
                $" [Date] datetime2 NOT NULL," +
                $" [UserId] uniqueidentifier NOT NULL," +
                $" [Payload] nvarchar(max) NULL," +
                $" CONSTRAINT[PK_{sqlServer.TableName}] PRIMARY KEY NONCLUSTERED([Id]))" +

                $" CREATE UNIQUE CLUSTERED INDEX [IX_{sqlServer.TableName}_Date] ON [{sqlServer.TableName}] ([Date])" +
                $" End";


        using var connection = new SqlConnection(sqlServer.ConnectionString);
        connection.Execute(createTableQuery);
    }



    public void AddEntityChanges(IEnumerable<EntityChangesInterception> entities)
    {
        using var connection = new SqlConnection(sqlServer.ConnectionString);

        foreach (var entity in entities)
        {
            entity.SetDateTime(dateTimeService.Now);
            entity.SetUserId(userInfoService.UserId);

            var details = new Dictionary<string, object>();

            foreach (var detail in entity.Details)
                details.Add(detail.Key, detail.Value);

            connection.Execute(InsertCommand, new
            {
                entity.Id,
                entity.Date,
                entity.State,
                entity.EntityId,
                entity.EntityType,
                entity.UserId,
                Payload = jsonService.SerializeObject(details)
            });

        }
    }
}


