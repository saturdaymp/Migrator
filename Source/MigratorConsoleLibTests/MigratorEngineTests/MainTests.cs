using MigratorConsoleLib;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigratorConsoleLibTests.MigratorEngineTests
{
    [TestFixture]
    class MainTests : TestBase
    {
        /// <summary>
        /// Make sure the correct error code is returned.
        /// </summary>
        [Test]
        public void InvalidArgs()
        {
            Assert.AreEqual(Exitcodes.InvalidArgs, _migEngine.Main(new string[] { }));
        }

    }
}
