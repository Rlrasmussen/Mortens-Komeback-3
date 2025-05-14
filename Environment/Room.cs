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
        bool currentRoom = false;
        private RoomType roomType;
        private static readonly Dictionary<RoomType, Vector2> spawnPoints = new()
        {
        {RoomType.PopeRoom, new Vector2(500, 500)},
        {RoomType.Stairs, new Vector2(1000, 1000)},
        {RoomType.CatacombesA, new Vector2(600, 400)},
        {RoomType.CatacombesB, new Vector2(600, 400)},
        {RoomType.CatacombesC, new Vector2(600, 400)},
        {RoomType.CatacombesD, new Vector2(600, 400)},
        {RoomType.CatacombesE, new Vector2(600, 400)},
        {RoomType.CatacombesF, new Vector2(600, 400)},
        {RoomType.CatacombesG, new Vector2(600, 400)},
        {RoomType.CatacombesH, new Vector2(600, 400)},
        {RoomType.TrapRoom, new Vector2(600, 400)},
        };
        #endregion

        #region Properties

        #endregion
        #region Constructor
        /// <summary>
        /// Irene
        /// </summary>
        /// <param name="type"></param>
        /// <param name="spawnPos"></param>
        public Room(Enum type, Vector2 spawnPos) : base(type, spawnPos)
        {
            scale = 1.5F;
            roomType = (RoomType)type;
            layer = 0f;
        }

        #endregion

        #region Method
        public void ActiveRoom(RoomType room)
        {
            currentRoom = true;
            
            switch (room)
            {
                case RoomType.PopeRoom:
                    Player.Instance.Position = spawnPoints[roomType];
                    Camera.Instance.Position = spawnPoints[roomType];
                    Debug.WriteLine("Entered: PopeRoom");
                    break;

                case RoomType.Stairs:
                    Player.Instance.Position = spawnPoints[roomType];
                    Camera.Instance.Position = spawnPoints[roomType];
                    Debug.WriteLine("Entered: Stairs");
                    break;

                case RoomType.CatacombesA:
                    Player.Instance.Position = spawnPoints[roomType];
                    break;

                case RoomType.CatacombesB:
                    Player.Instance.Position = spawnPoints[roomType];
                    break;

                case RoomType.CatacombesC:
                    Player.Instance.Position = spawnPoints[roomType];
                    break;

                case RoomType.CatacombesD:
                    Player.Instance.Position = spawnPoints[roomType];
                    break;

                case RoomType.CatacombesE:
                    Player.Instance.Position = spawnPoints[roomType];
                    break;

                case RoomType.CatacombesF:
                    Player.Instance.Position = spawnPoints[roomType];
                    break;

                case RoomType.CatacombesG:
                    Player.Instance.Position = spawnPoints[roomType];
                    break;

                case RoomType.CatacombesH:
                    Player.Instance.Position = spawnPoints[roomType];
                    break;

                case RoomType.TrapRoom:
                    Player.Instance.Position = spawnPoints[roomType];
                    break;

                default:
                    Debug.WriteLine("Unknown room");
                    break;
            }
        }

        //public Roomlist(Roomtype room)
        //{
            
        //}

        #endregion
    }
}
