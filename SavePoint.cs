﻿using Microsoft.Data.Sqlite;
using System;

namespace Mortens_Komeback_3
{
    public class SavePoint
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Method

        /// <summary>
        /// Retrieves data from local database as a way of "loading"
        /// Simon
        /// </summary>
        /// <returns>False if no data to retrieve, else True</returns>
        public static bool LoadSave()
        {

            string commandText = "SELECT * FROM Player WHERE ID = @ID"; //Retrieves the row where the ID matches the players ID (currently only one)
            SqliteCommand command = new SqliteCommand(commandText, GameWorld.Instance.Connection);
            command.Parameters.AddWithValue("@ID", 0);
            SqliteDataReader playerReader = command.ExecuteReader();

            if (playerReader.Read())
            {

                Player.Instance.SetHealthFromDB(playerReader.GetInt32(playerReader.GetOrdinal("CurrentHP")));
                Player.Instance.Position = GameWorld.Instance.Locations[(Location)playerReader.GetInt32(playerReader.GetOrdinal("RespawnPosition"))];

                int equipped = playerReader.GetOrdinal("Equipped_Item");

                if (!playerReader.IsDBNull(equipped)) //Checks if there's a null value for weapon (if no weapon was equipped when progress was saved)
                    Player.Instance.AcquireItem(playerReader.GetInt32(equipped)); //Adds the weapon equipped first before rest of inventory

            }
            else
            {

                return false; //Enables sending a "no row  was found" reaction

            }

            commandText = "SELECT Inventory.* FROM Inventory INNER JOIN Player ON Player.ID = @ID WHERE Player.Equipped_Item IS NULL OR Inventory.Type != Player.Equipped_Item"; //Retrieves all items from Inventory table and adds them to players List of the same name unless it's the equipped weapon (which was added earlier - method adds any weapon as equipped if it's null)
            command = new SqliteCommand(commandText, GameWorld.Instance.Connection);
            command.Parameters.AddWithValue("@ID", 0);
            SqliteDataReader inventoryReader = command.ExecuteReader();

            int type = inventoryReader.GetOrdinal("Type");
            int amount = inventoryReader.GetOrdinal("Amount");

            while (inventoryReader.Read())
            {

                int amountIs = inventoryReader.GetInt32(amount); //Gets the amount associated with the ID of the item
                if (amountIs > 0)
                    for (int i = 0; i < amountIs; i++)
                        Player.Instance.AcquireItem(inventoryReader.GetInt32(type)); //Adds the item with id for the "amount" times

            }

            return true;

        }

        /// <summary>
        /// Method for saving relevant data from Player to database, overwrites old data if present, otherwise "inserts" new data
        /// Simon
        /// </summary>
        /// <param name="location">Position logic converted to enum for ease-of-use with database</param>
        /// <returns>True if operation was successful</returns>
        public static bool SaveGame(Location location)
        {

            string commandText = "DELETE FROM Inventory WHERE Type NOT IN (SELECT Equipped_Item FROM Player WHERE Equipped_Item IS NOT NULL)"; //"Clears" the Inventory in the database except the equipped weapon (which is a foreign key)
            SqliteCommand command = new SqliteCommand(commandText, GameWorld.Instance.Connection);
            command.ExecuteNonQuery();

            commandText = "INSERT INTO Inventory (Type, Amount) SELECT @TYPE, 1 WHERE @TYPE NOT IN (SELECT Equipped_Item FROM Player WHERE Equipped_Item IS NOT NULL) ON CONFLICT(Type) DO UPDATE SET Amount = Amount + 1;"; //Populates the Inventory in database with exception of the "EquippedWeapon"
            foreach (GameObject item in Player.Instance.Inventory)
            {

                command = new SqliteCommand(commandText, GameWorld.Instance.Connection);

                switch (item)
                {
                    case Weapon:
                        command.Parameters.AddWithValue("@TYPE", item.Type);
                        break;
                    default:
                        command.Parameters.AddWithValue("@TYPE", Convert.ToInt32(item.Type) + Enum.GetNames(typeof(WeaponType)).Length); //Maybe?
                        break;
                }

                command.ExecuteScalar();

            }

            commandText = "INSERT INTO Player (ID, RespawnPosition, CurrentHP, Equipped_Item) VALUES (@ID, @POSITION, @HP, @EQUIPPED) ON CONFLICT(ID) DO UPDATE SET RespawnPosition = excluded.RespawnPosition, CurrentHP = excluded.CurrentHP, Equipped_Item = excluded.Equipped_Item"; //INSERTs or UPDATEs data relevant to player including the currently equipped weapon
            command = new SqliteCommand(commandText, GameWorld.Instance.Connection);
            command.Parameters.AddWithValue("@ID", 0);
            command.Parameters.AddWithValue("@HP", Player.Instance.Health);
            command.Parameters.AddWithValue("@POSITION", location);

            if (Player.Instance.EquippedWeapon == null)
                command.Parameters.AddWithValue("@EQUIPPED", DBNull.Value); //Inserts a null value if no weapon is equipped
            else
                command.Parameters.AddWithValue("@EQUIPPED", Player.Instance.EquippedWeapon.Type);

            var check = command.ExecuteScalar();

            return check != null;

        }

        /// <summary>
        /// Method to clear or reset data in database dependant on table
        /// Simon
        /// </summary>
        /// <returns>True if operation was succesful</returns>
        public static bool ClearSave()
        {

            string commandText = "DELETE FROM Player; DELETE FROM Inventory; UPDATE Puzzles SET Solved = 0 WHERE Solved = 1"; //Deletes all rows from Player and Inventory tables and sets all Solved values in Puzzles to false
            SqliteCommand command = new SqliteCommand(commandText, GameWorld.Instance.Connection);
            var check = command.ExecuteScalar();

            return check != null;

        }

        #endregion
    }
}
