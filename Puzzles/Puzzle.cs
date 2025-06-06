﻿using Microsoft.Data.Sqlite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mortens_Komeback_3.Collider;
using Mortens_Komeback_3.Environment;
using System;
using System.Collections.Generic;

namespace Mortens_Komeback_3.Puzzles
{
    /// <summary>
    /// Superclass for the games Puzzles
    /// Philip
    /// </summary>
    public abstract class Puzzle : GameObject, ICollidable
    {
        #region Fields
        protected Dictionary<string, GameObject> puzzlePieces;
        protected bool solved = false;
        protected Door puzzleDoor;
        protected int id;
        protected Location location;
        private Texture2D textBubble;


        #endregion

        #region Properties
        public bool Solved
        {
            get => solved;
            set
            {
                bool isChanged = solved;
                solved = value;
                if (value && !isChanged) //Simon
                {
                    SavePoint.ClearSave();
                    DatabaseUpdate();
                    if (location != Location.Spawn)
                        SavePoint.SaveGame(location);
                }
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
        /// <param name="id">Puzzles ID (for database) - Simon</param>
        public Puzzle(PuzzleType type, Vector2 spawnPos, Door puzzleDoor, int id) : base(type, spawnPos)
        {
            this.puzzleDoor = puzzleDoor;
            this.id = id;
            GetStatusFromDB(); //Simon

            foreach (GameObject puzzle in GameWorld.Instance.gamePuzzles) //Simon
                if (puzzle is Puzzle && (puzzle as Puzzle).ID == id)
                    throw new Exception("Puzzle ID already exists, MUST be unique");

            switch (id) //Simon
            {
                case 0:
                    location = Location.PuzzleOne;
                    break;
                case 1:
                    location = Location.PuzzleTwo;
                    break;
                case 2:
                    location = Location.PuzzleThree;
                    break;
                default:
                    break;
            }

            textBubble = GameWorld.Instance.Sprites[OverlayObjects.InteractBubble][0];
        }

        #endregion

        #region Method

        /// <summary>
        /// Changes state to solved if data recieved from database marks puzzle as already solved (on a "load"/respawn)
        /// Simon
        /// </summary>
        public override void Load()
        {
            LockDoor(); //Locks the doors of the puzzle. Philip
            if (solved)
                SolvePuzzle();

            base.Load();

        }

        /// <summary>
        /// Solves the puzzle.
        /// Philip
        /// </summary>
        public virtual void SolvePuzzle()
        {
            Solved = true;
            puzzleDoor.UnlockDoor();
            GameWorld.Instance.Sounds[Sound.PuzzleSucces].Play();
        }

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

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //Draws textbubble when player collides with puzzle - Philip
            if ((this as ICollidable).CheckCollision(Player.Instance) && (Player.Instance as IPPCollidable).DoHybridCheck(CollisionBox))
            {
                spriteBatch.Draw(textBubble, new Vector2(Position.X, Position.Y - Sprite.Height / 2), null, drawColor, Rotation, origin, scale, spriteEffect, layer);
            }
        }

        /// <summary>
        /// Method for updating database when a puzzle has been solved
        /// Simon
        /// </summary>
        public virtual void DatabaseUpdate()
        {

            string commandText = "INSERT INTO Puzzles (ID, Solved) VALUES (@ID, @SOLVED) ON CONFLICT(ID) DO UPDATE SET Solved = excluded.Solved";
            SqliteCommand command = new SqliteCommand(commandText, GameWorld.Instance.Connection);
            command.Parameters.AddWithValue("@ID", id);
            command.Parameters.AddWithValue("@SOLVED", solved);
            command.ExecuteScalar();

        }

        /// <summary>
        /// Method to retrieve data from database upon construction to see if the puzzle has already been solved (if loading a savepoint)
        /// Simon
        /// </summary>
        public virtual void GetStatusFromDB()
        {

            string commandText = "SELECT * FROM Puzzles WHERE ID = @ID"; //Retrieves all data from the row where ID matches the puzzles id, could also just have been "SELECT Solved"
            SqliteCommand command = new SqliteCommand(commandText, GameWorld.Instance.Connection);
            command.Parameters.AddWithValue("@ID", id);
            SqliteDataReader reader = command.ExecuteReader();

            if (reader.Read()) //If any data retrieved, does following
            {
                solved = reader.GetBoolean(reader.GetOrdinal("Solved")); //Sets "solved" to what data from database is
            }

        }

        #endregion
    }
}

