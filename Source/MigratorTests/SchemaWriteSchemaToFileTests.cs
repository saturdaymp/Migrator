using Migrator;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MigratorTests
{
    [TestFixture]
    class SchemaWriteSchemaToFileTests : TestBase
    {
        [TestFixtureSetUp]
        public override void TestFixtureSetUp()
        {
            base.TestFixtureSetUp();

            // Create a database with some tables, views, funcitons, and procs.
            // The test data should have a couple views that depend on each other.
            _migratorDb.ExecuteNonQuery(File.ReadAllText("..\\..\\Schema.sql"));
        }

        [Test]
        public void WithMigrationInserts()
        {
            Success("SchemaMigrations");
        }

        /// <summary>
        /// Make sure the schema written to the file is correct without scripting
        /// the migration inserts.
        /// </summary>
        [Test]
        public void WithoutMigrationInserts()
        {
            Success("");
        }

        /// <summary>
        /// A test for the successful schema write.
        /// </summary>
        /// <param name="migrationTable">If not empty then assumes the result should have the
        /// migration inserts, otherwise assumes the migration inserts are not scripted.</param>
        /// <remarks>
        /// The logic for testing the schema writting, with or without the migration inserts, is almost
        /// idendical hence it all lives here.
        /// </remarks>
        private void Success(string migrationTable)
        {
            // Clean-up the old file.
            var resultSchemaFile = Path.Combine(Path.GetTempPath(), "ResultSchema.sql");
            if (File.Exists(resultSchemaFile))
            {
                File.Delete(resultSchemaFile);
            }


            // Generate the schema.
            var schema = new SchemaSql(_connString);
            schema.WriteSchemaToFile(resultSchemaFile, migrationTable, null);


            // Does the file exist?
            Assert.IsTrue(File.Exists(resultSchemaFile));

            // Read in the results.
            var resultSchemaText = File.ReadAllText(resultSchemaFile);


            // Get the intial draft of the expected test.  Maybe a little bit of 
            // a cheat as we assume that the generated schema file matches the 
            // file that was used to initially create the database with a couple
            // date sensative items removed.
            var expectedSchemaText = File.ReadAllText("..\\..\\Schema.sql");

            // If the migration table is empty then the inserts should NOT be scripted so remove that
            // from the expected text.
            if (migrationTable == "")
            {
                expectedSchemaText = expectedSchemaText.Substring(0, expectedSchemaText.IndexOf("-- Migrations"));
            }

            // Is it what we expect?  
            Assert.AreEqual(expectedSchemaText, resultSchemaText);
        }


    }
}
