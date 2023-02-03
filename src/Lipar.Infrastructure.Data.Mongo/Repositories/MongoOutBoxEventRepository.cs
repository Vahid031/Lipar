using System.Collections.Generic;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using Lipar.Core.Contract.Data;
using Lipar.Core.Domain.Events;
using System.Threading.Tasks;

namespace Lipar.Infrastructure.Data.Mongo.Repositories;

public class MongoOutBoxEventRepository : IOutBoxEventRepository
{
    private readonly SqlServerOptions sqlServer;
    private readonly string SelectCommand;
    private readonly string UpdateCommand;
    private readonly string InsertCommand;

    public MongoOutBoxEventRepository(LiparOptions liparOptions)
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

        InsertCommand = string.Format("INSERT INTO {0}.{1}(Id, AccuredByUserId, AccuredOn, EventName, EventTypeName, EventPayload, IsProcessed) VALUES(@Id, @AccuredByUserId, @AccuredOn, @EventName, @EventTypeName, @EventPayload, @IsProcessed)",
         sqlServer.SchemaName,
         sqlServer.TableName
        );

        if (sqlServer.AutoCreateSqlTable)
            CreateTableIfNeeded();
    }

    private void CreateTableIfNeeded()
    {

    }

    public Task AddOutboxEvent(OutBoxEvent outBoxEvent)
    {
        //using var connection = new SqlConnection(sqlServer.ConnectionString);

        //await connection.ExecuteAsync(InsertCommand, outBoxEvent);

        return Task.CompletedTask;
    }

    public Task<List<OutBoxEvent>> GetOutBoxEventItemsForPublish(int maxCount)
    {
        //using var connection = new SqlConnection(sqlServer.ConnectionString);
        //string query = string.Format(SelectCommand, maxCount);

        //return (await connection.QueryAsync<OutBoxEvent>(query)).ToList();
        return Task.FromResult(new List<OutBoxEvent>());
    }

    public Task MarkAsRead(List<OutBoxEvent> outBoxEvents)
    {
        //string idForMark = string.Join(',', outBoxEvents.Where(c => c.IsProcessed).Select(c => $"'{c.Id}'").ToList());
        //if (!string.IsNullOrWhiteSpace(idForMark))
        //{
        //    using var connection = new SqlConnection(sqlServer.ConnectionString);
        //    string query = string.Format(UpdateCommand, idForMark);
        //    await connection.ExecuteAsync(query);
        //}
        return Task.CompletedTask;
    }
}


