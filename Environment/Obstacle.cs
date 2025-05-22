using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Mortens_Komeback_3.Collider;

namespace Mortens_Komeback_3.Environment
{
    class Obstacle : GameObject, ICollidable
    {
        private bool movable;
        private Vector2 velocity;
        public Obstacle(Enum type, Vector2 spawnPos, bool movable) : base(type, spawnPos)
        {
            this.movable = movable;
        }

        public override void Update(GameTime gameTime)
        {
            Position += velocity * 500 * GameWorld.Instance.DeltaTime;
        }

        public void OnCollision(ICollidable other)
        {
            if (other is Player)
            {
                velocity = (other as Player).Velocity;
                if (!(velocity == Vector2.Zero))
                    velocity.Normalize();
            }
        }
    }
}
