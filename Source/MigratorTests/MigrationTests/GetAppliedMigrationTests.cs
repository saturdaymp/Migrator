using Migrator;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            const string MIGRATION_ONE = "000001";
            const string MIGRATION_TWO = "000002";
            const string MIGRATION_THREE = "000003";

            _migratorDb.ExecuteNonQuery("Insert Into " + _migration.MigraionTable + " Values ('" + MIGRATION_ONE + "')");
            _migratorDb.ExecuteNonQuery("Insert Into " + _migration.MigraionTable + " Values ('" + MIGRATION_TWO + "')");
            _migratorDb.ExecuteNonQuery("Insert Into " + _migration.MigraionTable + " Values ('" + MIGRATION_THREE + "')");

            var migrations = _migration.GetAppliedMigrations();
            Assert.AreEqual(3, migrations.Count);

            Assert.IsTrue(migrations.Contains(MIGRATION_ONE));
            Assert.IsTrue(migrations.Contains(MIGRATION_TWO));
            Assert.IsTrue(migrations.Contains(MIGRATION_THREE));
        }
        #endregion

    }
}
