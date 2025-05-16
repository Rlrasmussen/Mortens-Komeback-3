using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3.Environment
{
    public class AvSurface : GameObject, IAnimate
    {
        #region Fields

        #endregion

        #region Properties

        public float FPS { get; set; }
        public Texture2D[] Sprites { get; set; }
        public float ElapsedTime { get; set; }
        public int CurrentIndex { get; set; }

        #endregion

        #region Constructor
        public AvSurface(Enum type, Vector2 spawnPos, float rotation) : base(type, spawnPos)
        {
            this.scale = 0.5f;
            FPS = 8;
            Sprites = GameWorld.Instance.Sprites[SurfaceType.AvSurface];
            Rotation = rotation;
        }

        #endregion

        #region Method

        public override void Update(GameTime gameTime)
        {
            (this as IAnimate).Animate();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Sprites != null)
                spriteBatch.Draw(Sprites[CurrentIndex], Position, null, drawColor, Rotation, origin, scale, spriteEffect, layer);
            else
                base.Draw(spriteBatch);
        }

        #endregion
    }
}
