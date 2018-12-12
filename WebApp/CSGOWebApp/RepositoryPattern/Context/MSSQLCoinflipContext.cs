using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPattern
{
    internal class MSSQLCoinflipContext : Database, ICoinflipContext
    {
        public List<Coinflip> GetAll()
        {
            string query = $"Coinflip_GetAll";

            try
            {
                if (OpenConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, sql_connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        try
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                List<Coinflip> flipList = new List<Coinflip>();
                                while (reader.Read())
                                {
                                    //Putting all the reader values into objects to simplify the values in the reader
                                    //Skin is added later in IF statement to check if skin is not null
                                    Coinflip coinflip = new Coinflip(reader.GetInt32(0), new List<Bet>(), reader.GetInt32(1));
                                    Bet bet = new Bet(reader.GetInt32(2), new User(), reader.GetDateTime(3), new List<Skin>());
                                    User user;
                                    //Converting price to decimal
                                    decimal balance = reader.GetInt32(8);
                                    balance = balance / 100;
                                    if (!reader.IsDBNull(5))
                                    {
                                        user = new User(reader.GetInt32(4), reader.GetInt64(5), reader.GetString(6), reader.GetBoolean(7), balance);
                                    }
                                    else
                                    {
                                        user = new User(reader.GetInt32(4), reader.GetString(6), reader.GetBoolean(7), balance);
                                    }
                                    
                                    bet.User = user;
                                    
                                    //putting the data in the actual list

                                    //Check if the row is still about the same coinflip
                                    if (flipList.Count != 0)
                                    {
                                        if (coinflip.ID != flipList.Last().ID)
                                        {
                                            flipList.Add(coinflip);
                                        }
                                    }
                                    else
                                    {
                                        flipList.Add(coinflip);
                                    }

                                    //Checks if the coinflip has any bets and adds one if it doesn't
                                    if (flipList.Last().Bets.Count != 0)
                                    {
                                        //Checks if the latest bet is the same as the one in the list, adds a new one if it isn't
                                        if (flipList.Last().Bets.Last().ID != bet.ID)
                                        {
                                            flipList.Last().Bets.Add(bet);
                                        }
                                    }
                                    else
                                    {
                                        flipList.Last().Bets.Add(bet);
                                    }

                                    //Checks if the skin in not null and adds it
                                    if (!reader.IsDBNull(9))
                                    {
                                        decimal price = reader.GetInt32(15);
                                        price = price / 100;
                                        Skin skin = new Skin(reader.GetInt32(9), reader.GetString(10), reader.GetString(11), reader.GetString(12),
                                            reader.GetInt32(13), reader.GetInt32(14), price, reader.GetDateTime(16), new Rarity(1));

                                        Rarity rarity;

                                        if (!reader.IsDBNull(22))
                                        {
                                            rarity = new Rarity(reader.GetInt32(17), reader.GetString(18), reader.GetInt32(19),
                                                reader.GetInt32(20), reader.GetInt32(21), reader.GetInt32(22));
                                        }
                                        else
                                        {
                                            rarity = new Rarity(reader.GetInt32(17), reader.GetString(18), reader.GetInt32(19),
                                                reader.GetInt32(20), reader.GetInt32(21));
                                        }

                                        skin.Rarity = rarity;

                                        flipList.Last().Bets.Last().Skins.Add(skin);
                                    }
                                }
                                return flipList;
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

            return new List<Coinflip>();
        }

        public Coinflip GetByID(int ID)
        {
            string query = $"Coinflip_GetByID";

            try
            {
                if (OpenConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, sql_connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        try
                        {
                            command.Parameters.AddWithValue("@CoinflipID", ID);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                Coinflip coinflip = new Coinflip(-1, new List<Bet>(), -1);
                                bool firstRead = true;
                                while (reader.Read())
                                {
                                    //Putting all the reader values into objects to simplify the values in the reader
                                    if (firstRead)
                                    {
                                        coinflip = new Coinflip(reader.GetInt32(0), new List<Bet>(), reader.GetInt32(1));
                                        firstRead = false;
                                    }
                                    Bet bet = new Bet(reader.GetInt32(2), new User(), reader.GetDateTime(3), new List<Skin>());
                                    User user;
                                    //Converting price to decimal
                                    decimal balance = reader.GetInt32(8);
                                    balance = balance / 100;
                                    if (!reader.IsDBNull(5))
                                    {
                                        user = new User(reader.GetInt32(4), reader.GetInt64(5), reader.GetString(6), reader.GetBoolean(7), balance);
                                    }
                                    else
                                    {
                                        user = new User(reader.GetInt32(4), reader.GetString(6), reader.GetBoolean(7), balance);
                                    }

                                    bet.User = user;
                                    
                                    //putting the data in the actual list

                                    //Checks if the coinflip has any bets and adds one if it doesn't
                                    if (coinflip.Bets.Count != 0)
                                    {
                                        //Checks if the latest bet is the same as the one in the list, adds a new one if it isn't
                                        if (coinflip.Bets.Last().ID != bet.ID)
                                        {
                                            coinflip.Bets.Add(bet);
                                        }
                                    }
                                    else
                                    {
                                        coinflip.Bets.Add(bet);
                                    }

                                    //Checks if the skin in not null and adds it
                                    if (!reader.IsDBNull(9))
                                    {
                                        decimal price = reader.GetInt32(15);
                                        price = price / 100;
                                        Skin skin = new Skin(reader.GetInt32(9), reader.GetString(10), reader.GetString(11), reader.GetString(12),
                                            reader.GetInt32(13), reader.GetInt32(14), price, reader.GetDateTime(16), new Rarity(1));

                                        Rarity rarity;

                                        if (!reader.IsDBNull(22))
                                        {
                                            rarity = new Rarity(reader.GetInt32(17), reader.GetString(18), reader.GetInt32(19),
                                                reader.GetInt32(20), reader.GetInt32(21), reader.GetInt32(22));
                                        }
                                        else
                                        {
                                            rarity = new Rarity(reader.GetInt32(17), reader.GetString(18), reader.GetInt32(19),
                                                reader.GetInt32(20), reader.GetInt32(21));
                                        }

                                        skin.Rarity = rarity;

                                        coinflip.Bets.Last().Skins.Add(skin);
                                    }
                                }
                                if (!firstRead)
                                {
                                    return coinflip;
                                }
                                else
                                {
                                    return null;
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

        public int CreateCoinflip(User User, List<Skin> Skins)
        {
            string query = $"Coinflip_Create";

            try
            {
                if (OpenConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, sql_connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        try
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("ID", typeof(int));
                            foreach (Skin skin in Skins)
                            {
                                dataTable.Rows.Add(skin.ID);
                            }

                            command.Parameters.AddWithValue("@Creator_ID", User.ID);
                            command.Parameters.AddWithValue("@Skins", dataTable);

                            return Convert.ToInt32(command.ExecuteScalar());
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

        public bool PotentialJoin(Coinflip coinflip)
        {
            string query = $"Coinflip_PotentialJoin";

            try
            {
                if (OpenConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, sql_connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        try
                        {
                            command.Parameters.AddWithValue("@CoinflipID", coinflip.ID);

                            return Convert.ToBoolean(command.ExecuteScalar());
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

            return false;
        }

        public bool JoinCoinflip(Coinflip coinflip, User User, List<Skin> Skins)
        {
            string query = $"Coinflip_Join";

            try
            {
                if (OpenConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, sql_connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        try
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Columns.Add("ID", typeof(int));
                            foreach (Skin skin in Skins)
                            {
                                dataTable.Rows.Add(skin.ID);
                            }

                            command.Parameters.AddWithValue("@CoinflipID", coinflip.ID);
                            command.Parameters.AddWithValue("@Joiner_ID", User.ID);
                            command.Parameters.AddWithValue("@Skins", dataTable);

                            return Convert.ToBoolean(command.ExecuteScalar());
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

            return false;
        }

        public int RewardWinner(Coinflip coinflip)
        {
            string query = $"Coinflip_RewardWinner";

            try
            {
                if (OpenConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, sql_connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        try
                        {
                            command.Parameters.AddWithValue("@CoinflipID", coinflip.ID);

                            return Convert.ToInt32(command.ExecuteScalar());
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
    }
}
