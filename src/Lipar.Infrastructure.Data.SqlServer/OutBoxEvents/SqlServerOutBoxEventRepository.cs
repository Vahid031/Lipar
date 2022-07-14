using Microsoft.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using Lipar.Core.Contract.Data;
using Lipar.Core.Domain.Events;
using Lipar.Core.Domain.Entities;
using Lipar.Core.Contract.Services;
using System;

namespace Lipar.Infrastructure.Data.SqlServer.OutBoxEvents;

public class SqlServerOutBoxEventRepository : IOutBoxEventRepository
{
    private readonly SqlServerOptions sqlServer;
    private readonly IUserInfoService userInfoService;
    private readonly IJsonService jsonService;
    private readonly IDateTimeService dateTimeService;
    private readonly string SelectCommand;
    private readonly string UpdateCommand;
    private readonly string InsertCommand;


    public SqlServerOutBoxEventRepository(LiparOptions liparOptions, IUserInfoService userInfoService, IJsonService jsonService, IDateTimeService dateTimeService)
    {
        this.sqlServer = liparOptions.OutBoxEvent.SqlServer;
        this.userInfoService = userInfoService;
        this.jsonService = jsonService;
        this.dateTimeService = dateTimeService;

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
                $"IF (NOT EXISTS (SELECT *  FROM INFORMATION_SCHEMA.TABLES WHERE " +
                $"TABLE_SCHEMA = '{sqlServer.SchemaName}' AND  TABLE_NAME = '{sqlServer.TableName}' )) Begin " +
                $"CREATE TABLE {sqlServer.SchemaName}.{sqlServer.TableName}(" +
                $"[Id][uniqueidentifier] NOT NULL," +
                $"[AccuredByUserId] [nvarchar](40) NULL," +
                $"[AccuredOn] [datetime2](7) NOT NULL," +

                $"[AggregateName] [nvarchar](200) NULL," +
                $"[AggregateTypeName] [nvarchar](500) NULL," +
                $"[AggregateId] [nvarchar](40) NULL," +
                $"[EventName] [nvarchar](100) NULL," +
                $"[EventTypeName] [nvarchar](500) NULL," +
                $"[EventPayload] [nvarchar](max)NULL," +
                $"[IsProcessed] [bit] NOT NULL," +
                $"CONSTRAINT[PK_{sqlServer.TableName}] PRIMARY KEY NONCLUSTERED" +
                $"([Id] ASC)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON[PRIMARY]" +
                $") ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]" +
                $" End";

        using var connection = new SqlConnection(sqlServer.ConnectionString);
        connection.Execute(createTableQuery);
    }

    public void AddOutboxEvetItems(List<AggregateRoot> changedAggregates)
    {
        using var connection = new SqlConnection(sqlServer.ConnectionString);

        foreach (var aggregate in changedAggregates)
        {
            foreach (var @event in aggregate.GetChanges())
            {
                connection.Execute(InsertCommand, new OutBoxEvent
                {
                    Id = Guid.NewGuid(),
                    AccuredByUserId = userInfoService.UserId.ToString(),
                    AccuredOn = dateTimeService.Now,
                    AggregateId = aggregate.Id.ToString(),
                    AggregateName = aggregate.GetType().Name,
                    AggregateTypeName = aggregate.GetType().FullName,
                    EventName = @event.GetType().Name,
                    EventTypeName = @event.GetType().FullName,
                    EventPayload = jsonService.SerializeObject(@event),
                    IsProcessed = false
                });
            }
        }
    }

    public List<OutBoxEvent> GetOutBoxEventItemsForPublish(int maxCount)
    {
        using var connection = new SqlConnection(sqlServer.ConnectionString);
        string query = string.Format(SelectCommand, maxCount);

        return connection.Query<OutBoxEvent>(query).ToList();
    }

    public void MarkAsRead(List<OutBoxEvent> outBoxEventItems)
    {
        string idForMark = string.Join(',', outBoxEventItems.Where(c => c.IsProcessed).Select(c => $"'{c.Id}'").ToList());
        if (!string.IsNullOrWhiteSpace(idForMark))
        {
            using var connection = new SqlConnection(sqlServer.ConnectionString);
            string query = string.Format(UpdateCommand, idForMark);
            connection.Execute(query);
        }
    }

}


