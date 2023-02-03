using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Linq;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using System.Data.SqlClient;
using System;

namespace Lipar.Infrastructure.Tools.Localize.Parrot.SqlServer;

public class SqlServerParrotDataWrapper : ParrotDataWrapper
{
    private readonly IDbConnection _dbConnection;
    private readonly List<LocalizationRecord> _localizationRecords;
    private readonly SqlServerOptions sqlServer;
    private readonly string SelectCommand = "Select * from [{0}].[{1}]";
    private readonly string InsertCommand = "INSERT INTO [{0}].[{1}]([Id],[CreateDate],[Key],[Value],[Culture]) VALUES (@Id,Getdate(),@Key,@Value,@Culture)";
    private static ParrotDataWrapper _instance;
    public static ParrotDataWrapper CreateFactory(LiparOptions liparOptions)
    {
        if (_instance is null)
            _instance = new SqlServerParrotDataWrapper(liparOptions);

        return _instance;
    }
    private SqlServerParrotDataWrapper(LiparOptions liparOptions)
    {
        sqlServer = liparOptions.Translation.SqlServer;
        _dbConnection = new SqlConnection(sqlServer.ConnectionString);

        if (sqlServer.AutoCreateSqlTable)
            CreateTableIfNeeded();

        SelectCommand = string.Format(SelectCommand, sqlServer.SchemaName, sqlServer.TableName);
        InsertCommand = string.Format(InsertCommand, sqlServer.SchemaName, sqlServer.TableName);

        _localizationRecords = _dbConnection.Query<LocalizationRecord>(SelectCommand, commandType: CommandType.Text).ToList();
    }

    private void CreateTableIfNeeded()
    {
        string createTable = $"IF (NOT EXISTS (SELECT *  FROM INFORMATION_SCHEMA.TABLES WHERE " +
        $"TABLE_SCHEMA = '{sqlServer.SchemaName}' AND  TABLE_NAME = '{sqlServer.TableName}' )) Begin " +
        $"CREATE TABLE [{sqlServer.SchemaName}].[{sqlServer.TableName}]( " +
        $"[Id] [uniqueidentifier] NOT NULL, " +
        $"[CreateDate] [datetime] NOT NULL, " +
        $"[Key] [nvarchar](255) NOT NULL, " +
        $"[Value] [nvarchar](500) NOT NULL, " +
        $"[Culture] [nvarchar](5) NOT NULL, " +
        $"CONSTRAINT [PK_'{sqlServer.SchemaName}'] PRIMARY KEY NONCLUSTERED " +
        $"([Id] ASC)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]" +
        $") ON [PRIMARY] " +
        $" End";

        _dbConnection.Execute(createTable);
    }

    public override string Get(string key, string culture)
    {
        var record = _localizationRecords.FirstOrDefault(c => c.Key == key && c.Culture == culture);
        if (record == null)
        {
            record = new LocalizationRecord
            {
                Id = Guid.NewGuid(),
                Key = key,
                Culture = culture,
                Value = key
            };

            _dbConnection.Execute(InsertCommand, param: record, commandType: CommandType.Text);
            _localizationRecords.Add(record);
        }
        return record.Value;
    }

    public void Dispose()
    {
        throw new System.NotImplementedException();
    }
}


