namespace Lipar.Infrastructure.Tools.Utilities.Configurations;

public class InBoxEventOptions
{
    public string TypeName { get; init; }
    public SqlServerOptions SqlServer { get; init; }
    public MongoDbOptions MongoDb { get; init; }
}

