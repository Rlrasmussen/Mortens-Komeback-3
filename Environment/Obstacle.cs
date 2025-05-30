﻿using System;
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
        private float moveTimer;
        private float moveTimerStop = 0.2f;
        private int speed = 500;
        private Room obstacleRoom;

        public bool Movable { get => movable; set => movable = value; }

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
            if (moveTimer < moveTimerStop && Movable)
            {
                //Only moves, if move doesn't move obstacle out of room
                if (!((Position.X + (velocity.X * speed * GameWorld.Instance.DeltaTime)) > obstacleRoom.CollisionBox.Right)
                    && !((Position.X + (velocity.X * speed * GameWorld.Instance.DeltaTime)) < obstacleRoom.CollisionBox.Left)
                    && !((Position.Y + (velocity.Y * speed * GameWorld.Instance.DeltaTime)) < obstacleRoom.CollisionBox.Top)
                    && !((Position.Y + (velocity.Y * speed * GameWorld.Instance.DeltaTime)) > obstacleRoom.CollisionBox.Bottom))

                {
                    Position += velocity * speed * GameWorld.Instance.DeltaTime;
                }
            }
        }

        public void OnCollision(ICollidable other)
        {
            if (Movable)
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
                if (other is Obstacle)
                {
                    if ((other as Obstacle).Movable ==  false)
                    {
                        Position -= velocity * speed * GameWorld.Instance.DeltaTime;
                    }
                    else
                    {
                        Vector2 tempVelocity = (other as Obstacle).velocity;
                        if (!(tempVelocity == Vector2.Zero))
                        {
                            velocity = tempVelocity;
                            velocity.Normalize();
                            moveTimer = 0;
                        }
                    }
                }
            }
        }
    }
}
