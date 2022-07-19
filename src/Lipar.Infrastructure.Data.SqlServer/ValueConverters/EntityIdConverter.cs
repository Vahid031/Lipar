using Lipar.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace Lipar.Infrastructure.Data.SqlServer.ValueConverters;

public class EntityIdConverter : ValueConverter<EntityId, Guid>
{
    public EntityIdConverter() : base(c => c.Value, d => EntityId.FromGuid(d))
    {
    }
}
