using Microsoft.Xna.Framework;
using Mortens_Komeback_3.Collider;
using Mortens_Komeback_3.Environment;
using Mortens_Komeback_3.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3.Puzzles
{
    class ShootPuzzle : Puzzle
    {
        #region Fields
        private AvSurface fire1;
        private AvSurface fire2;
        #endregion

        #region Properties
        #endregion

        #region Constructor

        /// <summary>
        /// A Puzzle that consist of a trigger that needs to be hit by projectile, and a door. 
        /// Philip
        /// </summary>
        /// <param name="type">Type of puzzle </param>
        /// <param name="triggerPos">Position of the trigger</param>
        /// <param name="puzzleDoor">The door that the puzzle will unlock</param>
        public ShootPuzzle(PuzzleType type, Vector2 triggerPos, Door puzzleDoor, int id) : base(type, triggerPos, puzzleDoor, id)
        {
            LockDoor();
        }

        /// <summary>
        /// A Puzzle that consist of a trigger that needs to be hit by projectile, a door and fire surfaces that blocks the door. 
        /// Philip
        /// </summary>
        /// <param name="type">Type of puzzle</param>
        /// <param name="triggerPos">Position of the trigger</param>
        /// <param name="puzzleDoor">The door that the puzzle will unlock</param>
        /// <param name="firstFirePos">Position of the first fire</param>
        /// <param name="firstFireRotation">The rotation of the first fire</param>
        /// <param name="secondFirePos">Position of the second fire</param>
        /// <param name="secondFireRotation">Rotation of the second fire</param>
        public ShootPuzzle(PuzzleType type, Vector2 triggerPos, Door puzzleDoor, Vector2 firstFirePos, float firstFireRotation, Vector2 secondFirePos, float secondFireRotation, int id) : base(type, triggerPos, puzzleDoor, id)
        {
            fire1 = new AvSurface(SurfaceType.AvSurface, firstFirePos, firstFireRotation);
            fire2 = new AvSurface(SurfaceType.AvSurface, secondFirePos, secondFireRotation);
            GameWorld.Instance.SpawnObject(fire1);
            GameWorld.Instance.SpawnObject(fire2);
            LockDoor();
        }
        #endregion

        #region Methods


        public override void OnCollision(ICollidable other)
        {
            if (other is Projectile && (PuzzleType)type == PuzzleType.ShootPuzzle)
            {
                SolvePuzzle();
            }
        }

        public override void SolvePuzzle()
        {
            base.SolvePuzzle();
            if(fire1 != null)
            {
                fire1.IsAlive = false;
            }
            if(fire2 != null)
            {
                fire2.IsAlive = false;
            }
        }
        #endregion
    }
}
