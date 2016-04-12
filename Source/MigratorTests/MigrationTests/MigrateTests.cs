using Microsoft.SqlServer.Management.Smo;
using Migrator;
using NUnit.Framework;
using System;
using System.Data;
using System.IO;

namespace MigratorTests.MigrationTests
{
    [TestFixture]
    class MigrateTests : TestBase
    {
        #region Vars
        private Migration _migration;

        private string _baseDirecotry = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MigrationTests");
        #endregion

        #region SetUp/TearDown
        [SetUp]
        public void SetUp()
        {
            // Clear up the database.
            ResetDatabase();

            // Initi the migration table.
            CreateMigrationTable(_migratorDb);

            // The migration class used by most of the tests.
            _migration = new Migration(_connString);
        }
        #endregion

        #region Migration Tests
        /// <summary>
        /// Simulate running the migration on a new database without any
        /// migrations having been ran yet.
        /// </summary>
        [Test]
        public void NewDb()
        {

            // Run the migrations.
            _migration.Migrate(Path.Combine(_baseDirecotry, "NewDbMigrations"));

            // Two tables, the schema table and the person table.            
            Assert.AreEqual(2, _migratorDb.Tables.Count);

            // Person table created and updated?
            Table contactsTable = _migratorDb.Tables["Contacts"];
            Assert.IsTrue(contactsTable.Columns.Contains("Id"));
            Assert.IsTrue(contactsTable.Columns.Contains("FirstName"));
            Assert.IsTrue(contactsTable.Columns.Contains("LastName"));

            // Version table updated?  Should have the two migrations ran in it.
            DataSet migrations = _migratorDb.ExecuteWithResults("Select * From SchemaMigrations Order By Version");
            Assert.AreEqual(2, migrations.Tables[0].Rows.Count);
            Assert.AreEqual("20100213145632", migrations.Tables[0].Rows[0]["Version"]);
            Assert.AreEqual("20100215091324", migrations.Tables[0].Rows[1]["Version"]);
        }

        /// <summary>
        /// Database that has some migrations ran against it already.  Want to make sure
        /// a migration isn't ran twice and we don't miss one.
        /// </summary>
        [Test]
        public void ExistingDb()
        {
            string migrationFolder = Path.Combine(_baseDirecotry, "ExistingDbMigrations");

            // Setup the DB so that some of the migrations have been ran.
            string createContactsTableSql = File.ReadAllText(migrationFolder + "\\20100213145632_CreateContactsTable.sql");
            _migratorDb.ExecuteNonQuery(createContactsTableSql);
            _migratorDb.ExecuteNonQuery("Insert Into " + Migration.DefaultSchemaMigrationsTable + " Values ('20100213145632')");

            string createAddressesTableSql = File.ReadAllText(migrationFolder + "\\20100328124805_AddAddressesToContactsTable.sql");
            _migratorDb.ExecuteNonQuery(createAddressesTableSql);
            _migratorDb.ExecuteNonQuery("Insert Into " + Migration.DefaultSchemaMigrationsTable + " Values ('20100328124805')");

            string createContactsViewSql = File.ReadAllText(migrationFolder + "\\20100215092393_CreateContactsView.sql");
            _migratorDb.ExecuteNonQuery(createContactsViewSql);
            _migratorDb.ExecuteNonQuery("Insert Into " + Migration.DefaultSchemaMigrationsTable + " Values ('20100215092393')");


            // Run the migrations.
            _migration.Migrate(Path.Combine(_baseDirecotry, "ExistingDbMigrations"));

            // Were the two updates applied?
            Table contactsTable = _migratorDb.Tables["Contacts"];
            Assert.IsTrue(contactsTable.Columns.Contains("LastName"));
            Assert.IsTrue(contactsTable.Columns.Contains("AddressId"));

            // Version table has the two updates in it?
            DataSet migrations = _migratorDb.ExecuteWithResults("Select * From SchemaMigrations Order By Version");
            Assert.AreEqual(5, migrations.Tables[0].Rows.Count);
            Assert.AreEqual("20100213145632", migrations.Tables[0].Rows[0]["Version"]);
            Assert.AreEqual("20100215091324", migrations.Tables[0].Rows[1]["Version"]);
            Assert.AreEqual("20100215092393", migrations.Tables[0].Rows[2]["Version"]);
            Assert.AreEqual("20100328123112", migrations.Tables[0].Rows[3]["Version"]);
            Assert.AreEqual("20100328124805", migrations.Tables[0].Rows[4]["Version"]);

            // Were the views refreshed?  If the views aren't refreshed then
            // the contacts view won't have the LastName field.
            View contactsView = _migratorDb.Views["ContactsView"];
            Assert.IsTrue(contactsView.Columns.Contains("LastName"));
        }

        /// <summary>
        /// What happens if an invalid migration folder is passed in?
        /// </summary>
        [Test]
        public void InvalidFolder()
        {
            Assert.Throws<DirectoryNotFoundException>(() => _migration.Migrate(Path.Combine(_baseDirecotry, "Blahdalsjhdflkjs")));
        }

        /// <summary>
        /// Check that the correct excpetion is thrown if the version of the migration
        /// file can't be determined.
        /// </summary>
        [Test]
        public void MissingVersion()
        {
            Assert.Throws<MissingMigrationFileVersionException>(() => _migration.Migrate(Path.Combine(_baseDirecotry, "MissingVersion")));
        }

        #endregion

        #region Create Migration Tests
        /// <summary>
        /// Migration file created without errors.
        /// </summary>
        [Test]
        public void CreateMigrationFile()
        {
            string migrationFolder = Path.GetTempPath();
            Console.WriteLine(migrationFolder);

            string newFile = Migration.CreateMigrationFile(migrationFolder, "NUnitTest");

            Assert.IsTrue(File.Exists(newFile));
        }
        #endregion
    }
}
