using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3.Environment
{
    public class DoorManager : GameObject
    {
        public static List<Room> Rooms = new();
        public static List<Door> Doors = new();

        public static Dictionary<string, Door> doorList = new Dictionary<string, Door>();

        public DoorManager(Enum type, Vector2 spawnPos) : base(type, spawnPos)
        {
            
        }

        public static void Initialize()
        {
            // Create Rooms
            Room popeRoom = new Room(RoomType.PopeRoom, new Vector2(0, 0));
            Room stairs = new Room(RoomType.Stairs, new Vector2(0, 2000));
            Room catacombesA = new Room(RoomType.CatacombesA, new Vector2(0, 4000));
            Room catacombesA1 = new Room(RoomType.CatacombesA, new Vector2(2650, 4000));
            Room catacombesB = new Room(RoomType.CatacombesB, new Vector2(0, 6000));
            Room catacombesC = new Room(RoomType.CatacombesC, new Vector2(0, 8000));
            Room catacombesD = new Room(RoomType.CatacombesD, new Vector2(0, 10000));
            Room catacombesD1 = new Room(RoomType.CatacombesD, new Vector2(0, 11500));
            Room catacombesE = new Room(RoomType.CatacombesE, new Vector2(0, 14000));
            Room catacombesF = new Room(RoomType.TrapRoom, new Vector2(0, 16000));
            Room catacombesG = new Room(RoomType.CatacombesF, new Vector2(0, 18000));
            Room catacombesH = new Room(RoomType.CatacombesG, new Vector2(0, 20000));
            Room trapRoom = new Room(RoomType.CatacombesH, new Vector2(0, 22000));

            //*3 + 275

            // Create Doors
            Door doorA1 = new Door(new Vector2(1190, 0), DoorDirection.Right);
            Door doorA2 = new Door(new Vector2(-1190, 2000), DoorDirection.Left);
            Door doorB1 = new Door(new Vector2(1190, 2000), DoorDirection.Right); //match 3 puzzle
            Door doorB2 = new Door(new Vector2(-1190, 4000), DoorDirection.Left);
            Door doorC1 = new Door(new Vector2(1190 * 3 + 275, 4000), DoorDirection.Right); //catacombesA
            Door doorC2 = new Door(new Vector2(-1190, 6000), DoorDirection.Left);
            Door doorD1 = new Door(new Vector2(1190, 6000), DoorDirection.Right); //shoot puzzle
            Door doorD2 = new Door(new Vector2(-1190, 8000), DoorDirection.Left);
            Door doorE1 = new Door(new Vector2(1190, 8000), DoorDirection.Right);
            Door doorE2 = new Door(new Vector2(-1190, 10000), DoorDirection.Left);
            Door doorF1 = new Door(new Vector2(0, 12100), DoorDirection.Bottom);
            Door doorF2 = new Door(new Vector2(-800, 13400), DoorDirection.Top);    //tilbage til catacombesD fra catacombesE
            Door doorG1 = new Door(new Vector2(600, 13400), DoorDirection.Top);     //catacombesE til traproom
            Door doorH1 = new Door(new Vector2(1190, 14000), DoorDirection.Right);  //catacombesE til catacombesF //////
            Door doorG3 = new Door(new Vector2(-800, 16600), DoorDirection.Bottom);  //videre
            Door doorG2 = new Door(new Vector2(600, 16000), DoorDirection.Top);  //traproom
            Door doorH2 = new Door(new Vector2(-1190, 18000), DoorDirection.Left);  //traproom
            Door doorI1 = new Door(new Vector2(1190, 18000), DoorDirection.Right);
            Door doorI2 = new Door(new Vector2(-1190, 20000), DoorDirection.Left);
            Door doorJ1 = new Door(new Vector2(1190, 20000), DoorDirection.Right);
            Door doorJ2 = new Door(new Vector2(-1190, 22000), DoorDirection.Left);

            doorList.Add("doorA1", doorA1);
            doorList.Add("doorA2", doorA2);
            doorList.Add("doorB1", doorB1);
            doorList.Add("doorB2", doorB2);
            doorList.Add("doorC1", doorC1);
            doorList.Add("doorC2", doorC2);
            doorList.Add("doorD1", doorD1);
            doorList.Add("doorD2", doorD2);
            doorList.Add("doorE1", doorE1);
            doorList.Add("doorE2", doorE2);
            doorList.Add("doorF1", doorF1);
            doorList.Add("doorF2", doorF2);
            doorList.Add("doorG1", doorG1);
            doorList.Add("doorG2", doorG2);
            doorList.Add("doorG3", doorG3);
            doorList.Add("doorH1", doorH1);
            doorList.Add("doorH2", doorH2);
            doorList.Add("doorI1", doorI1);
            doorList.Add("door12", doorI2);
            doorList.Add("doorJ1", doorJ1);
            doorList.Add("doorJ2", doorJ2);


            // Assign doors to rooms
            popeRoom.AddDoor(doorA1);
            stairs.AddDoor(doorA2);
            stairs.AddDoor(doorB1);
            catacombesA.AddDoor(doorB2);
            catacombesA1.AddDoor(doorC1);
            catacombesB.AddDoor(doorC2);
            catacombesB.AddDoor(doorD1);
            catacombesC.AddDoor(doorD2);
            catacombesC.AddDoor(doorE1);
            catacombesD.AddDoor(doorE2);
            catacombesD1.AddDoor(doorF1);
            catacombesE.AddDoor(doorF2);
            catacombesE.AddDoor(doorG1);
            catacombesE.AddDoor(doorG2);

            trapRoom.AddDoor(doorG2);
            trapRoom.AddDoor(doorG3);

            catacombesF.AddDoor(doorH2);
            catacombesF.AddDoor(doorI1);
            catacombesG.AddDoor(doorI2);
            catacombesG.AddDoor(doorJ1);

            // Link doors
            doorA1.LinkTo(doorA2);
            doorB1.LinkTo(doorB2);
            doorC1.LinkTo(doorC2);
            doorD1.LinkTo(doorD2);
            doorE1.LinkTo(doorE2);
            doorF1.LinkTo(doorF2);
            doorG1.LinkTo(doorG2); // husk at blokér at man kan gå tilbage!!
            doorG3.LinkTo(doorG1); // husk !!
            doorH2.LinkTo(doorI1);
            doorI2.LinkTo(doorJ1);
            doorJ2.LinkTo(doorH2);//
            doorH1.LinkTo(doorH2);
            doorI1.LinkTo(doorI2);
            doorJ1.LinkTo(doorJ2);
            doorH1.LinkTo(doorH2);
            //mangler stadig lidt links pga traproom

            // Store for gameworld or rendering
            Rooms.Add(popeRoom);
            Rooms.Add(stairs);
            Rooms.Add(catacombesA);
            Rooms.Add(catacombesA1);
            Rooms.Add(catacombesB);
            Rooms.Add(catacombesC);
            Rooms.Add(catacombesD);
            Rooms.Add(catacombesD1);
            Rooms.Add(catacombesE);
            Rooms.Add(catacombesF);
            Rooms.Add(catacombesG);
            Rooms.Add(catacombesH);
            Rooms.Add(trapRoom);

            Doors.Add(doorA1);
            Doors.Add(doorA2);
            Doors.Add(doorB1);
            Doors.Add(doorB2);
            Doors.Add(doorC1);
            Doors.Add(doorC2);
            Doors.Add(doorD1);
            Doors.Add(doorD2); 
            Doors.Add(doorE1);
            Doors.Add(doorE2); 
            Doors.Add(doorF1);
            Doors.Add(doorF2); 
            Doors.Add(doorG1);
            Doors.Add(doorG2); 
            Doors.Add(doorG3);
            Doors.Add(doorH1);
            Doors.Add(doorH2);
            Doors.Add(doorI1);
            Doors.Add(doorI2);       
            Doors.Add(doorJ1);
            Doors.Add(doorJ2);
        }

    }
}
