namespace Lipar.Infrastructure.Tools.Utilities.Configurations;

public class OutBoxEventOptions
{
    public string TypeName { get; init; }
    public SqlServerOptions SqlServer { get; set; }
}



