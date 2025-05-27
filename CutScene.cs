using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3
{
    public class CutScene : GameObject, IAnimate
    {
        #region Fields
        private int maxIndex = 56 - 1;
        private bool animate = true;

        private string start = "Press E to start";
        private string end = "Press ESC to stop playing";
        #endregion

        #region Properties
        public float FPS { get; set; } = 6;
        public Texture2D[] Sprites { get; set; }
        public float ElapsedTime { get; set; }
        public int CurrentIndex { get; set; }

        #endregion

        #region Constructor
        public CutScene(Enum type, Vector2 spawnPos) : base(type, spawnPos)
        {
            if (GameWorld.Instance.Sprites.TryGetValue(type, out var sprites))
                Sprites = sprites;
#if DEBUG
            else
                Debug.WriteLine("Kunne ikke sætte sprites for " + ToString());
#endif

            layer = 0.8f;
        }

        public override void Update(GameTime gameTime)
        {
            (this as IAnimate).Animate();
            base.Update(gameTime);


        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (CurrentIndex == maxIndex)
            {
                spriteBatch.Draw(Sprites[maxIndex], Position, null, drawColor, Rotation, origin, scale, spriteEffect, layer);
                animate = false;

                spriteBatch.DrawString(GameWorld.Instance.GameFont, start, new Vector2(0, -2000) + new Vector2(250, 400), Color.Black, 0f, Vector2.Zero, 1.9f, SpriteEffects.None, layer + 0.05f);
                spriteBatch.DrawString(GameWorld.Instance.GameFont, end, new Vector2(0, -2000) + new Vector2(-700, 400), Color.Black, 0f, Vector2.Zero, 1.9f, SpriteEffects.None, layer + 0.05f);
            }
            else if (animate == true)
            {
                spriteBatch.Draw(Sprites[CurrentIndex], Position, null, drawColor, Rotation, origin, scale, spriteEffect, layer);
            }
            else
            {
                spriteBatch.Draw(Sprites[maxIndex], Position, null, drawColor, Rotation, origin, scale, spriteEffect, layer);

                spriteBatch.DrawString(GameWorld.Instance.GameFont, start, new Vector2(0, -2000) + new Vector2(250, 400), Color.Black, 0f, Vector2.Zero, 1.9f, SpriteEffects.None, layer + 0.05f);
                spriteBatch.DrawString(GameWorld.Instance.GameFont, end, new Vector2(0, -2000) + new Vector2(-700, 400), Color.Black, 0f, Vector2.Zero, 1.9f, SpriteEffects.None, layer + 0.05f);
            }
        }
        #endregion

        #region Method

        #endregion
    }
}
