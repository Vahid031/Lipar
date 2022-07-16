using Dapper;
using Lipar.Core.Contract.Data;
using Lipar.Core.Contract.Services;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using Microsoft.Data.SqlClient;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lipar.Infrastructure.Data.SqlServer.Repositories;

public class SqlServerInBoxEventRepository : IInBoxEventRepository
{
    private readonly SqlServerOptions sqlServer;
    private readonly IDateTimeService dateTimeService;
    private readonly string SelectCommand;
    private readonly string InsertCommand;


    public SqlServerInBoxEventRepository(LiparOptions liparOptions, IDateTimeService dateTimeService)
    {
        sqlServer = liparOptions.InBoxEvent.SqlServer;
        this.dateTimeService = dateTimeService;

        SelectCommand = string.Format("Select Id from {0}.{1} Where [OwnerService] = @OwnerService and [MessageId] = @MessageId",
            sqlServer.SchemaName,
            sqlServer.TableName
            );

        InsertCommand = string.Format("INSERT INTO {0}.{1} ([Id], [OwnerService] ,[MessageId], [ReceivedDate] ) values(newid(), @OwnerService,@MessageId, @ReceivedDate)",
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
                $" [ReceivedDate] [datetime2] NOT NULL," +
                $" [OwnerService] [nvarchar](100) NULL," +
                $" [MessageId] [nvarchar](50) NULL," +
                $" CONSTRAINT[PK_{sqlServer.TableName}] PRIMARY KEY NONCLUSTERED([Id]))" +

                $" CREATE UNIQUE CLUSTERED INDEX [IX_{sqlServer.TableName}_ReceivedDate] ON [{sqlServer.TableName}] ([ReceivedDate])" +
                $" End";


        using var connection = new SqlConnection(sqlServer.ConnectionString);
        connection.Execute(createTableQuery);
    }

    public bool AllowReceive(string messageId, string fromService)
    {
        using var connection = new SqlConnection(sqlServer.ConnectionString);
        var result = connection.Query<Guid>(SelectCommand, new
        {
            OwnerService = fromService,
            MessageId = messageId
        }).Any();

        return !result;
    }

    public async Task Receive(string messageId, string fromService)
    {
        using var connection = new SqlConnection(sqlServer.ConnectionString);
        await connection.ExecuteAsync(InsertCommand, new
        {
            OwnerService = fromService,
            MessageId = messageId,
            ReceivedDate = dateTimeService.Now
        });
    }
}


