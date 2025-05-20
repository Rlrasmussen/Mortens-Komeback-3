using Microsoft.Xna.Framework;
using Microsoft.Data.Sqlite;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mortens_Komeback_3.Environment;
using Mortens_Komeback_3.Collider;
using Microsoft.Xna.Framework.Graphics;
using Mortens_Komeback_3.Factory;

namespace Mortens_Komeback_3.Puzzles
{
    public abstract class Puzzle : GameObject, ICollidable
    {
        #region Fields
        protected Dictionary<string, GameObject> puzzlePieces;
        protected bool solved = false;
        protected Door puzzleDoor;
        protected int id;


        #endregion

        #region Properties
        public bool Solved
        {
            get => solved;
            set
            {
                bool isChanged = solved;
                solved = value;
                if (value && !isChanged)
                    DatabaseUpdate();
            }
        }


        public int ID { get => id; }

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for a Puzzle. The type given, determnines which kind of puzzle it is.
        /// Philip
        /// </summary> 
        /// <param name="type">The type of puzzle</param>
        /// <param name="spawnPos">The position the main element of the puzzle will be spawned at.</param>
        public Puzzle(PuzzleType type, Vector2 spawnPos, Door puzzleDoor, int id) : base(type, spawnPos)
        {

            this.id = id;
            GetStatusFromDB();

            foreach (GameObject puzzle in GameWorld.Instance.gamePuzzles)
                if (puzzle is Puzzle && (puzzle as Puzzle).ID == id)
                    throw new Exception("Puzzle ID already exists, must be unique");

        }

        #endregion

        #region Method


        public override void Load()
        {

            if (solved)
                SolvePuzzle();

            base.Load();

        }

        /// <summary>
        /// Solves the puzzle.
        /// Philip
        /// </summary>
        public abstract void SolvePuzzle();

        /// <summary>
        /// The funcitoning happening when the puzzle is colliding with another object, 
        /// </summary>
        /// <param name="other">The ICollidable object, that the puzzle is colliding with. </param>
        public virtual void OnCollision(ICollidable other)
        {
        }
        /// <summary>
        /// If the puzzleDoor of the puzzle is not locked, its' status is set to locked, and the sprite changed to the locked version. 
        /// Philip
        /// </summary>
        public virtual void LockDoor()
        {
            if (puzzleDoor.DoorStatus == DoorType.Open || puzzleDoor.DoorStatus == DoorType.Closed)
            {
                puzzleDoor.DoorStatus = DoorType.Locked;
            }
            else if (puzzleDoor.DoorStatus == DoorType.Stairs)
            {
                puzzleDoor.DoorStatus = DoorType.StairsLocked;
            }
        }


        public virtual void DatabaseUpdate()
        {

            using (GameWorld.Instance.Connection)
            {

                GameWorld.Instance.Connection.Open();

                string commandText = "INSERT INTO Puzzles (ID, Solved) VALUES (@ID, @SOLVED) ON CONFLICT(ID) DO UPDATE SET Solved = excluded.Solved";
                SqliteCommand command = new SqliteCommand(commandText, GameWorld.Instance.Connection);
                command.Parameters.AddWithValue("@ID", id);
                command.Parameters.AddWithValue("@SOLVED", solved);
                command.ExecuteScalar();

            }

        }


        public virtual void GetStatusFromDB()
        {

            using (GameWorld.Instance.Connection)
            {

                GameWorld.Instance.Connection.Open();

                string commandText = "SELECT * FROM Puzzles WHERE ID = @ID";
                SqliteCommand command = new SqliteCommand(commandText, GameWorld.Instance.Connection);
                command.Parameters.AddWithValue("@ID", id);
                SqliteDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    solved = reader.GetBoolean(reader.GetOrdinal("Solved"));
                }

            }

        }

        #endregion
    }
}

