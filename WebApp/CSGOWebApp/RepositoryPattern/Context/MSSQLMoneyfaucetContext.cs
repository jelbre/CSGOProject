using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern
{
    internal class MSSQLMoneyfaucetContext : Database, IMoneyfaucetContext
    {
        public List<Moneyfaucet> GetAll(User user)
        {
            List<Moneyfaucet> faucets = new List<Moneyfaucet>();
            string query = $"Moneyfaucet_GetAll";

            try
            {
                if (OpenConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, sql_connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        try
                        {
                            command.Parameters.AddWithValue("@User_ID", user.ID);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    faucets.Add(new Moneyfaucet(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetDateTime(3), 
                                        reader.GetInt32(4), reader.GetInt32(5) / 100, reader.GetInt32(6) / 100, 
                                        reader.GetInt32(7), reader.GetInt32(8), reader.GetInt32(9), reader.GetInt32(10)));
                                }
                                return faucets;
                            }
                        }
                        catch (InvalidOperationException exception)
                        {
                        }
                    }
                }
            }
            catch (SqlException exception)
            {
            }
            finally
            {
                CloseConnection();
            }

            return null;
        }
    }
}
