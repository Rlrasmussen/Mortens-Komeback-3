using Microsoft.Xna.Framework;
using Mortens_Komeback_3.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3.Puzzles
{
    class PathfindingPuzzle : Puzzle
    {
        private Room puzzleRoom;

        public PathfindingPuzzle(PuzzleType type, Vector2 spawnPos, Door puzzleDoor, int id, Room room, Vector2 pathStartPos, Vector2 pathEndPos, Vector2 pathGoalPoint) : base(type, spawnPos, puzzleDoor, id)
        {
            


        }

        public override void SolvePuzzle()
        {
            throw new NotImplementedException();
        }
    }
}
