using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MigratorTests
{
    class TestBase
    {
        #region Vars
        protected string _connString;
        protected Database _migratorDb;

        private string _masterConnString;
        #endregion

        #region Fixture SetUp/TearDown
        [TestFixtureSetUp]
        public virtual void TestFixtureSetUp()
        {
            // Connection string for master database, used when dropping and recreating the
            // Migrator database.
            _masterConnString = ConfigurationManager.ConnectionStrings["Master"].ConnectionString;

            // Connection string for the migrator database.
            _connString = ConfigurationManager.ConnectionStrings["Migrator"].ConnectionString;

            // Establish a connection to the database.
            ResetDatabase();
        }
        #endregion

        #region Database Reset
        /// <summary>
        /// Drops and recreates the database referenced in the initial catalog section of
        /// the _connString.
        /// </summary>
        /// <returns>The created database.</returns>
        protected void ResetDatabase()
        {
            _migratorDb = ResetDatabase(new SqlConnectionStringBuilder(_connString).InitialCatalog);
        }

        /// <summary>
        /// Drops and re-creates the given database.
        /// </summary>
        /// <returns>The created database.</returns>
        protected Database ResetDatabase(string dbName)
        {
            // Connect to the server, using the master database as our default.
            var masterConnString = ConfigurationManager.ConnectionStrings["Master"].ConnectionString;
            var masterServer = new Server(new ServerConnection(new SqlConnection(_masterConnString)));

            // See if the database exists already, if it dose then drop it.
            if (masterServer.Databases.Contains(dbName))
            {
                masterServer.KillDatabase(dbName);
            }

            // Create the database.
            var testDb = new Database(masterServer, dbName);
            testDb.Create();
            testDb.SetOnline();

            // Prevents "no process is on the other end of the pipe" errors from randomly occuring.
            // They happen because the test database that was killed and re-created might still have
            // an old but invalid connection in the connection pool.  Clear the pool to prevent the
            // stale connections from being used.
            SqlConnection.ClearAllPools();

            // All done
            return testDb;
        }

        protected void CreateMigrationTable(Database db)
        {
            // Create the version table.
            string createMigrationTableSql = @"CREATE TABLE SchemaMigrations
                                             (
                                             Version varchar(50) NOT NULL
                                             )";

            db.ExecuteNonQuery(createMigrationTableSql);

        }
        #endregion
    }
}
