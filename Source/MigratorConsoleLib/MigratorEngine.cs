using Migrator;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace MigratorConsoleLib
{
    public class MigratorEngine
    {
       
        #region Actions
        private const string _ACTION_MIGRATE = "Migrate";
        #endregion

        #region IO Vars
        private TextWriter _outputStream;
        private TextWriter _errorStream;
        #endregion

        #region Constructors
        public MigratorEngine()
        {
            _outputStream = System.Console.Out;
            _errorStream = System.Console.Error;
        }
        #endregion

        #region Main
        public int Main(string[] args)
        {
            if (!ValidateArguments(args))
            {
                _errorStream.WriteLine(GetUsage());

                return Exitcodes.InvalidArgs;
            }

            var connString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            var migrationFolder = ConfigurationManager.AppSettings["MigrationFolder"];
            var migrationTable = ConfigurationManager.AppSettings["VersionTable"];

            try
            {
                Migration mig = new Migration(connString, migrationTable);
                mig.Migrate(migrationFolder, AddResult);
            }
            catch (Exception ex)
            {
                _errorStream.WriteLine("Exception: " + ex.Message);
                _errorStream.WriteLine(ex.StackTrace);

                return Exitcodes.UnhandledExcpetion;
            }

            return Exitcodes.Success;
        }
        #endregion

        #region Results
        private void AddResult(string msg)
        {
            _outputStream.WriteLine(DateTime.Now.ToString() + " - " + msg);
        }
        #endregion

        #region Argument Handling
        public bool ValidateArguments(string[] args)
        {
            if (args.Count() != 1)
            {
                return false;
            }

            if (args[0].ToUpper() != _ACTION_MIGRATE.ToUpper())
            {
                return false;
            }

            // Arguments are valid.
            return true;
        }

        public string GetUsage()
        {
            var usage = "Usage: migrator-console <action>" + Environment.NewLine;
            usage += "action: Migrate" + Environment.NewLine;
            usage += Environment.NewLine;
            usage += "All the arguments are read from the application config file.";

            return usage;

        }
        #endregion
    }
}
