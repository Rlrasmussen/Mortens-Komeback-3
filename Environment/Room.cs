using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using SharpDX.Direct2D1;

namespace Mortens_Komeback_3.Environment
{
    public class Room : GameObject
    {
        #region Fields
        private Room currentRoom;
        private RoomType roomType;
        private Dictionary<Vector2, Tile> tiles = new Dictionary<Vector2, Tile>();
        #endregion

        #region Properties
        public RoomType RoomType { get; private set; }
        public List<Door> Doors { get; private set; }
        public Dictionary<Vector2, Tile> Tiles { get => tiles; set => tiles = value; }

        #endregion
        #region Constructor
        /// <summary>
        /// Irene
        /// </summary>
        /// <param name="type"></param>
        /// <param name="spawnPos"></param>
        public Room(RoomType type, Vector2 spawnPos) : base(type, spawnPos)
        {
            RoomType = type;
            Doors = new List<Door>();
            scale = 1.5F;
            layer = 0.1f;

        }

        #endregion

        #region Method
        public void AddDoor(Door door)
        {
            Doors.Add(door);
        }


        /// <summary>
        /// Adds a grid of tiles to the room, used by AStar algorithm. 
        /// </summary>
        public void AddTiles()
        {
            int tilesX = CollisionBox.Width / 150;
            int tilesY = CollisionBox.Height / 150;
            for (int i = 1; i < tilesX - 1; i++)
            {
                for (int j = 1; j < tilesY; j++)
                {
                    Tile t = new Tile(TileEnum.Tile, new Vector2(CollisionBox.Left + (i * 150), CollisionBox.Top + (j * 150)));
                    Tiles.Add(t.Position, t);
                }
            }
            foreach (var tile in tiles)
            {
                tile.Value.SetWalkable();
            }
        }
        #endregion
    }
}
