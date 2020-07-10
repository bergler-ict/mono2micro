using Monolith.Data.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Monolith.Data
{
    public static class Database
    {
        public static int ExecuteSql(string sql, object parameters)
        {
            using (var connection = new SqlConnection(Settings.Default.DatabaseConnString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                if (parameters != null)
                {
                    command.Parameters.AddRange(getSqlParametersFromObject(parameters).ToArray());
                }
                return command.ExecuteNonQuery();
            }
        }
        
        public static IEnumerable<T> ExecuteSql<T>(string sql, Func<IDataRecord, T> createObjectFunc)
        {
            return ExecuteSql<T>(sql, null, createObjectFunc);
        }

        public static IEnumerable<T> ExecuteSql<T>(string sql, object parameters, Func<IDataRecord, T> createObjectFunc)
        {
            using (var connection = new SqlConnection(Settings.Default.DatabaseConnString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                if (parameters != null)
                {
                    command.Parameters.AddRange(getSqlParametersFromObject(parameters).ToArray());
                }

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    yield return createObjectFunc(reader);
                }
            }
        }

        private static IEnumerable<SqlParameter> getSqlParametersFromObject(object obj)
        {
            foreach (var p in obj
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(pi => pi.CanRead))
            {
                yield return new SqlParameter(p.Name, p.GetValue(obj));
            }
        }
    }
}
