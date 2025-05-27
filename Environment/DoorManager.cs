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
            Room catacombesA = new Room(RoomType.CatacombesA, new Vector2(0, 4000), GetEnemies(RoomType.CatacombesA));
            catacombesA.LeftSideOfBigRoom = true;
            Room catacombesA1 = new Room(RoomType.CatacombesA1, new Vector2(2695, 4000));
            catacombesA1.RightSideOfBigRoom = true;
            Room catacombesB = new Room(RoomType.CatacombesB, new Vector2(0, 6000));
            Room catacombesC = new Room(RoomType.CatacombesC, new Vector2(0, 8000), GetEnemies(RoomType.CatacombesC));
            Room catacombesD = new Room(RoomType.CatacombesD, new Vector2(0, 10000), GetEnemies(RoomType.CatacombesD));
            catacombesD.TopSideOfBigRoom = true;
            Room catacombesD1 = new Room(RoomType.CatacombesD1, new Vector2(0, 11515));
            catacombesD1.ButtomSideOfBigRoom = true;
            Room catacombesE = new Room(RoomType.CatacombesE, new Vector2(0, 14000));
            Room catacombesF = new Room(RoomType.CatacombesF, new Vector2(0, 16000));
            Room catacombesG = new Room(RoomType.CatacombesG, new Vector2(0, 18000));
            Room catacombesH = new Room(RoomType.CatacombesH, new Vector2(0, 20000), GetEnemies(RoomType.CatacombesH));
            Room trapRoom = new Room(RoomType.TrapRoom, new Vector2(0, 22000));

            Room cutscene = new Room(RoomType.Curscene, new Vector2(0, -2000));

            popeRoom.AddTiles();

            // Create Doors
            Door doorA1 = new Door(new Vector2(1190, 0), DoorDirection.Right);
            Door doorA2 = new Door(new Vector2(-1190, 2000), DoorDirection.Left);
            Door doorB1 = new Door(new Vector2(880, 2000), DoorDirection.Right, DoorType.StairsLocked);
            Door doorB2 = new Door(new Vector2(-945, 4000), DoorDirection.Left, DoorType.StairsUp);
            Door doorC1 = new Door(new Vector2(1190 * 3 + 320, 4000), DoorDirection.Right); //catacombesA
            Door doorC2 = new Door(new Vector2(-1190, 6000), DoorDirection.Left);
            Door doorD1 = new Door(new Vector2(1190, 6000), DoorDirection.Right); //shoot puzzle
            Door doorD2 = new Door(new Vector2(-1190, 8000), DoorDirection.Left);
            Door doorE1 = new Door(new Vector2(1190, 8000), DoorDirection.Right);
            Door doorE2 = new Door(new Vector2(-1190, 10000), DoorDirection.Left);
            Door doorF1 = new Door(new Vector2(0, 12118), DoorDirection.Bottom);
            Door doorF2 = new Door(new Vector2(-800, 13400), DoorDirection.Top);    //tilbage til catacombesD fra catacombesE
            Door doorG1 = new Door(new Vector2(600, 13400), DoorDirection.Top);     //catacombesE til traproom
            Door doorH1 = new Door(new Vector2(1190, 14000), DoorDirection.Right);  //catacombesE til catacombesF //////
            Door doorH2 = new Door(new Vector2(-1190, 16000), DoorDirection.Left);  //pathfinding
            //Door doorG3 = new Door(new Vector2(-800, 16600), DoorDirection.Bottom);  //videsre
            //Door doorG2 = new Door(new Vector2(600, 16000), DoorDirection.Top);  //traproom
            Door doorG3 = new Door(new Vector2(-800, 22600), DoorDirection.Bottom);  //videre
            Door doorG2 = new Door(new Vector2(600, 22600), DoorDirection.Bottom);  //traproom
            Door doorI1 = new Door(new Vector2(1190, 16000), DoorDirection.Right); //ud af pathfinding I1
            Door doorI2 = new Door(new Vector2(-1190, 18000), DoorDirection.Left);
            Door doorJ1 = new Door(new Vector2(1190, 18000), DoorDirection.Right);
            Door doorJ2 = new Door(new Vector2(-1190, 20000), DoorDirection.Left);

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
            catacombesE.AddDoor(doorH1);

            trapRoom.AddDoor(doorG2);
            trapRoom.AddDoor(doorG3);

            catacombesF.AddDoor(doorH2);
            catacombesF.AddDoor(doorI1);
            catacombesG.AddDoor(doorI2);
            catacombesG.AddDoor(doorJ1);
            catacombesH.AddDoor(doorJ2);

            // Link doors
            doorA1.LinkTo(doorA2);
            doorB1.LinkTo(doorB2);
            doorC1.LinkTo(doorC2);
            doorD1.LinkTo(doorD2);
            doorE1.LinkTo(doorE2);
            doorF1.LinkTo(doorF2);
            doorG1.LinkTo(doorG2); // husk at blokér at man kan gå tilbage!!
            doorG3.LinkTo(doorG1); // husk !!
            //doorH2.LinkTo(doorI1); Seems a mistake
            //doorI2.LinkTo(doorJ1); Seems a mistake
            //doorJ2.LinkTo(doorH2);Seems a mistake
            doorH1.LinkTo(doorH2);
            doorI1.LinkTo(doorI2);
            doorJ1.LinkTo(doorJ2);
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
            Rooms.Add(cutscene);

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


        private static List<(EnemyType, Vector2)> GetEnemies(RoomType room)
        {

            List<(EnemyType, Vector2)> enemies = new List<(EnemyType, Vector2)>();

            switch (room)
            {
                case RoomType.CatacombesA:
                    enemies.Add((EnemyType.WalkingGoose, new Vector2(900, 3540)));
                    enemies.Add((EnemyType.WalkingGoose, new Vector2(450, 4440)));
                    enemies.Add((EnemyType.WalkingGoose, new Vector2(3300, 3690)));
                    break;
                case RoomType.CatacombesC:
                    enemies.Add((EnemyType.AggroGoose, new Vector2(-210, 8375)));
                    enemies.Add((EnemyType.AggroGoose, new Vector2(0, 7630)));
                    enemies.Add((EnemyType.AggroGoose, new Vector2(850, 7575)));
                    enemies.Add((EnemyType.AggroGoose, new Vector2(850, 8420)));
                    break;
                case RoomType.CatacombesD:
                    enemies.Add((EnemyType.AggroGoose, new Vector2(-300, 9700)));
                    enemies.Add((EnemyType.AggroGoose, new Vector2(-260, 10550)));
                    enemies.Add((EnemyType.AggroGoose, new Vector2(350, 10400)));
                    enemies.Add((EnemyType.AggroGoose, new Vector2(330, 9580)));
                    enemies.Add((EnemyType.AggroGoose, new Vector2(430, 11000)));
                    enemies.Add((EnemyType.AggroGoose, new Vector2(-800, 11330)));
                    enemies.Add((EnemyType.AggroGoose, new Vector2(670, 11825)));
                    break;
                case RoomType.CatacombesH:
                    enemies.Add((EnemyType.Goosifer, new Vector2(0, 20200)));
                    break;
                default:
                    break;
            }

            return enemies;

        }

    }
}
