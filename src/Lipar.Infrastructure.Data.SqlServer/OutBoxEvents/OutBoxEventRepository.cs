using Microsoft.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using Lipar.Core.Contract.Data;
using Lipar.Core.Domain.Events;
using Lipar.Core.Domain.Entities;
using Lipar.Core.Contract.Utilities;
using System;

namespace Lipar.Infrastructure.Data.SqlServer.OutBoxEvents
{
    public class OutBoxEventRepository : IOutBoxEventRepository
    {
        private readonly LiparOptions liparOptions;
        private readonly IUserInfo userInfo;
        private readonly IJson json;
        private readonly IDateTime dateTime;

        public OutBoxEventRepository(LiparOptions liparOptions, IUserInfo userInfo, IJson json, IDateTime dateTime)
        {
            this.liparOptions = liparOptions;
            this.userInfo = userInfo;
            this.json = json;
            this.dateTime = dateTime;
        }

        public void AddOutboxEvetItems(List<AggregateRoot> changedAggregates)
        {
            using var connection = new SqlConnection(liparOptions.OutBoxEvent.ConnectionString);

            foreach (var aggregate in changedAggregates)
            {
                foreach (var @event in aggregate.GetChanges())
                {
                    connection.Execute(liparOptions.OutBoxEvent.InsertCommand, new Core.Domain.Events.OutBoxEvent
                    {
                        Id = Guid.NewGuid(),
                        AccuredByUserId = userInfo.UserId.ToString(),
                        AccuredOn = dateTime.DateTime,
                        AggregateId = aggregate.Id.ToString(),
                        AggregateName = aggregate.GetType().Name,
                        AggregateTypeName = aggregate.GetType().FullName,
                        EventName = @event.GetType().Name,
                        EventTypeName = @event.GetType().FullName,
                        EventPayload = json.SerializeObject(@event),
                        IsProcessed = false
                    });
                }
            }
        }

        public List<Core.Domain.Events.OutBoxEvent> GetOutBoxEventItemsForPublish(int maxCount)
        {
            using var connection = new SqlConnection(liparOptions.OutBoxEvent.ConnectionString);
            string query = string.Format(liparOptions.OutBoxEvent.SelectCommand, maxCount);

            return connection.Query<Core.Domain.Events.OutBoxEvent>(query).ToList();
        }

        public void MarkAsRead(List<Core.Domain.Events.OutBoxEvent> outBoxEventItems)
        {
            string idForMark = string.Join(',', outBoxEventItems.Where(c => c.IsProcessed).Select(c => $"'{c.Id}'").ToList());
            if (!string.IsNullOrWhiteSpace(idForMark))
            {
                using var connection = new SqlConnection(liparOptions.OutBoxEvent.ConnectionString);
                string query = string.Format(liparOptions.OutBoxEvent.UpdateCommand, idForMark);
                connection.Execute(query);
            }
        }

    }
}
