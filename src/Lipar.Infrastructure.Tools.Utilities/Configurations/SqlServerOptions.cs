namespace Lipar.Infrastructure.Tools.Utilities.Configurations;

public class SqlServerOptions
{
    public string ConnectionString { get; init; }
    public string TableName { get; init; }
    public string SchemaName { get; init; }
    public bool AutoCreateSqlTable { get; init; }
}


