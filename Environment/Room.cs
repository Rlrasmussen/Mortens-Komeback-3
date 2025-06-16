using Microsoft.Xna.Framework;
using Mortens_Komeback_3.Factory;
using Mortens_Komeback_3.State;
using System.Collections.Generic;

namespace Mortens_Komeback_3.Environment
{
    public class Room : GameObject
    {
        #region Fields
        private Room currentRoom;
        //private RoomType roomType;
        private Dictionary<Vector2, Tile> tiles = new Dictionary<Vector2, Tile>();
        private bool leftSideOfBigRoom = false;
        private bool rightSideOfBigRoom = false;
        private bool topSideOfBigRoom = false;
        private bool buttomSideOfBigRoom = false;
        private bool enemiesSpawned = false;
        private List<(EnemyType Type, Vector2 Position)> spawnList;
        private static List<Enemy> EnemiesSpawned = new List<Enemy>();

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
            spawnList = GetEnemies();

        }

        #endregion

        #region Method


        public override void Update(GameTime gameTime)
        {
            UnlockRooms();
            base.Update(gameTime);

        }

        /// <summary>
        /// Resets data for room
        /// Irene, Simon
        /// </summary>
        public override void Load()
        {
            foreach (Door doors in Doors)
            {
                if (doors.DoorStatus == DoorType.Closed)
                {
                    doors.DoorStatus = DoorType.Locked;

                }
            }

            enemiesSpawned = false;
            EnemiesSpawned.Clear();

            base.Load();
        }

        /// <summary>
        /// "Connects" a door to the room
        /// Irene
        /// </summary>
        /// <param name="door"></param>
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
            if (LeftSideOfBigRoom || RightSideOfBigRoom)
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

