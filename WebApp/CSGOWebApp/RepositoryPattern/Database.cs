using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern
{
    internal class Database
    {
        public SqlConnection sql_connection;

        public bool OpenConnection()
        {
            string connectionString = @"Server=tcp:csgoserver.database.windows.net,1433;Initial Catalog=CSGODatabase;Persist Security Info=False;User ID=jelbre;Password=KillerappSemester2;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            sql_connection = new SqlConnection(connectionString);

            try
            {
                sql_connection.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CloseConnection()
        {
            try
            {
                sql_connection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
