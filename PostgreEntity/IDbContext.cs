using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NServiceKit.OrmLite;
using NServiceKit.OrmLite.PostgreSQL;

namespace PostgreEntity
{
    internal  abstract class IDbContext
    {

        private OrmLiteConnectionFactory _connection;
        public  IDbConnection Db { get; set; }

        public IDbContext(string connectionString)
        {
            _connection =
                new OrmLiteConnectionFactory(connectionString, true, PostgreSqlDialect.Provider);
            Db = _connection.OpenDbConnection();
        }

      
    }
}
