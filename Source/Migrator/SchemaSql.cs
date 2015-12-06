using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;


namespace Migrator
{
    public class SchemaSql : Schema
    {
        #region Constructors
        public SchemaSql(string connectionString)
            : base(connectionString)
        {
        }
        #endregion

        #region Create Schema
        /// <summary>
        /// Generates the Schema and writes it to the passed in file.
        /// </summary>
        /// <param name="schemaFile">The complete path and name for the file.</param>
        /// /// <param name="migrationTable">The table that contains the migrations to be scripted.  If blank then no migrations are scripted.</param>
        /// <param name="statusCallback">A function used to passback the status.</param>
        public virtual void WriteSchemaToFile(string schemaFile, string migrationTable, UpdateStatus statusCallback)
        {
            // So we can do let the caller know how things are going.
            UpdateStatusDelg = statusCallback;

            // Remove the previous schema file.
            StatusUpdate("Checking if schema file already exists.  The file is: " + schemaFile);
            if (File.Exists(schemaFile))
            {
                StatusUpdate("Deleting previous schema file...");
                File.Delete(schemaFile);
            }

            // Get the database connection.
            StatusUpdate("Establishing a connection the database with the name: " + Server.ConnectionContext.DatabaseName);
            var db = Server.Databases[Server.ConnectionContext.DatabaseName];


            // Settings for script generation.
            StatusUpdate("Creating scripter and setting options...");
            var scripter = new Scripter(Server)
            {
                Options =
                {
                    AppendToFile = true,
                    FileName = schemaFile,
                    Encoding = Encoding.Default,
                    DriAll = true,
                    DriAllConstraints = true,
                    DriIncludeSystemNames = true,
                    ScriptOwner = false,
                    IncludeIfNotExists = true,
                    AllowSystemObjects = false,
                    Indexes = true,
                    IncludeHeaders = false,
                    NoCollation = false,
                    Triggers = true,
                    WithDependencies = false
                }
            };



            // All the objects to be scripted.  Once we have them all
            // we will sort them in dependency order.
            var objectsToScript = new List<NamedSmoObject>();


            // User defined tables.
            StatusUpdate("Parsing user defined tables...");
            objectsToScript.AddRange(db.UserDefinedTableTypes.Cast<NamedSmoObject>());

            // Tables.
            StatusUpdate("Parsing tables...");
            objectsToScript.AddRange((from Table t in db.Tables where !t.IsSystemObject select t));

            // User defined functions.
            StatusUpdate("Parsing user defined funtions...");
            objectsToScript.AddRange((from UserDefinedFunction udf in db.UserDefinedFunctions where !udf.IsSystemObject select udf));

            // Views
            StatusUpdate("Parsing views...");
            objectsToScript.AddRange((from View v in db.Views where !v.IsSystemObject select v));

            // Procs
            StatusUpdate("Parsing procs...");
            objectsToScript.AddRange((from StoredProcedure sp in db.StoredProcedures where !sp.IsSystemObject select sp));


            // Sort the objects.  I had trouble with the SQL dependencies in SQL 2005
            // so instead of relying on SQL to figure out the dependencies I just
            // looked for the name.  This can result in some false positives but
            // that is not a bad thing.
            //
            // SQL 2008 appears to have better dependency tracking but I want this
            // script to work on all versions of SQL.
            //
            // Performance is acceptable for the databases I've tried it on but if you
            // have a large database and it's slow let me know and we can figure out
            // a faster algorithm.
            StatusUpdate("Determing dependencies...");
            OrderSqlDependencies(objectsToScript);


            // Script out the objects, should be in the correct order.
            scripter.Script(objectsToScript.Cast<SqlSmoObject>().ToArray());


            // If the migration table is used then write out the migrations.
            if (migrationTable != "")
            {
                File.AppendAllText(schemaFile, "-- Migrations --" + Environment.NewLine, Encoding.Default);

                var mig = new Migration(Server.ConnectionContext.ConnectionString, migrationTable);
                var migrationKeys = mig.GetAppliedMigrations();

                foreach (string migKey in migrationKeys)
                {
                    // Note: The scripter uses Unicode by default.
                    File.AppendAllText(schemaFile, "Insert Into " + migrationTable + " Values ('" + migKey + "');" + Environment.NewLine, Encoding.Default);
                }
            }
            else
            {
                StatusUpdate("Skipping migration scripting as not migration table was passed in.");
            }
        }

        private void OrderSqlDependencies(List<NamedSmoObject> listToOrder)
        {
            for (var i = 1; i < listToOrder.Count - 1; i++)
            {
                var dependencyList = GetViewDependencies(listToOrder[i].Name);

                var index = listToOrder.FindIndex(0, i, x => dependencyList.Contains(x.Name));
                if (index > 0)
                {
                    var itemToMove = listToOrder[i];

                    listToOrder.RemoveAt(i);
                    listToOrder.Insert(index, itemToMove);
                }
            }
        }

        /// <summary>
        /// Checks to see what other smo objects depending on the object passed in.
        /// </summary>
        /// <param name="smoObjectName">The object to check for dependencies.</param>
        /// <returns>A list of smo objects that depend on the object passed in.</returns>
        /// <remarks>
        /// In SQL 2005 the dependencies it builds are not reliable.  What little testing
        /// I've done with SQL 2008 shows that it is better but because I want this tool
        /// to work with both versions.
        /// </remarks>
        private List<string> GetViewDependencies(string smoObjectName)
        {
            // Check if the object exists in any other object.  This can result
            // in some false positive but I'd much rather have that then miss
            // a dependency.
            var sql = @"Select so.name
                        From syscomments sc
                        Inner Join sysobjects so ON sc.id = so.id 
                        Where text Like '%{0}%'
                        And so.Name <> '{0}'";

            sql = string.Format(sql, smoObjectName);

            var ds = Server.ConnectionContext.ExecuteWithResults(sql);

            // Add the dependencies to the list.
            var deps = (from DataRow dr in ds.Tables[0].Rows select dr["name"].ToString()).ToList();

            // All done.
            return deps;
        }
        #endregion
    }
}
