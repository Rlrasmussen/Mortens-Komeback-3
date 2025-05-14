using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Mortens_Komeback_3.Environment
{
    public class Room : GameObject
    {
        #region Fields
        bool currentRoom = false;
        private RoomType roomType;
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
        }

        #endregion

        #region Method
        public void ActiveRoom(RoomType room)
        {
            currentRoom = true;
            
            switch (room)
            {
                case RoomType.PopeRoom:
                    Player.Instance.Position = new Vector2(500, 500);
                    Debug.WriteLine("Entered: PopeRoom");
                    break;

                case RoomType.Stairs:
                    Player.Instance.Position = new Vector2(1000, 1000);
                    Debug.WriteLine("Entered: Stairs");
                    break;

                case RoomType.CatacombesA:
                    Player.Instance.Position = new Vector2(600, 400);
                    break;

                case RoomType.CatacombesB:
                    Player.Instance.Position = new Vector2(650, 420);
                    break;

                case RoomType.CatacombesC:
                    Player.Instance.Position = new Vector2(700, 440);
                    break;

                case RoomType.CatacombesD:
                    Player.Instance.Position = new Vector2(750, 460);
                    break;

                case RoomType.CatacombesE:
                    Player.Instance.Position = new Vector2(800, 480);
                    break;

                case RoomType.CatacombesF:
                    Player.Instance.Position = new Vector2(850, 500);
                    break;

                case RoomType.CatacombesG:
                    Player.Instance.Position = new Vector2(900, 520);
                    break;

                case RoomType.CatacombesH:
                    Player.Instance.Position = new Vector2(950, 540);
                    break;

                case RoomType.TrapRoom:
                    Player.Instance.Position = new Vector2(1000, 560);
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
