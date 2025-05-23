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
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for an OrderPuzzle: A puzzle consisting of a door, a lever and three rotatable plaques.
        /// Philip
        /// </summary>
        /// <param name="type">Type of the puzzle</param>
        /// <param name="spawnPos">Position of primary element of puzzle.  </param>
        /// <param name="puzzleDoor">The door that the puzzle will unlock</param>
        /// <param name="plaque1Pos">Position of the first plaque</param>
        /// <param name="plaque2Pos">Position of the second plaque</param>
        /// <param name="plaque3Pos">Position of the third plaque</param>
        public OrderPuzzle(PuzzleType type, Vector2 spawnPos, Door puzzleDoor, Vector2 plaque1Pos, Vector2 plaque2Pos, Vector2 plaque3Pos, int id) : base(type, spawnPos, puzzleDoor, id)
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
            Position = spawnPos;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Tries solving the puzzle, i.e. checks if all requirements for solving is met, and if so, changes the puzzle to solved. Requirements depends on type of puzzle. 
        /// </summary> Philip
        public void TrySolve()
        {
            if ((PuzzleType)type == PuzzleType.OrderPuzzle)
            {
                if (
                puzzlePieces["plaque1"].Sprite == GameWorld.Instance.Sprites[PuzzleType.OrderPuzzlePlaque][0] &&
                puzzlePieces["plaque2"].Sprite == GameWorld.Instance.Sprites[PuzzleType.OrderPuzzlePlaque][1] &&
                puzzlePieces["plaque3"].Sprite == GameWorld.Instance.Sprites[PuzzleType.OrderPuzzlePlaque][2]
                    )
                {
                    this.SolvePuzzle();
                }

            }

        }

        public override void SolvePuzzle()
        {
            base.SolvePuzzle();
            Sprite = GameWorld.Instance.Sprites[PuzzleType.OrderPuzzle][2];
        }




        #endregion

    }
}
