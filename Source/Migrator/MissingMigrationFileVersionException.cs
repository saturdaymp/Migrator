using System;

namespace Migrator
{
    /// <summary>
    /// Exception is raised if the version number of the migration file can't be deteremined.
    /// </summary>
    public class MissingMigrationFileVersionException : Exception
    {
        /// <summary>
        /// Exception with the migration file missing the version.
        /// </summary>
        /// <param name="migrationFileName">The migartion file name that is missing the version.</param>
        public MissingMigrationFileVersionException(string migrationFileName)
            : base("Unable to determine the version for migration file '" + migrationFileName + "'.")
        {
        }
    }
}
