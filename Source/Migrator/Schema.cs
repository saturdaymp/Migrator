using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
//using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace Migrator
{
    public abstract class Schema
    {
        #region Vars
        protected readonly Server Server;
        #endregion

        #region Status Update
        public delegate void UpdateStatus(string status);
        protected UpdateStatus UpdateStatusDelg;

        protected void StatusUpdate(string statusUpdate)
        {
            UpdateStatusDelg?.Invoke(statusUpdate);
        }

        #endregion

        #region Constructors
        protected Schema(string connectionString)
        {
            // Open the database connection.  If we can't then just let the error
            // float up.
            var conn = new SqlConnection(connectionString);
            Server = new Server(new ServerConnection(conn));

            // Preload the IsSystemObject field.  If it's not preloaded then
            // checking for it can cause a bunch of extra database queries.
            Server.SetDefaultInitFields(typeof(StoredProcedure), "IsSystemObject");
            Server.SetDefaultInitFields(typeof(Table), "IsSystemObject");
            Server.SetDefaultInitFields(typeof(View), "IsSystemObject");
            Server.SetDefaultInitFields(typeof(UserDefinedFunction), "IsSystemObject");
        }
        #endregion
    }
}
