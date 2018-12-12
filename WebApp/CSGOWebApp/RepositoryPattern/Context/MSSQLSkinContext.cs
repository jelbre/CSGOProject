using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using System.Data.SqlClient;
using System.Data;

namespace RepositoryPattern
{
    internal class MSSQLSkinContext : Database, ISkinContext
    {
        public Inventory GetAllFromInv(User user, bool invType)
        {
            string query = $"Skin_GetAllFromInv";

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
                            command.Parameters.AddWithValue("@InvType", invType);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                Inventory inv = new Inventory();
                                inv.Type = invType;
                                while (reader.Read())
                                {
                                    inv.ID = reader.GetInt32(0);

                                    decimal price = reader.GetInt32(7);
                                    price = price / 100;

                                    if (!reader.IsDBNull(14))
                                    {
                                        //With higherRarityID
                                        inv.Skins.Add(
                                            new Skin(reader.GetInt32(1), reader.GetString(2), reader.GetString(3),
                                            reader.GetString(4), reader.GetInt32(5), reader.GetInt32(6), price,
                                            reader.GetDateTime(8), new Rarity(reader.GetInt32(9), reader.GetString(10), reader.GetInt32(11),
                                            reader.GetInt32(12), reader.GetInt32(13), reader.GetInt32(14))));
                                    }
                                    else
                                    {
                                        //Without higherRarityID
                                        inv.Skins.Add(
                                            new Skin(reader.GetInt32(1), reader.GetString(2), reader.GetString(3),
                                            reader.GetString(4), reader.GetInt32(5), reader.GetInt32(6), price,
                                            reader.GetDateTime(8), new Rarity(reader.GetInt32(9), reader.GetString(10), reader.GetInt32(11),
                                            reader.GetInt32(12), reader.GetInt32(13))));
                                    }
                                }
                                return inv;
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
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

        public bool InsertSkins(List<Skin> skins, User user, bool invType)
        {
            string query = $"Skin_Insert";

            try
            {
                if (OpenConnection())
                {
                    using (SqlCommand command = new SqlCommand(query, sql_connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        foreach (Skin skin in skins)
                        {
                            try
                            {
                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("@UserID", user.ID);
                                command.Parameters.AddWithValue("@InvType", invType);
                                command.Parameters.AddWithValue("@Name", skin.Name);
                                command.Parameters.AddWithValue("@Price", skin.Price * 100);
                                command.Parameters.AddWithValue("@Weapon", skin.Weapon);
                                command.Parameters.AddWithValue("@SkinCollection", skin.Collection);
                                command.Parameters.AddWithValue("@Float", skin.SkinFloat * 1000000000);
                                command.Parameters.AddWithValue("@PatternID", skin.PatternID);
                                command.Parameters.AddWithValue("@RarityID", skin.Rarity.ID); //LATER SKIN RARITY ID PAKKEN
                                command.ExecuteNonQuery();
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                        return true;
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

        public bool TransferSkins(User user, List<Skin> skins, bool TargetInv)
        {
            string query = $"Skin_TransferSkins";

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
                            foreach (Skin skin in skins)
                            {
                                dataTable.Rows.Add(skin.ID);
                            }
                            command.Parameters.AddWithValue("@UserID", user.ID);
                            command.Parameters.AddWithValue("@Skins", dataTable);
                            command.Parameters.AddWithValue("@TargetInv", TargetInv);

                            command.ExecuteNonQuery();

                            return true;
                        }
                        catch (Exception ex)
                        {
                            throw ex;
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
    }
}
