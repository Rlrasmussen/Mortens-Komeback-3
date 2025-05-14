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

namespace Mortens_Komeback_3
{
    public class Puzzle : GameObject, ICollidable
    {
        #region Fields
        private Dictionary<string, Puzzle> puzzlePieces;
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
            switch (type)
            {
                case PuzzleType.OrderPuzzle:
                    {
                        puzzlePieces = new Dictionary<string, Puzzle>();
                        puzzlePieces.Add("plaque1", new Puzzle(PuzzleType.OrderPuzzlePlaque, new Vector2(Position.X - (Sprite.Width / 2) - (GameWorld.Instance.Sprites[PuzzleType.OrderPuzzlePlaque][0].Width / 2), Position.Y)));
                        puzzlePieces.Add("plaque2", new Puzzle(PuzzleType.OrderPuzzlePlaque, new Vector2((puzzlePieces["plaque1"].Position.X - GameWorld.Instance.Sprites[PuzzleType.OrderPuzzlePlaque][0].Width), Position.Y)));
                        puzzlePieces.Add("plaque3", new Puzzle(PuzzleType.OrderPuzzlePlaque, new Vector2((puzzlePieces["plaque2"].Position.X - GameWorld.Instance.Sprites[PuzzleType.OrderPuzzlePlaque][0].Width), Position.Y)));
                        foreach (var item in puzzlePieces)
                        {
                            GameWorld.Instance.SpawnObject(item.Value);
                            GameWorld.Instance.gamePuzzles.Add(item.Value);
                        }
                        spriteIndex = 0;
                        break;
                    }
                case PuzzleType.ShootPuzzle:
                    {
                        puzzlePieces = new Dictionary<string, Puzzle>();
                        break;
                    }
                default:
                    break;
                    
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
                puzzlePieces["plaque1"].Sprite == GameWorld.Instance.Sprites[PuzzleType.OrderPuzzlePlaque][0] &&
                puzzlePieces["plaque2"].Sprite == GameWorld.Instance.Sprites[PuzzleType.OrderPuzzlePlaque][2] &&
                puzzlePieces["plaque3"].Sprite == GameWorld.Instance.Sprites[PuzzleType.OrderPuzzlePlaque][1]
                    )
                {
                    Solved = true;
                    Sprite = GameWorld.Instance.Sprites[PuzzleType.OrderPuzzle][1];
                }

            }

        }

        public void OnCollision(ICollidable other)
        {
            /* 
             * if(other is Projectile && (PuzzleType)type == PuzzleType.ShootPuzzle)
             * {
             * Solved = true;
             * }
             * 


            */
        }

        #endregion
    }
}

