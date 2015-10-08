using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Migrator
{
    public abstract class Schema
    {
        #region Vars
        protected Server _server;
        #endregion

        #region Status Update
        public delegate void UpdateStatus(string status);
        protected UpdateStatus _updateStatusDelg;

        protected void StatusUpdate(string statusUpdate)
        {
            if (_updateStatusDelg != null)
            {
                _updateStatusDelg(statusUpdate);
            }
        }
        #endregion

        #region Constructors
        public Schema(string connectionString)
        {
            // Open the database connection.  If we can't then just let the error
            // float up.
            SqlConnection conn = new SqlConnection(connectionString);
            _server = new Server(new ServerConnection(conn));
        }
        #endregion

        #region Create Schema
        public abstract void WriteSchemaToFile(string schemaFile, string migrationTable, UpdateStatus statusCallback);
        #endregion
    }
}
