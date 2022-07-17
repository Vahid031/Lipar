using Microsoft.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using Lipar.Core.Contract.Data;
using Lipar.Core.Domain.Events;
using System.Threading.Tasks;

namespace Lipar.Infrastructure.Data.SqlServer.Repositories;

public class SqlServerOutBoxEventRepository : IOutBoxEventRepository
{
    private readonly SqlServerOptions sqlServer;
    private readonly string SelectCommand;
    private readonly string UpdateCommand;
    private readonly string InsertCommand;

    public SqlServerOutBoxEventRepository(LiparOptions liparOptions)
    {
        sqlServer = liparOptions.OutBoxEvent.SqlServer;

        SelectCommand = string.Format("Select top {2} * from {0}.{1} where IsProcessed = 0",
            sqlServer.SchemaName,
            sqlServer.TableName,
            "{0}"
        );

        UpdateCommand = string.Format("Update {0}.{1} set IsProcessed = 1 where Id in ({2})",
            sqlServer.SchemaName,
            sqlServer.TableName,
            "{0}"
        );

        InsertCommand = string.Format("INSERT INTO {0}.{1}(Id, AccuredByUserId, AccuredOn, AggregateName, AggregateTypeName, AggregateId, EventName, EventTypeName, EventPayload, IsProcessed) VALUES(@Id, @AccuredByUserId, @AccuredOn, @AggregateName, @AggregateTypeName, @AggregateId, @EventName, @EventTypeName, @EventPayload, @IsProcessed)",
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
                $" [AccuredByUserId] [nvarchar](40) NULL," +
                $" [AccuredOn] [datetime2](7) NOT NULL," +
                $" [AggregateName] [nvarchar](200) NULL," +
                $" [AggregateTypeName] [nvarchar](500) NULL," +
                $" [AggregateId] [nvarchar](40) NULL," +
                $" [EventName] [nvarchar](100) NULL," +
                $" [EventTypeName] [nvarchar](500) NULL," +
                $" [EventPayload] [nvarchar](max)NULL," +
                $" [IsProcessed] [bit] NOT NULL," +
                $" CONSTRAINT[PK_{sqlServer.TableName}] PRIMARY KEY NONCLUSTERED([Id]))" +

                $" CREATE UNIQUE CLUSTERED INDEX [IX_{sqlServer.TableName}_AccuredOn] ON [{sqlServer.TableName}] ([AccuredOn])" +
                $" End";


        using var connection = new SqlConnection(sqlServer.ConnectionString);
        connection.Execute(createTableQuery);
    }

    public async Task AddOutboxEvetItems(List<OutBoxEvent> outBoxEvents)
    {
        using var connection = new SqlConnection(sqlServer.ConnectionString);

        foreach (var outBoxEvent in outBoxEvents)
        {
            await connection.ExecuteAsync(InsertCommand, outBoxEvent);
        }
    }

    public async Task<List<OutBoxEvent>> GetOutBoxEventItemsForPublish(int maxCount)
    {
        using var connection = new SqlConnection(sqlServer.ConnectionString);
        string query = string.Format(SelectCommand, maxCount);

        return (await connection.QueryAsync<OutBoxEvent>(query)).ToList();
    }

    public async Task MarkAsRead(List<OutBoxEvent> outBoxEvents)
    {
        string idForMark = string.Join(',', outBoxEvents.Where(c => c.IsProcessed).Select(c => $"'{c.Id}'").ToList());
        if (!string.IsNullOrWhiteSpace(idForMark))
        {
            using var connection = new SqlConnection(sqlServer.ConnectionString);
            string query = string.Format(UpdateCommand, idForMark);
            await connection.ExecuteAsync(query);
        }
    }
}


