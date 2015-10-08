using MigratorConsoleLib;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigratorConsoleLibTests.MigratorEngineTests
{
    class TestBase
    {
        protected MigratorEngine _migEngine;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _migEngine = new MigratorEngine();
        }
    }
}
