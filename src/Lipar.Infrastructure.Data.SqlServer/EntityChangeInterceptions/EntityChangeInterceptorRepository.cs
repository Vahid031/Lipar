using Lipar.Core.Contract.Data;
using Lipar.Core.Contract.Services;
using Lipar.Core.Domain.Events;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Dapper;

namespace Lipar.Infrastructure.Data.SqlServer.EntityChangeInterceptor;

public class EntityChangeInterceptorRepository : IEntityChangesInterceptorRepository
{
    private readonly LiparOptions liparOptions;
    private readonly IUserInfoService userInfoService;
    private readonly IDateTimeService dateTimeService;
    private readonly IJsonService jsonService;
    
    public EntityChangeInterceptorRepository(LiparOptions liparOptions, IUserInfoService userInfoService, IDateTimeService dateTimeService, IJsonService jsonService)
    {
        this.liparOptions = liparOptions;
        this.userInfoService = userInfoService;
        this.dateTimeService = dateTimeService;
        this.jsonService = jsonService;
    }
    public void AddEntityChanges(IEnumerable<EntityChangesInterception> entities)
    {
        using var connection = new SqlConnection(liparOptions.EntityChangesInterception.ConnectionString);
        var insertCommand = "Insert Into _EntityChangesInterceptions(Id, Date, State, EntityId, EntityType, UserId, Payload) Values(@Id, @Date, @State, @EntityId, @EntityType, @UserId, @Payload)";
        
        foreach (var entity in entities)
        {
            entity.SetDateTime(dateTimeService.Now);
            entity.SetUserId(userInfoService.UserId);
            
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
                Payload = jsonService.SerializeObject(details)
            });
            
        }
    }
}


