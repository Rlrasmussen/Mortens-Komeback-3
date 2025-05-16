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
        /// <summary>
        /// A Puzzle that consist of a trigger that needs to be hit by projectile, and a door. 
        /// Philip
        /// </summary>
        /// <param name="type">Type of puzzle </param>
        /// <param name="spawnPos">Position of the trigger</param>
        /// <param name="doorPos">Position of the door</param>
        public ShootPuzzle(PuzzleType type, Vector2 spawnPos, Vector2 doorPos) : base(type, spawnPos)
        {

            puzzleDoor = new Door(doorPos, DoorDirection.Top, DoorType.Locked);
            GameWorld.Instance.SpawnObject(puzzleDoor);


        }

        public override void OnCollision(ICollidable other)
        {
            if (other is Projectile && (PuzzleType)type == PuzzleType.ShootPuzzle)
            {
                SolvePuzzle();
            }
        }

        public override void SolvePuzzle()
        {
            Solved = true;
            puzzleDoor.UnlockDoor();
        }
    }
}
