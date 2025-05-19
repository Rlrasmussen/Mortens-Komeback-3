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
        #endregion

        #region Properties
        public RoomType RoomType { get; private set; }
        public List <Door> Doors { get; private set; }
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
    

        #endregion
    }
}
