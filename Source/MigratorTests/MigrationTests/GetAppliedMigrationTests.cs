using Migrator;
using NUnit.Framework;

namespace MigratorTests.MigrationTests
{
    class GetAppliedMigrationTests : TestBase
    {
        #region Vars
        private Migration _migration;
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

        #region Tests
        [Test]
        public void NoMigrations()
        {
            var migrations = _migration.GetAppliedMigrations();
            Assert.AreEqual(0, migrations.Count);
        }

        [Test]
        public void ThreeMigrations()
        {
            const string migrationOne = "000001";
            const string migrationTwo = "000002";
            const string migrationThree = "000003";

            _migratorDb.ExecuteNonQuery("Insert Into " + _migration.MigraionTable + " Values ('" + migrationOne + "')");
            _migratorDb.ExecuteNonQuery("Insert Into " + _migration.MigraionTable + " Values ('" + migrationTwo + "')");
            _migratorDb.ExecuteNonQuery("Insert Into " + _migration.MigraionTable + " Values ('" + migrationThree + "')");

            var migrations = _migration.GetAppliedMigrations();
            Assert.AreEqual(3, migrations.Count);

            Assert.IsTrue(migrations.Contains(migrationOne));
            Assert.IsTrue(migrations.Contains(migrationTwo));
            Assert.IsTrue(migrations.Contains(migrationThree));
        }
        #endregion

    }
}
