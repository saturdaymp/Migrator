using MigratorConsoleLib;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigratorConsoleLibTests.MigratorEngineTests
{
    [TestFixture]
    class ValidateArgumentsTests : TestBase
    {
        /// <summary>
        /// Successful validate test.
        /// </summary>
        [Test]
        public void ValidAction()
        {
            string[] args = { "Migrate" };

            Assert.IsTrue(_migEngine.ValidateArguments(args));
        }

        /// <summary>
        /// Invalid action.
        /// </summary>
        [Test]
        public void InvalidAction()
        {
            string[] args = { "InvalidAction" };

            Assert.IsFalse(_migEngine.ValidateArguments(args));
        }

        /// <summary>
        /// Should error out if no arguments are passed in.
        /// </summary>
        [Test]
        public void ZeroArguments()
        {
            Assert.IsFalse(_migEngine.ValidateArguments(new string[] { }));
        }

        /// <summary>
        /// More then expected amount of arguments.
        /// </summary>
        [Test]
        public void TooManyArguments()
        {
            string[] args = { "Migrate", "Argument2" };

            Assert.IsFalse(_migEngine.ValidateArguments(new string[] { }));
        }
    }
}
