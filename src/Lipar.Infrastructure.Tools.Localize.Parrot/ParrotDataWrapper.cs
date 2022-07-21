using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Linq;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using System.Data.SqlClient;

namespace Zamin.Infra.Tools.Localizer.Parrot;

public class ParrotDataWrapper
{
    private readonly IDbConnection _dbConnection;
    private readonly List<LocalizationRecord> _localizationRecords;
    private readonly SqlServerOptions sqlServer;
    private readonly string SelectCommand = "Select * from [{0}].[{1}]";
    private readonly string InsertCommand = "INSERT INTO [{0}].[{1}]([Key],[Value],[Culture]) VALUES (@Key,@Value,@Culture) select SCOPE_IDENTITY()";
    private static ParrotDataWrapper _instance;
    public static ParrotDataWrapper CreateFactory(LiparOptions liparOptions)
    {
        if (_instance is null)
            _instance = new ParrotDataWrapper(liparOptions);

        return _instance;
    }
    private ParrotDataWrapper(LiparOptions liparOptions)
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
        $"TABLE_SCHEMA = '{sqlServer.SchemaName}' AND  TABLE_NAME = '{ sqlServer.TableName}' )) Begin " +
        $"CREATE TABLE [{sqlServer.SchemaName}].[{ sqlServer.TableName}]( " +
        $"[Id] [int] IDENTITY(1,1) NOT NULL Primary Key," +
        $"[Key] [nvarchar](255) NOT NULL," +
        $"[Value] [nvarchar](500) NOT NULL," +
        $"[Culture] [nvarchar](5) NULL)" +
        $" End";

        _dbConnection.Execute(createTable);
    }

    public string Get(string key, string culture)
    {
        var record = _localizationRecords.FirstOrDefault(c => c.Key == key && c.Culture == culture);
        if (record == null)
        {
            record = new LocalizationRecord
            {
                Key = key,
                Culture = culture,
                Value = key
            };

            var parameters = new DynamicParameters();
            parameters.Add("@Key", key);
            parameters.Add("@Culture", culture);
            parameters.Add("@Value", key);

            record.Id = _dbConnection.Query<int>(InsertCommand, param: parameters, commandType: CommandType.Text).FirstOrDefault();
            _localizationRecords.Add(record);
        }
        return record.Value;
    }

}


