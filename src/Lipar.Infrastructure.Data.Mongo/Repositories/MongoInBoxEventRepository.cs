using Lipar.Core.Contract.Data;
using Lipar.Core.Contract.Services;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using System.Threading.Tasks;

namespace Lipar.Infrastructure.Data.Mongo.Repositories;

public class MongoInBoxEventRepository : IInBoxEventRepository
{
    private readonly SqlServerOptions sqlServer;
    private readonly IDateTimeService dateTimeService;
    private readonly string SelectCommand;
    private readonly string InsertCommand;


    public MongoInBoxEventRepository(LiparOptions liparOptions, IDateTimeService dateTimeService)
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
    }

    public bool AllowReceive(string messageId, string fromService)
    {
        //using var connection = new SqlConnection(sqlServer.ConnectionString);
        //var result = connection.Query<Guid>(SelectCommand, new
        //{
        //    OwnerService = fromService,
        //    MessageId = messageId
        //}).Any();

        //return !result;
        return true;
    }

    public Task Receive(string messageId, string fromService)
    {
        //using var connection = new SqlConnection(sqlServer.ConnectionString);
        //await connection.ExecuteAsync(InsertCommand, new
        //{
        //    OwnerService = fromService,
        //    MessageId = messageId,
        //    ReceivedDate = dateTimeService.Now
        //});
        return Task.CompletedTask;
    }
}


