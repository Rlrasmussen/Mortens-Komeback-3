﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mortens_Komeback_3.Environment;
using System;

namespace Mortens_Komeback_3
{
    /// <summary>
    /// A tile used to make a grid, so AStar can find a path. 
    /// </summary>
    public class Tile : GameObject
    {
        private bool walkable = true;
        private Tile parent;


        public int G { get; set; }
        public int H { get; set; }
        public int F => G + H;

        public Tile Parent { get => parent; set => parent = value; }
        public bool Walkable
        {
            get => walkable;
            set
            {
                if (value == false && walkable == true)
                {
                    walkable = value;
                }
            }
        }


        public Tile(Enum type, Vector2 spawnPos) : base(type, spawnPos)
        {
            layer = 0.65f;
        }


        /// <summary>
        /// CollisionBox for tile
        /// Simon / Edited by Philip
        /// </summary>
        public override Rectangle CollisionBox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, 150, 150);
            }

        }
        /// <summary>
        /// Sets the tile as walkable, except when it overlaps with an obstacle or AvSurface. Then it is not walkable. 
        /// Philip
        /// </summary>
        public void SetWalkable()
        {
            walkable = true;
            foreach (GameObject go in GameWorld.Instance.GameObjects)
            {
                if (!(go is AvSurface || go is Obstacle))
                    continue;
                if (CollisionBox.Intersects(go.CollisionBox))
                {
                    Walkable = false;
                }

            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
                base.Draw(spriteBatch);
        }
    }
}
