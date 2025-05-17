using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mortens_Komeback_3.Collider;
using Mortens_Komeback_3.Environment;
using Mortens_Komeback_3.Puzzles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3.Puzzles
{
    class OrderPuzzle : Puzzle
    {


        /// <summary>
        /// Constructor for an OrderPuzzle: A puzzle consisting of a door and three rotatable plaques.
        /// Philip
        /// </summary>
        /// <param name="type">Type of the puzzle</param>
        /// <param name="spawnPos">Position of the Door/primary element of puzzle</param>
        /// <param name="doorDirection">Direction of the door</param>
        /// <param name="plaque1Pos">Position of the first plaque</param>
        /// <param name="plaque2Pos">Position of the second plaque</param>
        /// <param name="plaque3Pos">Position of the third plaque</param>
        public OrderPuzzle(PuzzleType type, Vector2 spawnPos, DoorDirection doorDirection, Vector2 plaque1Pos, Vector2 plaque2Pos, Vector2 plaque3Pos) : base(type, spawnPos)
        {
            puzzlePieces = new Dictionary<string, GameObject>();
            puzzlePieces.Add("plaque1", new OrderPuzzlePlaque(PuzzleType.OrderPuzzlePlaque, plaque1Pos));
            puzzlePieces.Add("plaque2", new OrderPuzzlePlaque(PuzzleType.OrderPuzzlePlaque, plaque2Pos));
            puzzlePieces.Add("plaque3", new OrderPuzzlePlaque(PuzzleType.OrderPuzzlePlaque, plaque3Pos));
            foreach (var item in puzzlePieces)
            {
                GameWorld.Instance.SpawnObject(item.Value);
                GameWorld.Instance.gamePuzzles.Add(item.Value);
            }
            puzzleDoor = new Door(Position, doorDirection, DoorType.Locked);
            GameWorld.Instance.SpawnObject(puzzleDoor);
        }

        

        /// <summary>
        /// Tries solving the puzzle, i.e. checks if all requirements for solving is met, and if so, changes the puzzle to solved. Requirements depends on type of puzzle. 
        /// </summary> Philip
        public void TrySolve()
        {
            if ((PuzzleType)type == PuzzleType.OrderPuzzle)
            {
                if (
                puzzlePieces["plaque1"].Sprite == GameWorld.Instance.Sprites[PuzzleType.OrderPuzzlePlaque][0] &&
                puzzlePieces["plaque2"].Sprite == GameWorld.Instance.Sprites[PuzzleType.OrderPuzzlePlaque][2] &&
                puzzlePieces["plaque3"].Sprite == GameWorld.Instance.Sprites[PuzzleType.OrderPuzzlePlaque][1]
                    )
                {
                    this.SolvePuzzle();
                }

            }

        }

        public override void SolvePuzzle()
        {
            Solved = true;
            puzzleDoor.UnlockDoor();
        }


        public override void Draw(SpriteBatch spriteBatch)
        {   
        }


    }
}
