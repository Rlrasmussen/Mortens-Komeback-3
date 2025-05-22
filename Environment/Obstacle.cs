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
        private float moveTimer;
        private float moveTimerStop = 0.2f;
        private int speed = 500;

        /// <summary>
        /// An obstacle that can be collided with, and potentialy moved.
        /// </summary>
        /// <param name="type">The type of obstacle</param>
        /// <param name="spawnPos">Position the obstacle should spawn</param>
        /// <param name="movable">Wether the obstacle should be movable</param>
        public Obstacle(Enum type, Vector2 spawnPos, bool movable) : base(type, spawnPos)
        {
            this.movable = movable;
        }

        public override void Update(GameTime gameTime)
        {
            moveTimer += GameWorld.Instance.DeltaTime;
            Move();
        }
        /// <summary>
        /// Slides the obstacle 
        /// </summary>
        public void Move()
        {
            if (moveTimer < moveTimerStop)
            {
                //Only moves, if move doesn't move obstacle out of room
                // OBS!! Change to current room when available!!!
                if (!((Position.X + (velocity.X * speed * GameWorld.Instance.DeltaTime)) > DoorManager.Rooms.Find(x => (RoomType)x.Type == RoomType.PopeRoom).CollisionBox.Right)
                    && !((Position.X + (velocity.X * speed * GameWorld.Instance.DeltaTime)) < DoorManager.Rooms.Find(x => (RoomType)x.Type == RoomType.PopeRoom).CollisionBox.Left)
                    && !((Position.Y + (velocity.Y * speed * GameWorld.Instance.DeltaTime)) < DoorManager.Rooms.Find(x => (RoomType)x.Type == RoomType.PopeRoom).CollisionBox.Top)
                    && !((Position.Y + (velocity.X * speed * GameWorld.Instance.DeltaTime)) > DoorManager.Rooms.Find(x => (RoomType)x.Type == RoomType.PopeRoom).CollisionBox.Bottom))

                {
                    Position += velocity * speed * GameWorld.Instance.DeltaTime;
                }
            }
        }

        public void OnCollision(ICollidable other)
        {
            if (other is Player)
            {
                Vector2 tempVelocity = (other as Player).Velocity;
                if (!(tempVelocity == Vector2.Zero))
                {
                    velocity = tempVelocity;
                    velocity.Normalize();
                    moveTimer = 0;
                }

            }
            else if (other is Door)
            {
                Position -= velocity * speed * GameWorld.Instance.DeltaTime;
            }
        }
    }
}
