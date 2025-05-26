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
        private bool leftSideOfBigRoom = false;
        private bool rightSideOfBigRoom = false;
        private bool topSideOfBigRoom = false;
        private bool buttomSideOfBigRoom = false;

        #endregion

        #region Properties
        public RoomType RoomType { get; private set; }
        public List<Door> Doors { get; private set; }
        public Dictionary<Vector2, Tile> Tiles { get => tiles; set => tiles = value; }
        public bool LeftSideOfBigRoom { get => leftSideOfBigRoom; set => leftSideOfBigRoom = value; }
        public bool RightSideOfBigRoom { get => rightSideOfBigRoom; set => rightSideOfBigRoom = value; }
        public bool TopSideOfBigRoom { get => topSideOfBigRoom; set => topSideOfBigRoom = value; }
        public bool ButtomSideOfBigRoom { get => buttomSideOfBigRoom; set => buttomSideOfBigRoom = value; }

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
            //scale = 1.5F;
            layer = 0.1f;

        }

        #endregion

        #region Method
        public void AddDoor(Door door)
        {
            Doors.Add(door);
            door.room = this;
        }


        /// <summary>
        /// Adds a grid of tiles to the room, used by AStar algorithm. 
        /// Philip
        /// </summary>
        public void AddTiles()
        {
            int tilesX = CollisionBox.Width / 150;
            int tilesY = CollisionBox.Height / 150;
            if(LeftSideOfBigRoom || RightSideOfBigRoom)
            {
                tilesX = (CollisionBox.Width * 2) / 150;
            }
            if (TopSideOfBigRoom || ButtomSideOfBigRoom)
            {
                tilesY = (CollisionBox.Height * 2) / 150;
            }
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
