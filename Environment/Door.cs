﻿using Microsoft.Xna.Framework;
using Mortens_Komeback_3.Collider;
using Mortens_Komeback_3.Puzzles;
using System;
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
                if (value == DoorType.StairsLocked)
                {
                    Sprite = GameWorld.Instance.Sprites[DoorType.StairsLocked][0];
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
        public Door(Vector2 spawnPos, DoorDirection direction, DoorType initialType = DoorType.Locked) : base(initialType, spawnPos)
        {

            DoorStatus = initialType;
            Rotation = GetDoorDirection(direction); //Sprite rotation
            Direction = direction;
            layer = 0.11f;
        }

        #endregion

        #region Method
        /// <summary>
        /// Unlocks door/stairs - sprite changes to closed and is now collidable
        /// Irene
        /// </summary>
        public void UnlockDoor()
        {
            if (DoorStatus == DoorType.Locked)
            {
                DoorStatus = DoorType.Closed;
#if DEBUG
                Debug.WriteLine("room unlocked");
#endif
                Sprite = GameWorld.Instance.Sprites[DoorStatus][0];
            }
            else if (DoorStatus == DoorType.StairsLocked)
            {
                DoorStatus = DoorType.Stairs;
                Sprite = GameWorld.Instance.Sprites[DoorType.Stairs][0];
            }
        }

        /// <summary>
        /// Changes sprite/enum to open
        /// Irene
        /// </summary>
        public void OpenDoor()
        {
            if (DoorStatus == DoorType.Closed)
            {
                DoorStatus = DoorType.Open;
#if DEBUG
                Debug.WriteLine("room open");
#endif
            }
        }





        /// <summary>
        /// Rotating door sprite
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


        /// <summary>
        /// When Player collides with door, player is teleported to a new position
        /// Irene
        /// </summary>
        /// <param name="other"></param>
        public void OnCollision(ICollidable other)
        {

            if (other == Player.Instance && (DoorStatus == DoorType.Closed || DoorStatus == DoorType.Open || DoorStatus == DoorType.Stairs || DoorStatus == DoorType.StairsUp))
            {
                if ((RoomType)room.Type == RoomType.CatacombesF)
                {
                    GameObject tempPathPuzzle = GameWorld.Instance.gamePuzzles.Find(x => (PuzzleType)x.Type == PuzzleType.PathfindingPuzzle);
                    foreach (Obstacle obstacle in (tempPathPuzzle as PathfindingPuzzle).PuzzleObstacles)
                    {
                        obstacle.Position = obstacle.OriginalPosition;
                    }
                }

                if (DoorStatus == DoorType.Locked || DoorStatus == DoorType.StairsLocked)
                {
                    return; // Stop passage
                }
                //// Tjek om fjender stadig er i live i det nuværende rum
                //bool enemiesAlive = GameWorld.Instance.GameObjects
                //    .OfType<Enemy>()
                //    .Any(enemy => enemy.Position.Y > room.Position.Y && enemy.Position.Y < room.Position.Y + 2000 && enemy.IsAlive); // Tilpas evt. grænser

                //if (enemiesAlive /*&& DoorStatus == DoorType.Locked*/)
                //{
                //    DoorStatus = DoorType.Locked;
                //    return; // Stop spillerens teleportering
                //}
                //else
                //{
                //    DoorStatus = DoorType.Closed;
                //}


                GameWorld.Instance.CurrentRoom = DestinationRoom;

                switch (Direction)
                {
                    case DoorDirection.Top:
                        Player.Instance.Position = new Vector2(DestinationDoor.Position.X, DestinationDoor.Position.Y - 180);
                        break;
                    case DoorDirection.Right:
                        Player.Instance.Position = new Vector2(DestinationDoor.Position.X + 320, DestinationDoor.Position.Y);
                        break;
                    case DoorDirection.Bottom:
                        Player.Instance.Position = new Vector2(DestinationDoor.Position.X, DestinationDoor.Position.Y + 180);
                        break;
                    case DoorDirection.Left:
                        Player.Instance.Position = new Vector2(DestinationDoor.Position.X - 320, DestinationDoor.Position.Y);
                        break;
                    default:
                        break;
                }
            }

        }

        /// <summary>
        /// 
        /// Irene
        /// </summary>
        /// <param name="otherDoor"></param>
        public void LinkTo(Door otherDoor)
        {
            this.DestinationDoor = otherDoor;
            this.DestinationRoom = otherDoor.room;

            otherDoor.DestinationDoor = this;
            otherDoor.DestinationRoom = this.room;
        }


        #endregion
    }
}
