using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mortens_Komeback_3.Environment;
using Mortens_Komeback_3.Collider;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using Mortens_Komeback_3.Factory;

namespace Mortens_Komeback_3.Puzzles
{
    public abstract class Puzzle : GameObject, ICollidable
    {
        #region Fields
        protected Dictionary<string, GameObject> puzzlePieces;
        protected bool solved = false;
        protected Door puzzleDoor;


        #endregion

        #region Properties
        public bool Solved { get => solved; set => solved = value; }

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for a Puzzle. The type given, determnines which kind of puzzle it is.
        /// Philip
        /// </summary> 
        /// <param name="type">The type of puzzle</param>
        /// <param name="spawnPos">The position the main element of the puzzle will be spawned at.</param>
        public Puzzle(PuzzleType type, Vector2 spawnPos, Door puzzleDoor) : base(type, spawnPos)
        {
        }

        #endregion

        #region Method

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

        #endregion
    }
}

