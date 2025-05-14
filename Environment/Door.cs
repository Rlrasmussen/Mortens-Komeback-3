using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mortens_Komeback_3.Collider;


namespace Mortens_Komeback_3.Environment
{
    public class Door : GameObject, ICollidable
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructor
        public Door(Enum type, Vector2 spawnPos, DoorDirection direction) : base(type, spawnPos)
        {
            Rotation = GetDoorDirection(direction);
        }

        #endregion

        #region Method


        /// <summary>
        /// Roterer dørene
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
            //if (other == Player.Instance)
            //{
            //    TeleportPlayer();
            //}
            //throw new NotImplementedException();
        }

        /// <summary>
        /// When player collides with door, players position is moved to next room
        /// </summary>
        /// <param name="room"></param>
        //public void TeleportPlayer(RoomType room)
        //{
        //    Player.Instance.Position = RoomType;
        //}
      
        #endregion
    }
}
