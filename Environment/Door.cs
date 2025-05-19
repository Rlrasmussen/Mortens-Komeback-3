using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mortens_Komeback_3.Collider;
using System.Diagnostics;


namespace Mortens_Komeback_3.Environment
{
    public class Door : GameObject, ICollidable
    {
        #region Fields
        private Vector2 teleportPosition = new Vector2(500, 500);
        private DoorType doorStatus;

        #endregion

        #region Properties
        public DoorType DoorStatus
        {
            get => doorStatus;
            set
            {
                doorStatus = value;
                if (value == DoorType.Locked)
                {
                    Sprite = GameWorld.Instance.Sprites[DoorType.Locked][0];
                }
            }

        }
        public DoorDirection Direction { get; private set; }
        public float DoorOffset { get; private set; }
        public Room room { get; set; }

        public Room DestinationRoom { get; set; }
        public Door DestinationDoor { get; set; }
        #endregion

        #region Constructor
        public Door(Vector2 spawnPos, DoorDirection direction, DoorType initialType = DoorType.Closed) : base(initialType, spawnPos)
        {

            DoorStatus = initialType;
            Rotation = GetDoorDirection(direction); //Sprite rotation
            Direction = direction;
            layer = 0.11f;
        }

        #endregion

        #region Method
        /// <summary>
        /// Irene
        /// </summary>
        public void UnlockDoor()
        {
            if (DoorStatus == DoorType.Locked)
            {
                DoorStatus = DoorType.Closed;
                Debug.WriteLine("room unlocked");
                Sprite = GameWorld.Instance.Sprites[DoorStatus][0];
            }
        }

        /// <summary>
        /// Irene
        /// </summary>
        public void OpenDoor()
        {
            if (DoorStatus == DoorType.Closed)
            {
                DoorStatus = DoorType.Open;
                Debug.WriteLine("room open");
            }
        }





        /// <summary>
        /// Roterer dørene
        /// Irene
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public float GetDoorDirection(DoorDirection direction)
        {

            switch (direction)
            {
                case DoorDirection.Top:
                    return 0f;
                case DoorDirection.Right:
                    return (float)(Math.PI / 2); // 90 grader
                case DoorDirection.Bottom:
                    return (float)(Math.PI); // 180 grader
                case DoorDirection.Left:
                    return (float)(-Math.PI / 2); // -90 grader
                default:
                    return 0f;

            }

        }



        public void OnCollision(ICollidable other)
        {
            //if (other == Player.Instance &&
            //    (DoorStatus == DoorType.Closed || DoorStatus == DoorType.Open) &&
            //    DestinationRoom != null && DestinationDoor != null)
            //{
            //    Player.Instance.Position = DestinationDoor.Position;
            //    Player.Instance.Position = Vector2.Zero;
            //    GameWorld.Instance.CurrentRoom = DestinationRoom;
            //    //Debug.WriteLine("Player teleported to " + DestinationRoom.RoomType);
            //}

            if (other == Player.Instance && (DoorStatus == DoorType.Closed || DoorStatus == DoorType.Open))
            {
                Player.Instance.Position = new Vector2(DestinationDoor.Position.X + 180, DestinationDoor.Position.Y);
                GameWorld.Instance.CurrentRoom = DestinationRoom;
                //Debug.WriteLine("Player teleported to " + DestinationRoom.RoomType);
                GameWorld.Instance.Sounds[Sound.PlayerDamage].Play();
            }

        }

        public void LinkTo(Door otherDoor)
        {
            this.DestinationDoor = otherDoor;
            this.DestinationRoom = otherDoor.room;

            otherDoor.DestinationDoor = this;
            otherDoor.DestinationRoom = this.room;
        }


        /// <summary>
        /// When player collides with door, players position is moved to next room
        /// </summary>
        /// <param name = "room" ></ param >
        //public void TeleportPlayer()
        //{
        //    Player.Instance.Position = teleportPosition;
        //    //    Debug.WriteLine("teleport");
        //}

        #endregion
    }
}
