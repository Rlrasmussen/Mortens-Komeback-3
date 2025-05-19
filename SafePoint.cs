using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3
{
    public class SafePoint
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Method


        public static bool LoadSave()
        {

            using (GameWorld.Instance.Connection)
            {
                GameWorld.Instance.Connection.Open();

                string commandText = "SELECT * FROM Player WHERE ID = @ID";
                SqliteCommand command = new SqliteCommand(commandText, GameWorld.Instance.Connection);
                command.Parameters.AddWithValue("@ID", Player.Instance.Type);
                SqliteDataReader playerReader = command.ExecuteReader();

                if (playerReader.Read())
                {

                    Player.Instance.Health = playerReader.GetInt32(playerReader.GetOrdinal("CurrentHP"));
                    Player.Instance.Position = GameWorld.Instance.Locations[(Location)playerReader.GetInt32(playerReader.GetOrdinal("RespawnPosition"))];

                    int equipped = playerReader.GetOrdinal("Equipped_Item");

                    if (!playerReader.IsDBNull(equipped))
                        Player.Instance.AcquireItem(playerReader.GetInt32(equipped));

                }
                else
                {

                    return false; //Giv spilleren besked om at der ikke kunne findes noget at loade

                }

                commandText = "SELECT Inventory.* FROM Inventory INNER JOIN Player ON Player.ID = @ID WHERE Player.Equipped_Item IS NULL OR Inventory.Type != Player.Equipped_Item";
                command = new SqliteCommand(commandText, GameWorld.Instance.Connection);
                command.Parameters.AddWithValue("@ID", Player.Instance.Type);
                SqliteDataReader inventoryReader = command.ExecuteReader();

                int type = inventoryReader.GetOrdinal("Type");
                int amount = inventoryReader.GetOrdinal("Amount");

                while (inventoryReader.Read())
                {

                    int amountIs = inventoryReader.GetInt32(amount);
                    if (amountIs > 0)
                        for (int i = 0; i < amountIs; i++)
                            Player.Instance.AcquireItem(inventoryReader.GetInt32(type));

                }

            }

            return true;

        }


        public static bool SaveGame(Location location)
        {

            using (GameWorld.Instance.Connection)
            {

                GameWorld.Instance.Connection.Open();

                string commandText = "DELETE FROM Inventory";
                SqliteCommand command = new SqliteCommand(commandText, GameWorld.Instance.Connection);
                command.ExecuteNonQuery();

                commandText = "INSERT INTO Inventory (Type, Amount) VALUES (@TYPE, 1) ON CONFLICT(Type) DO UPDATE SET Amount = Amount + 1";
                foreach (GameObject item in Player.Instance.Inventory)
                {

                    command = new SqliteCommand(commandText, GameWorld.Instance.Connection);

                    switch (item)
                    {
                        case WeaponMelee:
                        case WeaponRanged:
                            command.Parameters.AddWithValue("@TYPE", item.Type);
                            break;
                        default:
                            command.Parameters.AddWithValue("@TYPE", Convert.ToInt32(item.Type) + Enum.GetNames(typeof(WeaponType)).Length); //Måske?
                            break;
                    }

                    command.ExecuteScalar();

                }

                commandText = "INSERT INTO Player (ID, RespawnPosition, CurrentHP, Equipped_Item) VALUES (@ID, @POSITION, @HP, @EQUIPPED) ON CONFLICT(ID) DO UPDATE SET RespawnPosition = excluded.RespawnPosition, CurrentHP = excluded.CurrentHP, Equipped_Item = excluded.Equipped_Item";
                command = new SqliteCommand(commandText, GameWorld.Instance.Connection);
                command.Parameters.AddWithValue("@ID", Player.Instance.Type);
                command.Parameters.AddWithValue("@HP", Player.Instance.Health);
                command.Parameters.AddWithValue("@POSITION", location);

                if (Player.Instance.EquippedWeapon == null)
                    command.Parameters.AddWithValue("@EQUIPPED", DBNull.Value);
                else
                    command.Parameters.AddWithValue("@EQUIPPED", Player.Instance.EquippedWeapon.Type);

                var check = command.ExecuteScalar();

                return check != null;

            }

        }


        public static bool ClearSave()
        {

            using (GameWorld.Instance.Connection)
            {

                GameWorld.Instance.Connection.Open();
                string commandText = "DELETE FROM Player; DELETE FROM Inventory";
                SqliteCommand command = new SqliteCommand(commandText, GameWorld.Instance.Connection);
                var check = command.ExecuteScalar();

                return check != null;

            }

        }

        #endregion
    }
}
