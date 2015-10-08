using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        public override void WriteSchemaToFile(string schemaFile, string migrationTable, UpdateStatus statusCallback)
        {
            // So we can do let the caller know how things are going.
            _updateStatusDelg = statusCallback;

            // Remove the previous schema file.
            StatusUpdate("Checking if schema file already exists.  The file is: " + schemaFile);
            if (File.Exists(schemaFile))
            {
                StatusUpdate("Deleting previous schema file...");
                File.Delete(schemaFile);
            }

            // Get the database connection.
            StatusUpdate("Establishing a connection the database with the name: " + _server.ConnectionContext.DatabaseName);
            var db = _server.Databases[_server.ConnectionContext.DatabaseName];


            // Settings for script generation.
            StatusUpdate("Creating scripter and setting options...");
            Scripter scripter = new Scripter(_server);
            scripter.Options.ScriptOwner = false;
            scripter.Options.Triggers = true;
            scripter.Options.DriAll = true;
            scripter.Options.WithDependencies = false;
            scripter.Options.IncludeIfNotExists = true;
            scripter.Options.Indexes = true;
            scripter.Options.IncludeHeaders = false;
            scripter.Options.AppendToFile = true;
            scripter.Options.FileName = schemaFile;


            // Script the tables.  As far as I can tell the tables
            // are always scripted in the correct order to prevent errors
            // with foregin keys.
            StatusUpdate("Scripting Tables...");
            var tables = new Table[db.Tables.Count];
            db.Tables.CopyTo(tables, 0);
            scripter.Script(tables);


            // Script the UDFs
            StatusUpdate("Scripting UDFs...");
            var udfs = new List<NamedSmoObject>();
            foreach (UserDefinedFunction u in db.UserDefinedFunctions)
            {
                if (!u.IsSystemObject)
                {
                    AddSmoObjectToList(udfs, u);
                }
            }
            scripter.Script(udfs.ToArray());


            // script the views.
            StatusUpdate("Scripting Views...");
            var views = new List<NamedSmoObject>();
            foreach (View v in db.Views)
            {
                if (!v.IsSystemObject)
                {
                    AddSmoObjectToList(views, v);
                }
            }
            scripter.Script(views.ToArray());


            // Procs
            StatusUpdate("Scripting Procs...");
            var procs = new List<NamedSmoObject>();
            foreach (StoredProcedure p in db.StoredProcedures)
            {
                if (!p.IsSystemObject)
                {
                    AddSmoObjectToList(procs, p);
                }
            }
            scripter.Script(procs.ToArray());


            // If the migration table is used then write out the migrations.
            if (migrationTable != "")
            {
                File.AppendAllText(schemaFile, "-- Migrations --" + Environment.NewLine, Encoding.Unicode);

                Migration mig = new Migration(_server.ConnectionContext.ConnectionString, migrationTable);
                var migrationKeys = mig.GetAppliedMigrations();

                foreach (string migKey in migrationKeys)
                {
                    // Note: The scripter uses Unicode by default.
                    File.AppendAllText(schemaFile, "Insert Into " + migrationTable + " Values ('" + migKey + "');" + Environment.NewLine, Encoding.Unicode);
                }
            }
            else
            {
                StatusUpdate("Skipping migration scripting as not migration table was passed in.");
            }
        }

        /// <summary>
        /// Adds a smo object (i.e. view, stored proc, etc) to the list in the correct spot
        /// depending on it's dependencies on other objects.
        /// </summary>
        /// <param name="smoList">The list to add the object too.</param>
        /// <param name="objectToAdd">The object add.</param>
        private void AddSmoObjectToList(List<NamedSmoObject> smoList, NamedSmoObject objectToAdd)
        {
            if (!smoList.Exists(x => x.Name == objectToAdd.Name))
            {
                List<string> viewDeps = GetViewDependencies(objectToAdd.Name);

                int index = smoList.FindIndex(x => viewDeps.Contains(x.Name));
                if (index > 0)
                {
                    smoList.Insert(index, objectToAdd);
                }
                else
                {
                    smoList.Add(objectToAdd);
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

            var ds = _server.ConnectionContext.ExecuteWithResults(sql);

            // Add the dependencies to the list.
            var deps = new List<string>();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                deps.Add(dr["name"].ToString());
            }

            // All done.
            return deps;
        }
        #endregion
    }
}
