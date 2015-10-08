using MigratorConsoleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigratorConsole
{
    class Program
    {
        #region Main
        static int Main(string[] args)
        {
            var me = new MigratorEngine();
            return me.Main(args);
        }
        #endregion
    }
}
