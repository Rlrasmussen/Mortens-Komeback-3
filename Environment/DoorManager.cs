using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3.Environment
{
    class DoorManager : GameObject
    {
        public DoorManager(Enum type, Vector2 spawnPos) : base(type, spawnPos)
        {
        }

        public void SetupRooms()
        {
            Room roomA = new Room(RoomType.CatacombesA, new Vector2(0, 0));
            Room roomB = new Room(RoomType.CatacombesB, new Vector2(0, -1000));

            Door doorA = new Door(new Vector2(200, 400), DoorDirection.Top);
            Door doorB = new Door(new Vector2(200, 50), DoorDirection.Bottom);

            doorA.DestinationRoom = roomB;
            doorA.DestinationDoor = doorB;

            doorB.DestinationRoom = roomA;
            doorB.DestinationDoor = doorA;

            roomA.AddDoor(doorA);
            roomB.AddDoor(doorB);

            GameWorld.Instance.Rooms.Add(roomA);
            GameWorld.Instance.Rooms.Add(roomB);
            GameWorld.Instance.CurrentRoom = roomA;
        }
    }
}
