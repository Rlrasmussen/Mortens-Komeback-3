using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mortens_Komeback_3.Collider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3.Puzzles
{
    class OrderPuzzlePlaque : GameObject , ICollidable
    {
        private int spriteIndex;
        private Texture2D textBubble;

        public OrderPuzzlePlaque(PuzzleType type, Vector2 spawnPos) : base(type, spawnPos)
        {
            spriteIndex = 0;
            textBubble = GameWorld.Instance.Sprites[OverlayObjects.InteractBubble][0];
        }

        /// <summary>
        /// Changes the picture on the puzzle plaque, if the puzzle has the type OrderPuzzlePlaque.
        /// </summary>
        public void ChangePlaque()
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if ((this as ICollidable).CheckCollision(Player.Instance) && (Player.Instance as IPPCollidable).DoHybridCheck(CollisionBox))
            {
                spriteBatch.Draw(textBubble, new Vector2(Position.X, Position.Y - Sprite.Height/3), null, drawColor, Rotation, origin, scale, spriteEffect, layer);
            }
        }

        public void OnCollision(ICollidable other)
        {
        }
    }
}
