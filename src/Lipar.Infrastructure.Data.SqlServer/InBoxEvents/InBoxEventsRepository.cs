using Dapper;
using Lipar.Core.Contract.Data;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Lipar.Infrastructure.Data.SqlServer.InBoxEvents
{
    public class InBoxEventsRepository : IInBoxEventRepository
    {
       private readonly string _connectionString;
        public InBoxEventsRepository(LiparOptions liparOptions)
        {
            _connectionString = liparOptions.OutBoxEvent.ConnectionString;
        }

        public bool AllowReceive(string messageId, string fromService)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "Select Id from [_InBoxEvents] Where [OwnerService] = @OwnerService and [MessageId] = @MessageId";
            var result = connection.Query<long>(query, new
            {
                OwnerService = fromService,
                MessageId = messageId
            }).FirstOrDefault();
            return result < 1;
        }

        public async Task Receive(string messageId, string fromService)
        {
            using var connection = new SqlConnection(_connectionString);
            string query = "Insert Into [_InBoxEvents] ([OwnerService] ,[MessageId] ) values(@OwnerService,@MessageId)";
            await connection.ExecuteAsync(query, new
            {
                OwnerService = fromService,
                MessageId = messageId
            });
        }
    }
}
