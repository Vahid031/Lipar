using Microsoft.Data.SqlClient;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using Lipar.Infrastructure.Tools.Utilities.Configurations;

namespace Lipar.Infrastructure.Data.SqlServer.OutBoxEvents
{
    public class SqlOutBoxEventItemRepository : IOutBoxEventItemRepository
    {
        private readonly LiparOptions liparOptions;

        public SqlOutBoxEventItemRepository(IOptions<LiparOptions> options)
        {
            this.liparOptions = options.Value;
        }

        public List<OutBoxEventItem> GetOutBoxEventItemsForPublishe(int maxCount = 100)
        {
            using var connection = new SqlConnection(liparOptions.OutBoxEvent.ConnectionString);
            string query = string.Format(liparOptions.OutBoxEvent.SelectCommand, maxCount);
            var result = connection.Query<OutBoxEventItem>(query).ToList();
            return result;
        }
        public void MarkAsRead(List<OutBoxEventItem> outBoxEventItems)
        {
            string idForMark = string.Join(',', outBoxEventItems.Where(c => c.IsProcessed).Select(c => c.OutBoxEventItemId).ToList());
            if (!string.IsNullOrWhiteSpace(idForMark))
            {
                using var connection = new SqlConnection(liparOptions.OutBoxEvent.ConnectionString);
                string query = string.Format(liparOptions.OutBoxEvent.UpdateCommand, idForMark);
                connection.Execute(query);
            }
        }

    }
}
