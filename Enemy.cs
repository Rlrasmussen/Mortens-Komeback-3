using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mortens_Komeback_3.Collider;
using Mortens_Komeback_3.Factory;

namespace Mortens_Komeback_3
{
    public class Enemy : GameObject, IAnimate, ICollidable
    {
        #region Fields

        #endregion

        #region Properties
        public float FPS { get; set; } = 6;
        public Texture2D[] Sprites { get; set; }
        public float ElapsedTime { get; set; }
        public int CurrentIndex { get; set; }
        public int Health { get; set; }


        #endregion

        #region Constructor
        public Enemy(Enum type, Vector2 spawnPos) : base(type, spawnPos)
        {
            if (GameWorld.Instance.Sprites.TryGetValue(type, out var sprites))
                Sprites = sprites;
            else
                Debug.WriteLine("Kunne ikke sætte sprites for " + ToString());

        }


        #endregion

        #region Method
        public override void Update(GameTime gameTime)
        {
            (this as IAnimate).Animate();

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Sprites != null)
                spriteBatch.Draw(Sprites[CurrentIndex], Position, null, drawColor, Rotation, origin, scale, spriteEffect, layer);
            else
                base.Draw(spriteBatch);
        }

        public override void Load()
        {
            Health = 1;
            
            base.Load();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        public void OnCollision(ICollidable other)
        {
            if (other is Player)
            {
                Health--;

                if (Health == 0)
                {
                    IsAlive = false;

                    EnemyPool.Instance.ReleaseObject(this);
                }
            }
        }

        public void TakeDamage()
        {
            Health--;

            if (Health == 0)
            {
                IsAlive = false;

                EnemyPool.Instance.ReleaseObject(this);
            }
        }
        #endregion
    }
}
