using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mortens_Komeback_3.Environment;
using SharpDX.Direct2D1.Effects;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3
{
    /// <summary>
    /// A tile used to make a grid, so AStar can find a path. 
    /// </summary>
    public class Tile : GameObject
    {
        private bool walkable = true;
        private bool discovered = false;
        private Tile parent;
      

        public int G { get; set; }
        public int H { get; set; }
        public int F => G + H;

        public bool Discovered { get => discovered; set => discovered = value; }
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

        public bool ShowTile { get; set; } = false;

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
