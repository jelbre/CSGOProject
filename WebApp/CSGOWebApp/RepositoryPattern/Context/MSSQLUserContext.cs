using Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern
{
    internal class MSSQLUserContext : Database, IUserContext
    {
        public User GetByID(int ID)
        {
            string query = $"User_GetByID";

            try
            {
                if (OpenConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, sql_connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        try
                        {
                            command.Parameters.AddWithValue("@ID", ID);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    User testuser = new User(reader.GetInt32(0), reader.GetBoolean(3), reader.GetInt32(4));

                                    try { testuser.Steam64ID = reader.GetInt64(1); }
                                    catch { }
                                    try { testuser.Username = reader.GetString(2); }
                                    catch { }

                                    return testuser;
                                }
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

        public int LoginWithPass(User user)
        {
            string query = $"User_LoginWithPass";

            try
            {
                if (OpenConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, sql_connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        try
                        {
                            command.Parameters.AddWithValue("@Username", user.Username);
                            command.Parameters.AddWithValue("@Password", user.Password);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    return reader.GetInt32(0);
                                }
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

            return -1;
        }

        public int LoginWithSteam(User user)
        {
            string query = $"User_LoginWithSteam";

            try
            {
                if (OpenConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, sql_connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        try
                        {
                            command.Parameters.AddWithValue("@Steam64ID", user.Steam64ID);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    return reader.GetInt32(0);
                                }
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

            return -1;
        }

        public int RegisterPass(User user)
        {
            string query = $"User_RegisterPass";

            if (LoginWithPass(user) == -1)
            {
                try
                {
                    if (OpenConnection())
                    {
                        using (SqlCommand command = new SqlCommand(query, sql_connection))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            try
                            {
                                command.Parameters.AddWithValue("@Username", user.Username);
                                command.Parameters.AddWithValue("@Password", user.Password);

                                int id = Convert.ToInt32(command.ExecuteScalar());
                                return id;
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

                return -1;
            }
            else
            {
                return -1;
            }
        }

        public int RegisterSteam(User user)
        {
            string query = $"User_RegisterSteam";

            if (LoginWithSteam(user) == -1)
            {
                try
                {
                    if (OpenConnection())
                    {
                        using (SqlCommand command = new SqlCommand(query, sql_connection))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            try
                            {
                                command.Parameters.AddWithValue("@Steam64ID", user.Steam64ID);

                                int id = Convert.ToInt32(command.ExecuteScalar());
                                return id;
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

                return -1;
            }

            return -1;
        }

        public bool UpdateCooldown(User user, Moneyfaucet moneyfaucet, DateTime cooldown)
        {
            string query = $"User_UpdateCooldown";

            try
            {
                if (OpenConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, sql_connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        try
                        {
                            command.Parameters.AddWithValue("@UserID", user.ID);
                            command.Parameters.AddWithValue("@MoneyfaucetID", moneyfaucet.ID);

                            //To convert the c# datetime into sql format, use these 2 lines
                            SqlParameter parameter = command.Parameters.AddWithValue("@Cooldown", System.Data.SqlDbType.DateTime);
                            parameter.Value = cooldown;

                            command.ExecuteNonQuery();

                            return true;
                        }
                        catch (SqlException ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            catch (SqlException)
            {
                return false;
            }
            finally
            {
                CloseConnection();
            }
            return false;
        }

        //Uses procedures User_UpdatePassword, User_UpdateSteam64ID, User_UpdateUsername and User_UpdateUsernamePassword
        public bool UpdateLoginData(User user, string type)
        {
            //Type should always be 'Password', 'Steam64ID', 'Username' or 'UsernamePassword' (Case sensitive)
            string query = $"User_Update" + type;

            try
            {
                if (OpenConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, sql_connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        try
                        {
                            command.Parameters.AddWithValue("@ID", user.ID);
                            if (type.Contains("Username"))
                            { command.Parameters.AddWithValue("@Username", user.Username); }
                            if (type.Contains("Password"))
                            { command.Parameters.AddWithValue("@Password", user.Password); }
                            if(type.Contains("Steam64ID"))
                            { command.Parameters.AddWithValue("@Steam64ID", user.Steam64ID); }
                            
                            if(command.Parameters.Count == 1)
                            {
                                throw new Exception("The specified type does not exist. Type should always be 'Password', 'Steam64ID', 'Username' or 'UsernamePassword' (Case sensitive)");
                            }

                            command.ExecuteNonQuery();

                            return true;
                        }
                        catch (SqlException ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            catch (SqlException)
            {
                return false;
            }
            finally
            {
                CloseConnection();
            }
            return false;
        }

        public string GetPassword(User user)
        {
            string query = $"User_GetPassword";

            try
            {
                if (OpenConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, sql_connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        try
                        {
                            command.Parameters.AddWithValue("@ID", user.ID);

                            string password = (string)command.ExecuteScalar();

                            return password;
                        }
                        catch (SqlException ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            catch (SqlException)
            {
                return null;
            }
            finally
            {
                CloseConnection();
            }
            return null;
        }

        public bool Delete(User user)
        {
            string query = $"User_Delete";

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

                            int deletedUsers = Convert.ToInt32(command.ExecuteScalar());

                            if (deletedUsers > 0)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        catch (SqlException ex)
                        {
                            throw ex;
                        }
                    }
                }
            }
            catch (SqlException)
            {
                return false;
            }
            finally
            {
                CloseConnection();
            }
            return false;
        }
    }
}
