using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Migrator
{
    public class Migration
    {

        #region Vars
        public const string DEFAULT_SCHEMA_MIGRATIONS_TABLE = "SchemaMigrations";

        private string _migrationTable;
        private Server _server;
        #endregion

        #region Constructors
        public Migration(string connectionString) : this(connectionString, DEFAULT_SCHEMA_MIGRATIONS_TABLE)
        {
        }

        public Migration(string connectionString, string migrationTable)
        {
            // Open the database connection.  If we can't then just let the error float up.
            SqlConnection conn = new SqlConnection(connectionString);
            _server = new Server(new ServerConnection(conn));

            _migrationTable = migrationTable;
        }
        #endregion

        #region Properties
        public string MigraionTable
        {
            get { return _migrationTable; }
        }
        #endregion

        #region Migration
        /// <summary>
        /// Runs all the migrations found in the folder that haven't yet
        /// been applied to the database.
        /// </summary>
        ///<param name="migrationFolder">The folder containing all the migration files.</param>
        public void Migrate(string migrationFolder)
        {
            Migrate(migrationFolder, null);
        }

        /// <summary>
        /// Runs all the migrations found in the folder that haven't yet
        /// been applied to the database.
        /// </summary>
        /// <param name="migrationFolder">The folder containing all the migration files.</param>
        /// <param name="statusCallback">Used to update the status of the migration (i.e. what file is being
        /// migrated, has it been migrated already, etc).</param>
        /// <exception cref="DirectoryNotFoundException">If the migration folder isn't valid.</exception>
        public void Migrate(string migrationFolder, UpdateStatus statusCallback)
        {
            // So we can do let the caller know how things are going.
            _updateStatusDelg = statusCallback;

            StatusUpdate("Starting applying migrations");

            // Get all the migrations that have been ran already.
            StatusUpdate("Loading previously-applied migrations");
            var previousMigrations = GetAppliedMigrations(_migrationTable);
            StatusUpdate(string.Format("... {0} previously-applied migrations loaded", previousMigrations.Count));

            // Get all the files.
            StatusUpdate(string.Format("Loading migration files found at: {0}", migrationFolder));
            string[] allMigrationFiles = Directory.GetFiles(migrationFolder);
            StatusUpdate(string.Format("... {0} migration files loaded", allMigrationFiles.Length));

            // Run each migration, stopping if we have any errors.
            foreach (string migrationFile in allMigrationFiles)
            {
                StatusUpdate(string.Format("Applying migration from file: {0}", migrationFile));

                // The file migration version.
                string version = GetVersionFromFileName(migrationFile);
                StatusUpdate(string.Format("... version: {0}", version));

                // Has the migration been run already?
                if (!previousMigrations.Contains(version))
                {
                    // Run the migration.
                    string query = File.ReadAllText(migrationFile);
                    _server.ConnectionContext.ExecuteNonQuery(query);

                    // Update the migration table but only if the script hasn't yet.
                    int migrationTableUpdated = (int)_server.ConnectionContext.ExecuteScalar("Select Count(*) From " + _migrationTable + " Where Version = '" + version + "'");
                    if (migrationTableUpdated == 0)
                    {
                        _server.ConnectionContext.ExecuteScalar("Insert Into " + _migrationTable + " Values(" + version + ")");
                    }

                    StatusUpdate("... migration successfully applied");
                }
                else
                {
                    StatusUpdate("... migration was previously applied to database");
                }
            }

            // If the migration had any tables updated then any views that depend on that
            // table might need to be refreshed.
            RefreshAllViews(_server.ConnectionContext);


            StatusUpdate("Finished applying migrations");
        }
        #endregion

        #region Status Update
        public delegate void UpdateStatus(string status);
        private UpdateStatus _updateStatusDelg;

        private void StatusUpdate(string statusUpdate)
        {
            if (_updateStatusDelg != null)
            {
                _updateStatusDelg(statusUpdate);
            }
        }
        #endregion

        #region Create Migration File
        public static string CreateMigrationFile(string migrationFolder, string migrationDesc)
        {
            return CreateMigrationFile(migrationFolder, DEFAULT_SCHEMA_MIGRATIONS_TABLE, migrationDesc);
        }

        public static string CreateMigrationFile(string migrationFolder, string schemaTable, string migrationDesc)
        {
            var version = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
            var fileName = version + "_" + migrationDesc + ".sql";
            var fullPath = Path.Combine(migrationFolder, fileName);

            var versionUpdate = Environment.NewLine + Environment.NewLine;
            versionUpdate += "-- Place your code before the version update." + Environment.NewLine;
            versionUpdate += "Insert Into " + schemaTable + " Values(" + version + ")" + Environment.NewLine;
            File.WriteAllText(fullPath, versionUpdate);

            return fullPath;
        }
        #endregion

        #region Get Applied Migrations
        /// <summary>
        /// Gets a list of the applied migrations using the default migration table name.
        /// </summary>
        /// <returns>A list of the migrations that have been applied to this database.</returns>
        public List<string> GetAppliedMigrations()
        {
            return GetAppliedMigrations(DEFAULT_SCHEMA_MIGRATIONS_TABLE);
        }

        /// <summary>
        /// Gets a list of the applied migrations.
        /// </summary>
        /// <param name="migrationTable">The migration table.</param>
        /// <returns>A list of the migrations that have been applied to this database.</returns>
        public List<string> GetAppliedMigrations(string migrationTable)
        {
            var ds = _server.ConnectionContext.ExecuteWithResults("Select * From " + migrationTable);

            var migrations = new List<string>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                migrations.Add(dr[0].ToString());
            }

            return migrations;
        }
        #endregion

        #region Private Common Methods
        /// <summary>
        /// Given a migration file name will extract the version part.
        /// </summary>
        /// <param name="migrationFileName">The full migration file including the path (e.g. c:\Project Foo\Db\Migrations\20100213145632_CreateContactsTable.sql)</param>
        /// <returns>The version part of the file name.</returns>
        private string GetVersionFromFileName(string migrationFileName)
        {
            return Path.GetFileName(migrationFileName.Substring(0, migrationFileName.IndexOf("_")));
        }

        /// <summary>
        /// Refreshes all the views in the given database.
        /// </summary>
        /// <param name="conn">The database to refresh all the views for.</param>
        /// <remarks>
        /// By refresh I mean call sp_refreshview against any view that is not schema bound.
        /// Schema bound views can't be refreshed and will throw and error if you try to
        /// refresh them.
        /// </remarks>
        private void RefreshAllViews(ServerConnection conn)
        {
            // Find all the views that are not schema bound.
            string viewSql = @"Select Distinct name
                               From sys.objects so
                               Where type = 'V'
                               And ObjectProperty(object_id, 'IsSchemaBound') = 0";

            DataSet viewsDs = conn.ExecuteWithResults(viewSql);


            // Run refresh an each view.
            foreach (DataRow dr in viewsDs.Tables[0].Rows)
            {
                string refreshSql = @"Exec sp_refreshview '" + dr["name"] + "'";
                conn.ExecuteNonQuery(refreshSql);
            }
        }
        #endregion

    }
}
