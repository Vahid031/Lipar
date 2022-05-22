using Lipar.Core.Contract.Data;
using Lipar.Core.Contract.Utilities;
using Lipar.Core.Domain.Events;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Dapper;
using System.Linq;

namespace Lipar.Infrastructure.Data.SqlServer.EntityChangeInterceptor
{
    public class EntityChangeInterceptorRepository : IEntityChangesInterceptorRepository
    {
        private readonly LiparOptions liparOptions;
        private readonly IUserInfo userInfo;
        private readonly IDateTime dateTime;
        private readonly IJson json;

        public EntityChangeInterceptorRepository(LiparOptions liparOptions, IUserInfo userInfo, IDateTime dateTime, IJson json)
        {
            this.liparOptions = liparOptions;
            this.userInfo = userInfo;
            this.dateTime = dateTime;
            this.json = json;
        }
        public void AddEntityChanges(IEnumerable<EntityChangesInterception> entities)
        {
            using var connection = new SqlConnection(liparOptions.EntityChangesInterception.ConnectionString);
            var insertCommand = "Insert Into _EntityChangesInterceptions(Id, Date, State, EntityId, EntityType, UserId, Payload) Values(@Id, @Date, @State, @EntityId, @EntityType, @UserId, @Payload)";

            foreach (var entity in entities)
            {
                entity.SetDateTime(dateTime.DateTime);
                entity.SetUserId(userInfo.UserId);

                var details = new Dictionary<string, object>();

                foreach (var detail in entity.Details)
                    details.Add(detail.Key, detail.Value);

                connection.Execute(insertCommand, new
                {
                    entity.Id,
                    entity.Date,
                    entity.State,
                    entity.EntityId,
                    entity.EntityType,
                    entity.UserId,
                    Payload = json.SerializeObject(details)
                });

            }
        }
    }
}