        /// <summary>
        /// Predetermined lists of enemies dependent on which RoomType the room is
        /// Simon
        /// </summary>
        /// <returns>A list of enemies to be spawned in the room</returns>
        private List<(EnemyType, Vector2)> GetEnemies()
        {

            List<(EnemyType, Vector2)> enemies = new List<(EnemyType, Vector2)>();

            switch (RoomType)
            {
                case RoomType.CatacombesA:
                case RoomType.CatacombesA1:
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
                case RoomType.CatacombesD1:
                    enemies.Add((EnemyType.WalkingGoose, new Vector2(-300, 9700)));
                    enemies.Add((EnemyType.AggroGoose, new Vector2(-260, 10550)));
                    enemies.Add((EnemyType.WalkingGoose, new Vector2(350, 10400)));
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

            if (enemies.Count == 0)
                return null;

            return enemies;

        }

        /// <summary>
        /// Spawns enemies set to be spawned from data list and in specific cases gives a predefined list of "waypoints" to patrol
        /// Simon
        /// </summary>
        public void SpawnEnemies()
        {

            if (GameWorld.Instance.CurrentRoom == this && !enemiesSpawned && spawnList != null)
            {
                int i = 0;
                enemiesSpawned = true;
                foreach (var enemy in spawnList)
                {
                    Enemy spawnThis = (Enemy)EnemyPool.Instance.GetObject(enemy.Type, enemy.Position);
                    EnemiesSpawned.Add(spawnThis);
                    GameWorld.Instance.SpawnObject(spawnThis);
                    PatrolState waypoint;
                    switch (RoomType)
                    {
                        case RoomType.CatacombesA:
                        case RoomType.CatacombesA1:
                            switch (i)
                            {
                                case 0:
                                    List<Vector2> path0 = new List<Vector2>();
                                    path0.Add(new Vector2(2100, 3980));
                                    path0.Add(new Vector2(2550, 3540));
                                    path0.Add(new Vector2(2400, 4130));
                                    path0.Add(new Vector2(760, 3990));
                                    path0.Add(new Vector2(900, 3540));
                                    spawnThis.IgnoreState = true;
                                    waypoint = new PatrolState(path0);
                                    waypoint.Enter(spawnThis);
                                    break;
                                case 1:
                                    List<Vector2> path1 = new List<Vector2>();
                                    path1.Add(new Vector2(-150, 4440));
                                    path1.Add(new Vector2(300, 3840));
                                    path1.Add(new Vector2(1650, 4135));
                                    path1.Add(new Vector2(2400, 4500));
                                    path1.Add(new Vector2(450, 4440));
                                    spawnThis.IgnoreState = true;
                                    waypoint = new PatrolState(path1);
                                    waypoint.Enter(spawnThis);
                                    break;
                                case 2:
                                    List<Vector2> path2 = new List<Vector2>();
                                    path2.Add(new Vector2(3300, 4440));
                                    path2.Add(new Vector2(2250, 3540));
                                    path2.Add(new Vector2(1950, 4300));
                                    path2.Add(new Vector2(2400, 3840));
                                    path2.Add(new Vector2(3300, 3690));
                                    spawnThis.IgnoreState = true;
                                    waypoint = new PatrolState(path2);
                                    waypoint.Enter(spawnThis);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case RoomType.CatacombesC:
                            switch (i)
                            {
                                case 2:
                                    List<Vector2> path2 = new List<Vector2>();
                                    path2.Add(new Vector2(850, 8420));
                                    path2.Add(new Vector2(850, 7575));
                                    spawnThis.IgnoreState = true;
                                    waypoint = new PatrolState(path2);
                                    waypoint.Enter(spawnThis);
                                    break;
                                case 3:
                                    List<Vector2> path3 = new List<Vector2>();
                                    path3.Add(new Vector2(850, 7575));
                                    path3.Add(new Vector2(850, 8420));
                                    spawnThis.IgnoreState = true;
                                    waypoint = new PatrolState(path3);
                                    waypoint.Enter(spawnThis);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case RoomType.CatacombesD:
                        case RoomType.CatacombesD1:
                            switch (i)
                            {
                                case 4:
                                    List<Vector2> path4 = new List<Vector2>();
                                    path4.Add(new Vector2(-800, 11330));
                                    path4.Add(new Vector2(670, 11825));
                                    path4.Add(new Vector2(0, 11300));
                                    path4.Add(new Vector2(430, 11000));
                                    spawnThis.IgnoreState = true;
                                    waypoint = new PatrolState(path4);
                                    waypoint.Enter(spawnThis);
                                    break;
                                case 5:
                                    List<Vector2> path5 = new List<Vector2>();
                                    path5.Add(new Vector2(670, 11825));
                                    path5.Add(new Vector2(0, 11300));
                                    path5.Add(new Vector2(430, 11000));
                                    path5.Add(new Vector2(-800, 11330));
                                    spawnThis.IgnoreState = true;
                                    waypoint = new PatrolState(path5);
                                    waypoint.Enter(spawnThis);
                                    break;
                                case 6:
                                    List<Vector2> path6 = new List<Vector2>();
                                    path6.Add(new Vector2(0, 11300));
                                    path6.Add(new Vector2(430, 11000));
                                    path6.Add(new Vector2(-800, 11330));
                                    path6.Add(new Vector2(670, 11825));
                                    spawnThis.IgnoreState = true;
                                    waypoint = new PatrolState(path6);
                                    waypoint.Enter(spawnThis);
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case RoomType.CatacombesH:
                            spawnThis.IgnoreState = true;
                            BossFightState bossFight = new BossFightState();
                            bossFight.Enter(spawnThis);
                            break;
                        default:
                            break;
                    }
                    i++;
                }
            }
        }

        /// <summary>
        /// Removes spawned enemies (to conserve processingpower since some run seperate threads)
        /// Simon
        /// </summary>
        public void DespawnEnemies()
        {

            GameWorld.Instance.IgnoreSoundEffect = true;

            foreach (Enemy enemy in EnemiesSpawned)
            {
                enemy.IsAlive = false;
                EnemyPool.Instance.ReleaseObject(enemy);
            }

            GameWorld.Instance.IgnoreSoundEffect = false;

            EnemiesSpawned.Clear();
            enemiesSpawned = false;

        }

        /// <summary>
        /// Unlocks the doors in room if all enemies are killed
        /// Irene
        /// </summary>
        public void UnlockRooms()
        {
            if (EnemiesSpawned.Count > 0)
            {
                foreach (Enemy enemy in EnemiesSpawned)
                {
                    if (enemy.IsAlive)
                        return;
                }
            }

            foreach (Door doors in Doors)
            {
                if (doors.DoorStatus == DoorType.Locked && !(doors == DoorManager.doorList["doorI1"]) && !(doors == DoorManager.doorList["doorJ2"]))
                {
                   doors.UnlockDoor();
                }
            }

        }

        #endregion
    }
}
