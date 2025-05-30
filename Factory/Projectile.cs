using Microsoft.Xna.Framework;
using Mortens_Komeback_3.Command;
using Mortens_Komeback_3.Collider;
using Mortens_Komeback_3.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3.Factory
{
    public class Projectile : GameObject, ICollidable
    {
        #region Fields

        private Vector2 direction;
        private float speed = 1000f;
        private float rotateDirection;
        private float lifetime;

        #endregion

        #region Properties

        #endregion

        #region Constructor

        /// <summary>
        /// Projectiles constructor, sets damage the projectile does and handles sprite logic
        /// Simon
        /// </summary>
        /// <param name="type">Enum that represents what the object is, also determines sprite</param>
        /// <param name="spawnPos">Initial position of projectile (overrided by Load)</param>
        public Projectile(Enum type, Vector2 spawnPos) : base(type, spawnPos)
        {
            damage = 2;
        }

        #endregion

        #region Method

        /// <summary>
        /// (Re)sets projectile, including movement and rotation directions and runs basic (re)set logic
        /// Simon
        /// </summary>
        public override void Load()
        {

            Position = Player.Instance.Position;
            direction = InputHandler.Instance.MousePosition - Position;
            direction.Normalize();
            lifetime = 0f;

            if (Player.Instance.Position.X < InputHandler.Instance.MousePosition.X)
                rotateDirection = 0.15f;
            else
                rotateDirection = -0.15f;

            base.Load();

        }

        /// <summary>
        /// Effect that happens when a collision is triggered in GameWorld
        /// Simon
        /// </summary>
        /// <param name="other">Other object that was collided with</param>
        public void OnCollision(ICollidable other)
        {

            if (!(other is AvSurface))
            {
                ProjectilePool.Instance.ReleaseObject(this);
                IsAlive = false;
            }

        }

        /// <summary>
        /// Moves projectile in the direction that was given in load and "rotates" it at the same time
        /// Simon
        /// </summary>
        /// <param name="gameTime">DeltaTime, obsolete</param>
        public override void Update(GameTime gameTime)
        {

            Position += direction * speed * GameWorld.Instance.DeltaTime;
            Rotation += rotateDirection;
            lifetime += GameWorld.Instance.DeltaTime;

            if (lifetime > 3.5f)
            {
                ProjectilePool.Instance.ReleaseObject(this);
                IsAlive = false;
            }

        }

        #endregion
    }
}
