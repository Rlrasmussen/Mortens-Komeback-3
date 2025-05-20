using Microsoft.Xna.Framework;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3
{
    public class Tile : GameObject
    {
        private bool walkable = true;
        private bool fencePath = false;
        //   private HashSet<Edge> edges = new HashSet<Edge>();
        private bool discovered = false;
        private Tile parent;

        public int G { get; set; }
        public int H { get; set; }
        public int F => G + H;

        //public HashSet<Edge> Edges { get => edges; }
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
        public bool FencePath { get => fencePath; }

        public Tile(Enum type, Vector2 spawnPos, bool fencePath) : base(type, spawnPos)
        {

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
    }
}
