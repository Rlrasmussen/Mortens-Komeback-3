using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private int speed = 500;
        private Room obstacleRoom;
        private Vector2 originalPosition;

        public bool Movable { get => movable; set => movable = value; }
        public Vector2 OriginalPosition { get => originalPosition; set => originalPosition = value; }

        /// <summary>
        /// An obstacle that can be collided with, and potentialy moved.
        /// </summary>
        /// <param name="type">The type of obstacle</param>
        /// <param name="spawnPos">Position the obstacle should spawn</param>
        /// <param name="movable">Wether the obstacle should be movable</param>
        public Obstacle(Enum type, Vector2 spawnPos, bool movable, Room room) : base(type, spawnPos)
        {
            this.Movable = movable;
            this.obstacleRoom = room;
            OriginalPosition = spawnPos;
        }

        public override void Update(GameTime gameTime)
        {
            if (Movable)
            {
                Move();
            }
            if (GameWorld.Instance.CurrentRoom != obstacleRoom)
            {
                Position = OriginalPosition;
            }
        }
        /// <summary>
        /// Slides the obstacle 
        /// </summary>
        public void Move()
        {
            //Only moves, new position isn't out of room
            if (!((Position.X + (velocity.X * speed * GameWorld.Instance.DeltaTime)) > obstacleRoom.CollisionBox.Right)
                && !((Position.X + (velocity.X * speed * GameWorld.Instance.DeltaTime)) < obstacleRoom.CollisionBox.Left)
                && !((Position.Y + (velocity.Y * speed * GameWorld.Instance.DeltaTime)) < obstacleRoom.CollisionBox.Top)
                && !((Position.Y + (velocity.Y * speed * GameWorld.Instance.DeltaTime)) > obstacleRoom.CollisionBox.Bottom))

            {
                Position += velocity * speed * GameWorld.Instance.DeltaTime;
            }
            velocity = Vector2.Zero;
        }

        public void OnCollision(ICollidable other)
        {
            if (Movable)
            {
                if (other is Player)
                {
                    if (Player.Instance.Position.X < CollisionBox.Left)
                    {
                        velocity.X += 1;
                    }
                    else if (Player.Instance.Position.X > CollisionBox.Right)
                    {
                        velocity.X -= 1;
                    }
                    if (Player.Instance.Position.Y < CollisionBox.Top)
                    {
                        velocity.Y += 1;
                    }
                    else if (Player.Instance.Position.Y > CollisionBox.Bottom)
                    {
                        velocity.Y -= 1;
                    }
                    if (velocity != Vector2.Zero)
                    {
                        velocity.Normalize();
                    }

                }
                else if (other is Door)
                {
                    Position -= velocity * speed * GameWorld.Instance.DeltaTime;
                }
                if (other is Obstacle)
                {

                    if (other.Position.X < CollisionBox.Left)
                    {
                        velocity.X += 1;
                    }
                    else if (other.Position.X > CollisionBox.Right)
                    {
                        velocity.X -= 1;
                    }
                    if (other.Position.Y < CollisionBox.Top)
                    {
                        velocity.Y += 1;
                    }
                    else if (other.Position.Y > CollisionBox.Bottom)
                    {
                        velocity.Y -= 1;
                    }
                    if (velocity != Vector2.Zero)
                    {
                        velocity.Normalize();
                    }

                }
            }
        }
    }
}
