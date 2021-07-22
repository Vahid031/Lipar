using Microsoft.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using Lipar.Infrastructure.Events.OutboxEvent;

namespace Lipar.Infrastructure.Data.SqlServer.OutBoxEvents
{
    public class SqlOutBoxEventRepository : IOutBoxEventRepository
    {
        private readonly LiparOptions liparOptions;

        public SqlOutBoxEventRepository(LiparOptions liparOptions)
        {
            this.liparOptions = liparOptions;
        }

        public List<OutBoxEventItem> GetOutBoxEventItemsForPublish(int maxCount)
        {
            using var connection = new SqlConnection(liparOptions.OutBoxEvent.ConnectionString);
            string query = string.Format(liparOptions.OutBoxEvent.SelectCommand, maxCount);
            var result = connection.Query<OutBoxEventItem>(query).ToList();
            return result;
        }
        public void MarkAsRead(List<OutBoxEventItem> outBoxEventItems)
        {
            string idForMark = string.Join(',', outBoxEventItems.Where(c => c.IsProcessed).Select(c => $"'{c.Id}'" ).ToList());
            if (!string.IsNullOrWhiteSpace(idForMark))
            {
                using var connection = new SqlConnection(liparOptions.OutBoxEvent.ConnectionString);
                string query = string.Format(liparOptions.OutBoxEvent.UpdateCommand, idForMark);
                connection.Execute(query);
            }
        }

    }
}
