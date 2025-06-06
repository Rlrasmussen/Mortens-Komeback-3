﻿using Microsoft.Xna.Framework;
using Mortens_Komeback_3.Collider;
using Mortens_Komeback_3.Environment;
using System;

namespace Mortens_Komeback_3.State
{

    public class GoosiferFire : AvSurface
    {
        #region Field
        private IState<GoosiferFire> movement;
        private Enemy shooter;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor based of AvSurface
        /// Simon
        /// </summary>
        /// <param name="type">Sprite(s) of object</param>
        /// <param name="spawnPos">Objects starting position</param>
        /// <param name="rotation">Objects starting rotation</param>
        /// <param name="damage">Damage the object will "apply" if colliding with another object</param>
        public GoosiferFire(Enum type, Vector2 spawnPos, float rotation, int damage, Enemy shooter) : base(type, spawnPos, rotation)
        {

            Damage = damage;
            Sprites = null; //Delete if sprites are set for a fire animation
            FireState attackMorten = new FireState();
            attackMorten.Enter(this);
            movement = attackMorten;
            this.shooter = shooter;
            this.scale = 0.8f;

        }
        #endregion

        #region Method
        /// <summary>
        /// Handles movement and (if applicable, animation of sprites)
        /// Simon
        /// </summary>
        /// <param name="gameTime">DeltaTime (obsolete)</param>
        public override void Update(GameTime gameTime)
        {

            if (shooter != null && !shooter.IsAlive)
                movement.Exit();

            if (movement != null)
                movement.Execute();

            if (Sprites != null)
                base.Update(gameTime);

        }

        /// <summary>
        /// "Kills" projectile, and if hitting player, damages him and does a knockback-effect
        /// Simon
        /// </summary>
        /// <param name="other">Other object collided with</param>
        public override void OnCollision(ICollidable other)
        {

            base.OnCollision(other);
            movement.Exit();
        }
        #endregion
    }















    public class FireState : IState<GoosiferFire>
    {
        #region Field
        private GoosiferFire parent;
        private Vector2 direction;
        private float lifetime = -5f;
        private float speed = 700f;
        #endregion

        #region Properties
        public bool OverridesPathfinding { get; set; }
        #endregion

        #region Method
        /// <summary>
        /// Handles starting logic of the State
        /// Simon
        /// </summary>
        /// <param name="parent">Object that owns the State/subject of its effects</param>
        public void Enter(GoosiferFire parent)
        {

            this.parent = parent;
            direction = Player.Instance.Position - parent.Position;
            direction.Normalize();
            parent.Rotation = GetAngle();

        }

        /// <summary>
        /// Handles movement and lifespan duration of the projectile
        /// Simon
        /// </summary>
        public void Execute()
        {

            lifetime += GameWorld.Instance.DeltaTime;

            parent.Position += direction * speed * GameWorld.Instance.DeltaTime;

            if (lifetime >= 0)
                Exit();

        }

        /// <summary>
        /// Kill/end logic
        /// Simon
        /// </summary>
        public void Exit()
        {
            
            parent.IsAlive = false;

        }

        /// <summary>
        /// Gets radians for rotation of object to simulate the direction of the fireball (Old bit from another issue ChatGPT helped with)
        /// Simon
        /// </summary>
        /// <returns>Radians</returns>
        private float GetAngle()
        {
            Vector2 direction = parent.Position - Player.Instance.Position;
            float angleRadians = (float)Math.Atan2(direction.Y, direction.X);
            return angleRadians;
        }
        #endregion
    }

}
