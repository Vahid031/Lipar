﻿using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Linq;
using Lipar.Infrastructure.Tools.Utilities.Configurations;
using System.Data.SqlClient;

namespace Zamin.Infra.Tools.Localizer.Parrot
{
    public class ParrotDataWrapper
    {
        private readonly IDbConnection _dbConnection;
        private readonly List<LocalizationRecord> _localizationRecords;
        private readonly LiparOptions _liparOptions;
        private const string SelectCommand = "Select * from [{0}].[{1}]";
        private const string InsertCommand = "INSERT INTO [{0}].[{1}]([Key],[Value],[Culture]) VALUES (@Key,@Value,@Culture) select SCOPE_IDENTITY()";


        public ParrotDataWrapper(LiparOptions liparOptions)
        {
            _liparOptions = liparOptions;
            _dbConnection = new SqlConnection(liparOptions.Translation.ConnectionString);

            if (_liparOptions.Translation.AutoCreateSqlTable)
                CreateTableIfNeeded();
            
            _localizationRecords = _dbConnection.Query<LocalizationRecord>(SelectCommand, commandType: CommandType.Text).ToList();
        }

        private void CreateTableIfNeeded()
        {
            string table = _liparOptions.Translation.TableName;
            string schema = _liparOptions.Translation.SchemaName;
            string createTable = $"IF (NOT EXISTS (SELECT *  FROM INFORMATION_SCHEMA.TABLES WHERE " +
                $"TABLE_SCHEMA = '{schema}' AND  TABLE_NAME = '{table}' )) Begin " +
                $"CREATE TABLE [{schema}].[{table}]( " +
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
}