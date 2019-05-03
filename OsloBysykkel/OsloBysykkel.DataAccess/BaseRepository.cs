using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace OsloBysykkel.DataAccess
{
    public abstract class BaseRepository
    {
        private static string _connectionString = null;
        public static string ConnectionString => _connectionString ??
                                                 (_connectionString = ConfigurationManager.ConnectionStrings["ObDatabase"].ConnectionString);

        protected async Task<T> WithDatabaseConnection<T>(Func<IDbConnection, Task<T>> getData)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                await connection.OpenAsync();
                return await getData(connection);
            }
        }
        
        protected static string StringOrNull(string s)
        {
            return s == null
                ? "NULL"
                : $"'{s}'";
        }

        protected static string IntOrNull(int? i)
        {
            return i.HasValue
                ? i.Value.ToString()
                : "NULL";
        }
    }
}
