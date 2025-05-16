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
    public class Enemy : GameObject, IAnimate, ICollidable, IPPCollidable
    {
        #region Fields

        #endregion

        #region Properties
        public float FPS { get; set; } = 6;
        public Texture2D[] Sprites { get; set; }
        public float ElapsedTime { get; set; }
        public int CurrentIndex { get; set; }
        public int Health { get; set; }
        public List<RectangleData> Rectangles { get; set; } = new List<RectangleData>();


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
            (this as IPPCollidable).UpdateRectangles();

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Sprites != null)
                spriteBatch.Draw(Sprites[CurrentIndex], Position, null, drawColor, Rotation, origin, scale, spriteEffect, layer);
            else
                base.Draw(spriteBatch);
        }

        /// <summary>
        /// Reset Health with switch case by EnemyType
        /// Rikke
        /// </summary>
        public override void Load()
        {
            //Health switch case
            switch (this.type)
            {
                case EnemyType.WalkingGoose:
                    Health = 1;
                    this.Damage = 2;
                    break;
                case EnemyType.AggroGoose:
                    Health = 1;
                    this.Damage = 2;
                    break;
                case EnemyType.Goosifer:
                    Health = 1;
                    this.Damage = 2;
                    break;
            }

            base.Load();
        }

        /// <summary>
        /// Collision with Enemy
        /// </summary>
        /// <param name="other">IColliable</param>
        public void OnCollision(ICollidable other)
        {
            //Take damage by collision
            if (other is Projectile)
            {
                TakeDamage((GameObject)other);
            }

        }

        /// <summary>
        /// Enemy takes damage which makes the Health reduce. When Health is 0 en Enemy dies and will be released fra EnemyPool
        /// Rikke
        /// </summary>
        /// <param name="gameObject">GameObject which give damage to the Enemy</param>
        public void TakeDamage(GameObject gameObject)
        {
            Health -= gameObject.Damage;

            //Enemy dies is Health is 0
            if (Health <= 0)
            {
                IsAlive = false;

                EnemyPool.Instance.ReleaseObject(this);
            }
        }

        #endregion
    }
}
