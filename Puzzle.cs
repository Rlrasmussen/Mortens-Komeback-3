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

namespace Mortens_Komeback_3
{
    public class Puzzle : GameObject, ICollidable
    {
        #region Fields
        private Dictionary<string, Puzzle> puzzlePieces;
        private int spriteIndex;
        private bool solved = false;
        private Door puzzleDoor;
        private bool showTextBubble = false;
        private Texture2D textBubble;

        #endregion

        #region Properties
        public bool Solved { get => solved; set => solved = value; }

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for Puzzle. The type given, determnines which kind of puzzle it is.
        /// </summary> Philip
        /// <param name="type">The type of puzzle</param>
        /// <param name="spawnPos">THe position the main element of thee puzzle will be spawned at.</param>
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
                        puzzleDoor = new Door(Position, DoorDirection.Top, DoorType.Locked);
                        GameWorld.Instance.SpawnObject(puzzleDoor);
                        spriteIndex = 0;
                        break;
                    }
                case PuzzleType.ShootPuzzle:
                    {
                        puzzleDoor = new Door(new Vector2(Position.X + 700, Position.Y), DoorDirection.Top, DoorType.Locked);
                        GameWorld.Instance.SpawnObject(puzzleDoor);
                        break;
                    }
                default:
                    break;

            }
            textBubble = GameWorld.Instance.Sprites[OverlayObjects.InteractBubble][0];
        }

        /// <summary>
        /// Overload to Constructor for Puzzle, where doorplacement is given as argument.
        /// Beware that if puzzle is OrderPuzzle, the puzzle will be placed at same place as door, so Spawnposition is irellevant. 
        /// </summary> Philip
        /// <param name="type">The type of puzzle</param>
        /// <param name="spawnPos">THe position the main element of thee puzzle will be spawned at.</param>
        public Puzzle(PuzzleType type, Vector2 spawnPos, Vector2 doorPos) : base(type, spawnPos)
        {
            switch (type)
            {
                case PuzzleType.OrderPuzzle:
                    {
                        Position = doorPos;
                        puzzlePieces = new Dictionary<string, Puzzle>();
                        puzzlePieces.Add("plaque1", new Puzzle(PuzzleType.OrderPuzzlePlaque, new Vector2(Position.X - (Sprite.Width / 2) - (GameWorld.Instance.Sprites[PuzzleType.OrderPuzzlePlaque][0].Width / 2), Position.Y)));
                        puzzlePieces.Add("plaque2", new Puzzle(PuzzleType.OrderPuzzlePlaque, new Vector2((puzzlePieces["plaque1"].Position.X - GameWorld.Instance.Sprites[PuzzleType.OrderPuzzlePlaque][0].Width), Position.Y)));
                        puzzlePieces.Add("plaque3", new Puzzle(PuzzleType.OrderPuzzlePlaque, new Vector2((puzzlePieces["plaque2"].Position.X - GameWorld.Instance.Sprites[PuzzleType.OrderPuzzlePlaque][0].Width), Position.Y)));
                        foreach (var item in puzzlePieces)
                        {
                            GameWorld.Instance.SpawnObject(item.Value);
                            GameWorld.Instance.gamePuzzles.Add(item.Value);
                        }
                        puzzleDoor = new Door(Position, DoorDirection.Top, DoorType.Locked);
                        GameWorld.Instance.SpawnObject(puzzleDoor);
                        spriteIndex = 0;
                        break;
                    }
                case PuzzleType.ShootPuzzle:
                    {
                        puzzleDoor = new Door(doorPos, DoorDirection.Top, DoorType.Locked);
                        GameWorld.Instance.SpawnObject(puzzleDoor);
                        break;
                    }
                default:
                    break;

            }
            textBubble = GameWorld.Instance.Sprites[OverlayObjects.InteractBubble][0];
        }


        #endregion

        #region Method

        /// <summary>
        /// Changes the picture on the puzzle plaque, if the puzzle has the type OrderPuzzlePlaque.
        /// </summary>
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
        /// <summary>
        /// Tries solving the puzzle, i.e. check if all requoremnts ofr solving is met, and if so changes the puzzle to solved. Requirements depends on type of puzzle. 
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
                    SolvePuzzle();
                }

            }

        }

        public void SolvePuzzle()
        {
            Solved = true;
            puzzleDoor.UnlockDoor();
        }
        /// <summary>
        /// The funcitoning happening when the puzzle is colliding with an other object, 
        /// </summary>
        /// <param name="other">The ICollidable object, that the puzzle is colliding with. </param>
        public void OnCollision(ICollidable other)
        {

            if (other is Projectile && (PuzzleType)type == PuzzleType.ShootPuzzle)
            {
                SolvePuzzle();
            }




        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!((PuzzleType)type == PuzzleType.OrderPuzzle))
            {
                base.Draw(spriteBatch);
            }
            if ((PuzzleType)type == PuzzleType.OrderPuzzlePlaque && (this as ICollidable).CheckCollision(Player.Instance) && (Player.Instance as IPPCollidable).DoHybridCheck(this.CollisionBox))
            {
                spriteBatch.Draw(textBubble, new Vector2(Position.X, Position.Y - (Sprite.Height / 2) - (textBubble.Height / 2)), null, drawColor, Rotation, origin, scale, spriteEffect, layer);
            }
        }
        #endregion
    }
}

