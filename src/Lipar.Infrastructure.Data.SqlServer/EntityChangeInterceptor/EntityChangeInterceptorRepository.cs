using Lipar.Core.Contract.Data;
using Lipar.Core.Contract.Utilities;
using Lipar.Core.Domain.Events;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Dapper;

namespace Lipar.Infrastructure.Data.SqlServer.EntityChangeInterceptor
{
    public class EntityChangeInterceptorRepository : IEntityChangesInterceptorRepository
    {
        private readonly LiparOptions liparOptions;
        private readonly IUserInfo userInfo;
        private readonly IDateTime dateTime;

        public EntityChangeInterceptorRepository(LiparOptions liparOptions, IUserInfo userInfo, IDateTime dateTime)
        {
            this.liparOptions = liparOptions;
            this.userInfo = userInfo;
            this.dateTime = dateTime;
        }
        public void AddEntityChanges(IEnumerable<EntityChangesInterceptor> entities)
        {
            using var connection = new SqlConnection(liparOptions.EntityChangesInterceptor.ConnectionString);
            var insertCommand = "Insert Into dbo.EntityChangesInterceptors(Id, Date, State, EntityId, EntityType, UserId) Values(@Id, @Date, @State, @EntityId, @EntityType, @UserId)";
            var insertDetailCommand = "Insert Into dbo.EntityChangesInterceptorDetails(Id, [Key], [Value], EntityChangesInterceptorId) Values(@Id, @Key, @Value, @EntityChangesInterceptorId)";

            foreach (var entity in entities)
            {
                entity.SetDateTime(dateTime.DateTime);
                entity.SetUserId(userInfo.UserId);

                connection.Execute(insertCommand, new
                {
                    entity.Id,
                    entity.Date,
                    entity.State,
                    entity.EntityId,
                    entity.EntityType,
                    entity.UserId,
                });

                foreach (var detail in entity.EntityChangesInterceptorDetails)
                {
                    connection.Execute(insertDetailCommand, new
                    {
                        detail.Id,
                        detail.Key,
                        detail.Value,
                        detail.EntityChangesInterceptorId
                    });
                }
            }
        }
    }
}
