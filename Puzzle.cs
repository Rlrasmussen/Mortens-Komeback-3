using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mortens_Komeback_3.Environment;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;

namespace Mortens_Komeback_3
{
    public class Puzzle : GameObject
    {
        #region Fields
        private Dictionary<string, Puzzle> puzzlePlaques;
        private int spriteIndex;
        private bool solved = false;

        #endregion

        #region Properties
        public bool Solved { get => solved; set => solved = value; }

        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="spawnPos"></param>
        public Puzzle(PuzzleType type, Vector2 spawnPos) : base(type, spawnPos)
        {
            if (type == PuzzleType.OrderPuzzle)
            {
                puzzlePlaques = new Dictionary<string, Puzzle>();
                puzzlePlaques.Add("plaque1", new Puzzle(PuzzleType.OrderPuzzlePlaque, new Vector2(Position.X - (Sprite.Width / 2) - (GameWorld.Instance.Sprites[PuzzleType.OrderPuzzlePlaque][0].Width / 2), Position.Y)));
                puzzlePlaques.Add("plaque2", new Puzzle(PuzzleType.OrderPuzzlePlaque, new Vector2((puzzlePlaques["plaque1"].Position.X - GameWorld.Instance.Sprites[PuzzleType.OrderPuzzlePlaque][0].Width), Position.Y)));
                puzzlePlaques.Add("plaque3", new Puzzle(PuzzleType.OrderPuzzlePlaque, new Vector2((puzzlePlaques["plaque2"].Position.X - GameWorld.Instance.Sprites[PuzzleType.OrderPuzzlePlaque][0].Width), Position.Y)));
                foreach (var item in puzzlePlaques)
                { GameWorld.Instance.SpawnObject(item.Value); }
                spriteIndex = 0;
            }
        }

        #endregion

        #region Method


        public void ChangePlaque()
        {
            if ((PuzzleType)type == PuzzleType.OrderPuzzlePlaque)
            {
                if (spriteIndex < GameWorld.Instance.Sprites[PuzzleType.OrderPuzzlePlaque].Length - 1)
                {
                    spriteIndex++;
                }
                else
                {
                    spriteIndex = 0;
                }
                Sprite = GameWorld.Instance.Sprites[PuzzleType.OrderPuzzlePlaque][spriteIndex];
            }
        }

        public void TrySolve()
        {
            if ((PuzzleType)type == PuzzleType.OrderPuzzle)
            {
                if (
                puzzlePlaques["plague1"].Sprite == GameWorld.Instance.Sprites[PuzzleType.OrderPuzzlePlaque][0] &&
                puzzlePlaques["plague1"].Sprite == GameWorld.Instance.Sprites[PuzzleType.OrderPuzzlePlaque][2] &&
                puzzlePlaques["plague1"].Sprite == GameWorld.Instance.Sprites[PuzzleType.OrderPuzzlePlaque][1]
                    )
                {
                    Solved = true;
                }

            }

        }

        #endregion
    }
}

