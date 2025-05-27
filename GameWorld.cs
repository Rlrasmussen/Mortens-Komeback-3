using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.IO;
using System;
using System.Diagnostics;
using Mortens_Komeback_3.Command;
using Mortens_Komeback_3.Collider;
using Mortens_Komeback_3.Factory;
using Mortens_Komeback_3.Puzzles;
using Mortens_Komeback_3.Environment;
using Mortens_Komeback_3.Menu;
using Mortens_Komeback_3.State;
using Microsoft.Data.Sqlite;
using Mortens_Komeback_3.Observer;

namespace Mortens_Komeback_3
{
    public class GameWorld : Game, ISubject
    {
        #region Fields
        private static GameWorld instance;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Random random = new Random();
        private List<GameObject> gameObjects = new List<GameObject>();
        private List<GameObject> newGameObjects = new List<GameObject>();
        public List<Room> Rooms = new List<Room>();
        private HashSet<(GameObject, GameObject)> collisions = new HashSet<(GameObject, GameObject)>();
        public Dictionary<Location, Vector2> Locations = new Dictionary<Location, Vector2>();
        public Dictionary<Enum, Texture2D[]> Sprites = new Dictionary<Enum, Texture2D[]>();
        public Dictionary<Sound, SoundEffect> Sounds = new Dictionary<Sound, SoundEffect>();
        public Dictionary<MusicTrack, Song> Music = new Dictionary<MusicTrack, Song>();
        public Dictionary<EnemyType, (int health, int damage, float speed)> EnemyStats = new Dictionary<EnemyType, (int health, int damage, float speed)>();
        public SpriteFont GameFont;
        private float deltaTime;
        private bool gamePaused = false;
        private bool gameRunning = true;
        public List<GameObject> gamePuzzles = new List<GameObject>();
        public List<GameObject> npcs = new List<GameObject>();

        private string dbBasePath = AppDomain.CurrentDomain.BaseDirectory;
        public SqliteConnection Connection;

        private float spawnCutsceneTime = 5f;
        private float lastSpawnCutscene = 0f;
        private List<IObserver> listeners = new List<IObserver>();
        private Status status;

        //Rotation
        private float rotationTop = 0;
        private float rotationRight = (float)(Math.PI / 2);
        private float rotationBottom = (float)(Math.PI);
        private float rotationLeft = (float)(-Math.PI / 2);

        private Button myButton;
        public List<Button> buttonList = new List<Button>();
        public MenuType CurrentMenu { get; set; }
        private bool musicOn = true;
        private bool soundOn = true;

        private Song backgroundMusic;

        #endregion

        #region Properties
        public MenuManager MenuManager { get; set; }
        public Vector2 ScreenSize { get; private set; }

        /// <summary>
        /// Singleton for GameWorld
        /// </summary>
        public static GameWorld Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameWorld();

                return instance;
            }
        }

        /// <summary>
        /// DeltaTime for use by other classes
        /// </summary>
        public float DeltaTime { get => deltaTime; }

        /// <summary>
        /// Random generator for general usage
        /// </summary>
        public Random Random { get => random; }

        /// <summary>
        /// Bool to close threads when game is closed
        /// </summary>
        public bool GameRunning { get => gameRunning; set => gameRunning = value; }

        /// <summary>
        /// Bool to trigger a pause-effect
        /// </summary>
        public bool GamePaused { get => gamePaused; set => gamePaused = value; }

        /// <summary>
        /// Get function to retrieve/locate objects in GameObjects-list
        /// Simon
        /// </summary>
        public List<GameObject> GameObjects { get => gameObjects; }


#if DEBUG
        /// <summary>
        /// Bool to change if collisionboxes are draw or not
        /// Simon
        /// </summary>
        public bool DrawCollision { get; set; } = false;
#endif


        public Environment.Room CurrentRoom { get; set; }

        #endregion

        #region Constructor
        /// <summary>
        /// Constuctor used by Singleton
        /// </summary>
        private GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        #endregion

        #region Method
        /// <summary>
        /// Handles loading of assets and basic functionality
        /// All
        /// </summary>
        protected override void Initialize()
        {

            string dbPath = Path.Combine(dbBasePath, "Database", "mk3db.db");
            Connection = new SqliteConnection($"Data Source={dbPath}");

            LoadSprites();
            LoadSoundEffects();
            LoadMusic();
            GameFont = Content.Load<SpriteFont>("mortalKombatFont");
            AddLocations();
            GetEnemyStats();

            SetScreenSize(new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height));
            InputHandler.Instance.AddButtonDownCommand(Keys.Escape, new ExitCommand());

#if DEBUG
            InputHandler.Instance.AddButtonDownCommand(Keys.Space, new DrawCommand());
            InputHandler.Instance.AddButtonDownCommand(Keys.M, new SaveCommand());
            InputHandler.Instance.AddButtonDownCommand(Keys.U, new ClearSaveCommand());
            InputHandler.Instance.AddButtonDownCommand(Keys.P, new PauseCommand());
#endif
            CurrentMenu = MenuType.Playing;

            MenuManager = new MenuManager();
            MenuManager.CreateMenus();


            base.Initialize();
        }

        /// <summary>
        /// Handles once-per start/restart logic
        /// All
        /// </summary>
        protected override void LoadContent()
        {
            SavePoint.LoadSave();

            gameObjects.Add(Player.Instance);

            status = new Status();


            //SafePoint.SaveGame(Location.Spawn);

            //gameObjects.Add(new WeaponMelee(WeaponType.Melee, Player.Instance.Position + new Vector2(-300, 0)));
            //gameObjects.Add(new WeaponRanged(WeaponType.Ranged, Player.Instance.Position + new Vector2(-300, -100)));

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //Test Item
            //gameObjects.Add(new Item(ItemType.GeesusBlood, Vector2.Zero));

            #region Decorations
            gameObjects.Add(new Decoration(DecorationType.Coffin, new Vector2(100, 600), rotationTop));
            //gameObjects.Add(new Decoration(DecorationType.Hole0, new Vector2(600, 3500), rotationTop)); //Used for testing - To be removed
            gameObjects.Add(new Decoration(DecorationType.Hole1, new Vector2(600, 9750), rotationTop));
            gameObjects.Add(new Decoration(DecorationType.Candle, new Vector2(50, 3600), rotationTop));
            gameObjects.Add(new Decoration(DecorationType.Cobweb, new Vector2(-1160, 16500), rotationTop));

            gameObjects.Add(new Decoration(DecorationType.Candle, new Vector2(-447, -430), rotationTop)); //Under the painting in PopeRoom
            gameObjects.Add(new Decoration(DecorationType.Candle, new Vector2(-132, -430), rotationTop)); //Under the painting in PopeRoom


            #endregion


            DoorManager.Initialize();

            foreach (var room in DoorManager.Rooms)
                gameObjects.Add(room);

            foreach (var door in DoorManager.Doors)
                gameObjects.Add(door);
            if (Player.Instance.Position == Locations[Location.Spawn])
            { CurrentRoom = DoorManager.Rooms[0]; } // Start i første rum
            else
            {
                CurrentRoom = DoorManager.Rooms.Find(x => Player.Instance.CollisionBox.Intersects(x.CollisionBox));
            }

            #region Puzzles
            OrderPuzzle orderPuzzle = new OrderPuzzle(PuzzleType.OrderPuzzle, new Vector2(DoorManager.doorList["doorB1"].Position.X - 500, DoorManager.doorList["doorB1"].Position.Y + 500), DoorManager.doorList["doorB1"], new Vector2(300, 2000), new Vector2(100, 2000), new Vector2(-100, 2000), 0);
            gameObjects.Add(orderPuzzle);
            gamePuzzles.Add(orderPuzzle);
            ShootPuzzle shootPuzzle2 = new ShootPuzzle(PuzzleType.ShootPuzzle, new Vector2(DoorManager.doorList["doorD1"].Position.X-500, DoorManager.doorList["doorD1"].Position.Y - 400), DoorManager.doorList["doorD1"], new Vector2(DoorManager.doorList["doorD1"].Position.X - 300, DoorManager.Rooms.Find(x => x.RoomType == RoomType.CatacombesB).Position.Y), (float)Math.PI * 0.5f, new Vector2(DoorManager.doorList["doorD1"].Position.X - 700, DoorManager.Rooms.Find(x => x.RoomType == RoomType.CatacombesB).Position.Y), (float)Math.PI * 0.5f, 1);
            gameObjects.Add(shootPuzzle2);
            gamePuzzles.Add(shootPuzzle2);
            PathfindingPuzzle pathfindingPuzzle = new PathfindingPuzzle(PuzzleType.PathfindingPuzzle,
                new Vector2(DoorManager.Rooms.Find(x => x.RoomType == RoomType.CatacombesF).CollisionBox.Right - 200, DoorManager.Rooms.Find(x => x.RoomType == RoomType.CatacombesF).CollisionBox.Bottom - 500), //Puzzlelever
                DoorManager.doorList["doorI1"], //Door
                2, //ID
                new Vector2(-150, DoorManager.Rooms.Find(x => x.RoomType == RoomType.CatacombesF).CollisionBox.Bottom - 150), //Path start
                new Vector2(450, DoorManager.Rooms.Find(x => x.RoomType == RoomType.CatacombesF).CollisionBox.Top + 150), //Path end
                new Vector2(-150, DoorManager.Rooms.Find(x => x.RoomType == RoomType.CatacombesF).Position.Y - 150), //Path Goal
                DoorManager.Rooms.Find(x => x.RoomType == RoomType.CatacombesF)); //Room
            gameObjects.Add(pathfindingPuzzle);
            gamePuzzles.Add(pathfindingPuzzle);



            #endregion

            #region NPC + Bible & Rosary
            NPC ghost = new NPC(NPCType.Ghost, new Vector2(0, -2000));
            NPC pope = new NPC(NPCType.Pope, new Vector2(-800, 0));
            NPC coffin = new NPC(NPCType.Coffin, new Vector2(600, 2300));
            NPC hole0 = new NPC(NPCType.Hole0, new Vector2(600, 3400));
            NPC monk = new NPC(NPCType.Monk, new Vector2(-800, 6000));
            NPC nun = new NPC(NPCType.Nun, new Vector2(-600, 16000));
            NPC canadaGoose1 = new NPC(NPCType.CanadaGoose, new Vector2(0, 14000));
            NPC canadaGoose2 = new NPC(NPCType.CanadaGoose, new Vector2(0, 18000));
            canadaGoose2.Canada = true;

            npcs.Add(ghost);
            npcs.Add(pope);
            npcs.Add(coffin);
            npcs.Add(monk);
            npcs.Add(nun);
            npcs.Add(canadaGoose1);
            npcs.Add(canadaGoose2);
            npcs.Add(hole0); //test

            foreach (GameObject npc in npcs)
            {
                gameObjects.Add(npc);
            }

            gameObjects.Add(new Item(ItemType.Rosary, new Vector2(0, 22000))); 
            if (Player.Instance.Inventory.Find(x => x is WeaponRanged) == null)
            {
                gameObjects.Add(new Item(ItemType.Bible, new Vector2(2650, 4000)));
            }
            #endregion


            GameWorld.Instance.SpawnObject(EnemyPool.Instance.GetObject(EnemyType.WalkingGoose, DoorManager.Rooms.Find(x => (RoomType)x.Type == RoomType.CatacombesA).Position));

            foreach (GameObject gameObject in gameObjects)
                gameObject.Load();

            #region buttons and menu

            #endregion




            //Music
            backgroundMusic = Music[MusicTrack.Background];
            MediaPlayer.Play(Music[MusicTrack.Background]);
            MediaPlayer.IsRepeating = true;

            gameObjects.Add(new CutScene(CutSceneRoom.CutsceneMovie, new Vector2(0, -2000)));
        }

        /// <summary>
        /// Handles update logic
        /// Simon
        /// </summary>
        /// <param name="gameTime">DeltaTime</param>
        protected override void Update(GameTime gameTime)
        {

            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (GameObject gameObject in gameObjects)
            {

                if (!(gameObject is Player) && (Math.Abs(gameObject.Position.Y - Player.Instance.Position.Y) > 1000))
                    continue;

                gameObject.Update(gameTime);
                DoCollisionCheck(gameObject);
            }

            #region Chances of background music in some rooms
            if (backgroundMusic != Music[MusicTrack.GoosiferFigth] && CurrentRoom == DoorManager.Rooms.Find(x => x.RoomType is RoomType.CatacombesH))
            {
                backgroundMusic = Music[MusicTrack.GoosiferFigth];
                MediaPlayer.Play(backgroundMusic);
            }
            else if (backgroundMusic != Music[MusicTrack.TrapRoom] && CurrentRoom == DoorManager.Rooms.Find(x => x.RoomType is RoomType.TrapRoom))
            {
                backgroundMusic = Music[MusicTrack.TrapRoom];
                MediaPlayer.Play(backgroundMusic);
            }
            else if (backgroundMusic != Music[MusicTrack.Death] && Player.Instance.IsAlive == false) //Player is dead
            {
                backgroundMusic = Music[MusicTrack.Death];
                MediaPlayer.Play(backgroundMusic);
            }
            MenuManager.Update(InputHandler.Instance.MousePosition, InputHandler.Instance.LeftClick);

            //else if (backgroundMusic != Music[MusicTrack.Win] && Player.Instance.IsAlive == false) //Player win
            //{
            //    backgroundMusic = Music[MusicTrack.Win];
            //    MediaPlayer.Play(backgroundMusic);
            //}
            #endregion

            //if (gamePaused)
            //{
            //    switch (CurrentMenu)
            //    {
            //        case MenuType.MainMenu:
            //            MenuManager.Update(InputHandler.Instance.MousePosition, InputHandler.Instance.LeftClick);
            //            GameWorld.Instance.MenuManager.OpenMenu(MenuType.MainMenu);
            //            break;
            //        case MenuType.GameOver:
            //            MenuManager.Update(InputHandler.Instance.MousePosition, InputHandler.Instance.LeftClick);

            //            break;
            //        case MenuType.Pause:
            //            MenuManager.Update(InputHandler.Instance.MousePosition, InputHandler.Instance.LeftClick);
            //            break;
            //        case MenuType.Inventory: //Fjern
            //            break;
            //        case MenuType.Win:
            //            MenuManager.Update(InputHandler.Instance.MousePosition, InputHandler.Instance.LeftClick);
            //            break;
            //        case MenuType.Cursor: //Fjern
            //            break;
            //        case MenuType.Playing:
            //            break;
            //        default:
            //            break;
            //    }
            //}

            //Sets the right current room, if the room consist of two rooms, and therefore are not set by going through doors. - Philip
            if ((CurrentRoom.LeftSideOfBigRoom && Player.Instance.Position.X > CurrentRoom.CollisionBox.Right)
                || (CurrentRoom.RightSideOfBigRoom && Player.Instance.Position.X < CurrentRoom.CollisionBox.Left)
                || (CurrentRoom.TopSideOfBigRoom && Player.Instance.Position.Y > CurrentRoom.CollisionBox.Bottom)
                || (CurrentRoom.ButtomSideOfBigRoom && Player.Instance.Position.Y < CurrentRoom.CollisionBox.Top)
                )
            {
                CurrentRoom = DoorManager.Rooms.Find(x => Player.Instance.CollisionBox.Intersects(x.CollisionBox));
            }

            status.Update(gameTime);

            CleanUp();

            base.Update(gameTime);

            

        }

        /// <summary>
        /// Handles drawing of sprites
        /// All
        /// </summary>
        /// <param name="gameTime">DeltaTime</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(transformMatrix: Camera.Instance.GetTransformation(), samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Draw(_spriteBatch);

#if DEBUG
                if (DrawCollision)
                {
                    DrawCollisionBox(gameObject.CollisionBox);
                    if (gameObject is IPPCollidable)
                        DrawIPPCollisionBoxes(gameObject as IPPCollidable);

                    foreach (Room room in DoorManager.Rooms)
                    {
                        if (room.Tiles.Count > 0)
                        {
                            foreach (var tileKeyValue in room.Tiles)
                            {
                                DrawCollisionBox(tileKeyValue.Value.CollisionBox);
                            }
                        }
                    }
                }
#endif

            }

            //myButton.Draw(_spriteBatch, GameFont);

            ////foreach (Button button in buttonList)
            ////{
            ////    button.Draw(_spriteBatch, GameFont);

            ////}

            //GameWorld.Instance.buttonList.Draw(_spriteBatch, GameFont);
            MenuManager.Draw(_spriteBatch, GameFont);

            InputHandler.Instance.Draw(_spriteBatch);

            status.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Sætter skærmstørrelsen til at være de angivne dimensioner i vektor'en
        /// Simon
        /// </summary>
        /// <param name="screenSize">Angiver skærmstørrelse i form af x- og y-akser</param>
        private void SetScreenSize(Vector2 screenSize)
        {
            ScreenSize = screenSize;

            _graphics.PreferredBackBufferWidth = (int)screenSize.X;
            _graphics.PreferredBackBufferHeight = (int)screenSize.Y;
            _graphics.ApplyChanges();

        }

        /// <summary>
        /// Bruges eksternt som "GameWorld.Instance.SpawnObject(obj)" til at tilføje nye aktive objekter, og udskriver til Debugkonsollen hvad der er blevet tilføjet ud fra enum'et der er anvendt i konstruktøren
        /// </summary>
        /// <param name="gameObject"></param>
        public void SpawnObject(GameObject gameObject)
        {
            newGameObjects.Add(gameObject);
            Debug.WriteLine(gameObject.ToString() + " added to spawnlist");

        }

        /// <summary>
        /// Fjerner først objekter fra "gameobjects" hvor "IsAlive" er "false", skriver hvor mange der er det ud til Debug-konsollen, og tilføjer derefter alle objekter i "newGameObjects" efter at have kørt deres "Load" metode, og skriver hvor mange der er tilføjet i Debug-konsollen
        /// Simon
        /// </summary>
        private void CleanUp()
        {
            foreach (GameObject go in gameObjects.FindAll(x => !x.IsAlive))
            {
                Debug.WriteLine("Removed" + go.Type);
            }
            int remove = gameObjects.RemoveAll(x => !x.IsAlive);
            if (remove > 0)
                Debug.WriteLine($"{remove} objects removed from gameObjects");

            if (newGameObjects.Count > 0)
            {

                foreach (GameObject gameObject in newGameObjects)
                    gameObject.Load();

                gameObjects.AddRange(newGameObjects);
                Debug.WriteLine($"{newGameObjects.Count} objects added to gameObjects");
                newGameObjects.Clear();

            }

            if (collisions.Count > 0)
                collisions.Clear();

        }

        /// <summary>
        /// Handles Loading of sprites
        /// All
        /// </summary>
        private void LoadSprites()
        {

            #region Rooms

            Sprites.Add(RoomType.PopeRoom, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Rooms\\poperoomlight_") });
            Sprites.Add(RoomType.Stairs, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Rooms\\room_single") });
            Sprites.Add(RoomType.CatacombesA, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Rooms\\baggrundDobbelt_left") });
            Sprites.Add(RoomType.CatacombesA1, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Rooms\\baggrundDobbelt_right") });
            Sprites.Add(RoomType.CatacombesB, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Rooms\\room_single") });
            Sprites.Add(RoomType.CatacombesC, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Rooms\\room_single") });
            Sprites.Add(RoomType.CatacombesD, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Rooms\\rumT") });
            Sprites.Add(RoomType.CatacombesD1, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Rooms\\rumB") });
            Sprites.Add(RoomType.CatacombesE, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Rooms\\room_single") });
            Sprites.Add(RoomType.CatacombesF, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Rooms\\room_dark") });
            Sprites.Add(RoomType.CatacombesG, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Rooms\\room_dark") });
            Sprites.Add(RoomType.CatacombesH, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Rooms\\room_dark") });
            Sprites.Add(RoomType.TrapRoom, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Rooms\\room_dark") });
            Sprites.Add(RoomType.Curscene, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Rooms\\room_dark") });


            #endregion
            #region Player

            Texture2D[] crusaderWalk = new Texture2D[5];
            for (int i = 0; i < crusaderWalk.Length; i++)
            {
                crusaderWalk[i] = Content.Load<Texture2D>($"Sprites\\Player\\MortenWalk{i}");
            }
            Sprites.Add(PlayerType.Morten, crusaderWalk);

            Texture2D[] crusaderAttack = new Texture2D[11];
            for (int i = 0; i < crusaderAttack.Length; i++)
            {
                crusaderAttack[i] = Content.Load<Texture2D>($"Sprites\\Player\\attack{i}");
            }
            Sprites.Add(PlayerType.MortenAngriber, crusaderAttack);

            Texture2D[] monkWalk = new Texture2D[5];
            for (int i = 0; i < monkWalk.Length; i++)
            {
                monkWalk[i] = Content.Load<Texture2D>($"Sprites\\Player\\MortenMonk{i}");
            }
            Sprites.Add(PlayerType.MortenMunk, monkWalk);

            Texture2D[] monkAttack = new Texture2D[1];
            for (int i = 0; i < monkAttack.Length; i++)
            {
                monkAttack[i] = Content.Load<Texture2D>($"Sprites\\Player\\monkSling{i}");
            }
            Sprites.Add(PlayerType.MortenSling, monkAttack);
            //Texture2D[] holyWalk = new Texture2D[4];
            //for (int i = 0; i < holyWalk.Length; i++)
            //{
            //    holyWalk[i] = Content.Load<Texture2D>($"Sprites\\Player\\helligMortenHvid{i}");
            //}
            //Sprites.Add(PlayerType.MortenSling, holyWalk);

            #endregion
            #region Enemy

            Texture2D[] walkingGoose = new Texture2D[8];
            for (int i = 0; i < walkingGoose.Length; i++)
            {
                walkingGoose[i] = Content.Load<Texture2D>($"Sprites\\Enemy\\gooseWalk{i}");
            }
            Sprites.Add(EnemyType.WalkingGoose, walkingGoose);

            Texture2D[] aggroGoose = new Texture2D[8];
            for (int i = 0; i < aggroGoose.Length; i++)
            {
                aggroGoose[i] = Content.Load<Texture2D>($"Sprites\\Enemy\\aggro{i}");
            }
            Sprites.Add(EnemyType.AggroGoose, aggroGoose);

            Texture2D[] goosifer = new Texture2D[3];
            for (int i = 0; i < goosifer.Length; i++)
            {
                goosifer[i] = Content.Load<Texture2D>($"Sprites\\Enemy\\goosifer{i}");
            }
            Sprites.Add(EnemyType.Goosifer, goosifer);

            #endregion
            #region Items

            Sprites.Add(AttackType.Egg, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Items\\egg1") });
            Sprites.Add(ItemType.Key, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Items\\key") });
            Sprites.Add(ItemType.WallTurkey, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Items\\wallTurkey") });
            Texture2D[] sling = new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Items\\sling") };
            Sprites.Add(WeaponType.Ranged, sling);
            Sprites.Add(ItemType.Sling, sling);
            Texture2D[] sword = new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Items\\sword") };
            Sprites.Add(MenuType.Cursor, sword);
            Sprites.Add(WeaponType.Melee, sword);
            Sprites.Add(ItemType.Bible, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Items\\bible") });
            Sprites.Add(ItemType.Rosary, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Items\\rosary") });
            Sprites.Add(ItemType.GeesusBlood, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Items\\potion") });

            #endregion
            #region Menu

            Sprites.Add(MenuType.Win, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Menu\\winScreen") });
            Sprites.Add(MenuType.GameOver, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Menu\\looseScreen") });
            Sprites.Add(MenuType.MainMenu, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Menu\\start") });
            Sprites.Add(MenuType.Pause, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Menu\\pause") });

            Sprites.Add(ButtonSpriteType.Button, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Menu\\button") });
            Sprites.Add(ButtonSpriteType.ButtonPressed, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Menu\\buttonPressed") });
            Sprites.Add(ButtonSpriteType.ButtonSquare, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Rooms\\Square") });
            Sprites.Add(ButtonSpriteType.ButtonSquareChecked, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Menu\\buttonPressed") });

            #endregion
            #region NPC

            Sprites.Add(NPCType.Monk, new Texture2D[2] { Content.Load<Texture2D>("Sprites\\NPC\\monkNPCbible"), Content.Load<Texture2D>("Sprites\\NPC\\monkNPC") });
            Sprites.Add(NPCType.Nun, new Texture2D[2] { Content.Load<Texture2D>("Sprites\\NPC\\nunNPCrosary2"), Content.Load<Texture2D>("Sprites\\NPC\\nunNPC") });
            Sprites.Add(NPCType.Pope, new Texture2D[2] { Content.Load<Texture2D>("Sprites\\NPC\\pope0"), Content.Load<Texture2D>("Sprites\\NPC\\pope1") });
            Sprites.Add(NPCType.Hole0, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\hole") });
            Sprites.Add(NPCType.Coffin, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\coffin") });
            Sprites.Add(NPCType.Ghost, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\sqaure200x200") });


            Texture2D[] canadaGoose = new Texture2D[6];
            for (int i = 0; i < canadaGoose.Length; i++)
            {
                canadaGoose[i] = Content.Load<Texture2D>($"Sprites\\NPC\\CG{i}");
            }
            Sprites.Add(NPCType.CanadaGoose, canadaGoose);


            #endregion
            #region Overlay

            Sprites.Add(OverlayObjects.Heart, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Overlay\\heartSprite") });
            Sprites.Add(OverlayObjects.Dialog, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Overlay\\talk") });
            Sprites.Add(OverlayObjects.InteractBubble, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Overlay\\interact") });
            Sprites.Add(OverlayObjects.DialogBox, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Overlay\\dialogueBox") });
            Sprites.Add(OverlayObjects.WeaponBox, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Overlay\\round-corner") });


            #endregion
            #region Environment

            Sprites.Add(DoorType.Open, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\doorOpen_shadow") });
            Sprites.Add(DoorType.Locked, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\doorLocked") });
            Sprites.Add(DoorType.Closed, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\doorClosed_shadow") });
            Sprites.Add(SurfaceType.AvSurface, new Texture2D[4] { Content.Load<Texture2D>("Sprites\\Environment\\avsurfaceILD1"),
                Content.Load<Texture2D>("Sprites\\Environment\\avsurfaceILD2"),
                Content.Load<Texture2D>("Sprites\\Environment\\avsurfaceILD3"),
                Content.Load<Texture2D>("Sprites\\Environment\\avsurfaceILD4") });

            Sprites.Add(SurfaceType.BigSpikes, new Texture2D[3] { Content.Load<Texture2D>("Sprites\\Environment\\bigSpikes0"),
                Content.Load<Texture2D>("Sprites\\Environment\\bigSpikes1"),
                Content.Load<Texture2D>("Sprites\\Environment\\bigSpikes2") });

            Sprites.Add(SurfaceType.Spikes, new Texture2D[3] { Content.Load<Texture2D>("Sprites\\Environment\\spike0"),
                Content.Load<Texture2D>("Sprites\\Environment\\spike1"),
                Content.Load<Texture2D>("Sprites\\Environment\\spike2") });

            Sprites.Add(DoorType.Stairs, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\stair1") });
            Sprites.Add(DoorType.StairsLocked, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\stair0") });
            Sprites.Add(DoorType.StairsUp, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\stairsup") });
            Sprites.Add(EnvironmentType.Chest, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\chestClosed") });
            Sprites.Add(EnvironmentType.ChestOpen, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\chestOpen") });
            Sprites.Add(EnvironmentType.Lever, new Texture2D[3] { Content.Load<Texture2D>("Sprites\\Environment\\Lever0"), Content.Load<Texture2D>("Sprites\\Environment\\Lever1"), Content.Load<Texture2D>("Sprites\\Environment\\Lever2") });

            Sprites.Add(EnvironmentType.Plaque, new Texture2D[9] { Content.Load<Texture2D>("Sprites\\Environment\\plaqueDove"), Content.Load<Texture2D>("Sprites\\Environment\\plaqueCross"), Content.Load<Texture2D>("Sprites\\Environment\\plaqueSun"),
            Content.Load<Texture2D>("Sprites\\Environment\\plaqueLeaves"), Content.Load<Texture2D>("Sprites\\Environment\\plaqueStar"), Content.Load<Texture2D>("Sprites\\Environment\\plaqueMoon"),
            Content.Load<Texture2D>("Sprites\\Environment\\plaqueAnchor"), Content.Load<Texture2D>("Sprites\\Environment\\plaqueWine"), Content.Load<Texture2D>("Sprites\\Environment\\plaqueCandle")});

            #endregion

            #region Puzzle
            Sprites.Add(PuzzleType.OrderPuzzle, new Texture2D[3] { Content.Load<Texture2D>("Sprites\\Environment\\Lever0"), Content.Load<Texture2D>("Sprites\\Environment\\Lever1"), Content.Load<Texture2D>("Sprites\\Environment\\Lever2") });
            Sprites.Add(PuzzleType.OrderPuzzlePlaque, new Texture2D[9] { Content.Load<Texture2D>("Sprites\\Environment\\plaqueDove"), Content.Load<Texture2D>("Sprites\\Environment\\plaqueCross"), Content.Load<Texture2D>("Sprites\\Environment\\plaqueSun"),
            Content.Load<Texture2D>("Sprites\\Environment\\plaqueLeaves"), Content.Load<Texture2D>("Sprites\\Environment\\plaqueStar"), Content.Load<Texture2D>("Sprites\\Environment\\plaqueMoon"),
            Content.Load<Texture2D>("Sprites\\Environment\\plaqueAnchor"), Content.Load<Texture2D>("Sprites\\Environment\\plaqueWine"), Content.Load<Texture2D>("Sprites\\Environment\\plaqueCandle")});
            Sprites.Add(PuzzleType.ShootPuzzle, new Texture2D[3] { Content.Load<Texture2D>("Sprites\\Environment\\Lever0"), Content.Load<Texture2D>("Sprites\\Environment\\Lever1"), Content.Load<Texture2D>("Sprites\\Environment\\Lever2") });
            Sprites.Add(PuzzleType.PuzzleObstacle, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\sqaure200x200") });
            Sprites.Add(PuzzleType.PathfindingPuzzle, new Texture2D[3] { Content.Load<Texture2D>("Sprites\\Environment\\Lever0"), Content.Load<Texture2D>("Sprites\\Environment\\Lever1"), Content.Load<Texture2D>("Sprites\\Environment\\Lever2") });



            #endregion
            #region Decorations


            Sprites.Add(DecorationType.Splash, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\splash") });
            Sprites.Add(DecorationType.Cobweb, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\cobweb") });
            Sprites.Add(DecorationType.Cross, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\cross") });
            Sprites.Add(DecorationType.Coffin, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\coffin") });
            Sprites.Add(DecorationType.Light, new Texture2D[3] { Content.Load<Texture2D>("Sprites\\Environment\\Light0"), Content.Load<Texture2D>("Sprites\\Environment\\Light1"), Content.Load<Texture2D>("Sprites\\Environment\\Light2") });
            //Sprites.Add(DecorationType.Hole0, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\hole") });
            Sprites.Add(DecorationType.Hole1, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\hole1") });
            Sprites.Add(DecorationType.Candle, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\candle") });


            #endregion
            #region VFX

            Texture2D[] swordSwoosh = new Texture2D[11];
            for (int i = 0; i < swordSwoosh.Length; i++)
            {
                swordSwoosh[i] = Content.Load<Texture2D>($"Sprites\\VFX\\swing{i}");
            }
            Sprites.Add(AttackType.Swing, swordSwoosh);

            Sprites.Add(SurfaceType.Fireball, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\VFX\\Fireball") });

            #endregion
            #region Debug
            Sprites.Add(DebugEnum.Pixel, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Debug\\pixel") });
            Sprites.Add(TileEnum.Tile, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\Light2") });
            #endregion
            #region Cutscene
            Texture2D[] cutscene = new Texture2D[56];
            for (int i = 0; i < cutscene.Length; i++)
            {
                cutscene[i] = Content.Load<Texture2D>($"Sprites\\Cutscenes\\StartCutscene{i + 1}");
            }
            Sprites.Add(CutSceneRoom.CutsceneMovie, cutscene);
            #endregion

        }

        /// <summary>
        /// Handles loading of soundeffects
        /// All
        /// </summary>
        private void LoadSoundEffects()
        {

            #region Enemy

            Sounds.Add(Sound.GooseSound, Content.Load<SoundEffect>("Sounds\\Enemy\\gooseSound_Short"));
            Sounds.Add(Sound.CanadaGoose, Content.Load<SoundEffect>("Sounds\\Enemy\\Canada's Most Famous Word ＂Sorry＂"));
            Sounds.Add(Sound.Goosifer, Content.Load<SoundEffect>("Sounds\\Enemy\\goosifer"));


            #endregion
            #region Environment

            Sounds.Add(Sound.EggSmash, Content.Load<SoundEffect>("Sounds\\Environment\\eggSmashSound"));
            Sounds.Add(Sound.Fire, Content.Load<SoundEffect>("Sounds\\Environment\\fire-sound"));
            Sounds.Add(Sound.Click, Content.Load<SoundEffect>("Sounds\\Environment\\click"));
            Sounds.Add(Sound.CatacombDoor, Content.Load<SoundEffect>("Sounds\\Environment\\Door"));


            #endregion
            #region Player

            Sounds.Add(Sound.PlayerDamage, Content.Load<SoundEffect>("Sounds\\Player\\morten_Av"));
            Sounds.Add(Sound.PlayerHeal, Content.Load<SoundEffect>("Sounds\\Player\\playerHeal"));
            Sounds.Add(Sound.PlayerShoot, Content.Load<SoundEffect>("Sounds\\Player\\slingshoot"));
            Sounds.Add(Sound.PlayerWalk1, Content.Load<SoundEffect>("Sounds\\Player\\walkSound"));
            Sounds.Add(Sound.PlayerWalk2, Content.Load<SoundEffect>("Sounds\\Player\\walkSound2"));
            Sounds.Add(Sound.PlayerSwordAttack, Content.Load<SoundEffect>("Sounds\\Player\\playerSwordAttack"));
            Sounds.Add(Sound.PlayerChange, Content.Load<SoundEffect>("Sounds\\Player\\metal-impact-247482"));

            #endregion

        }

        /// <summary>
        /// Handles loading of music
        /// All
        /// </summary>
        private void LoadMusic()
        {

            Music.Add(MusicTrack.Battle, Content.Load<Song>("Music\\battleMusic"));

            Music.Add(MusicTrack.Background, Content.Load<Song>("Music\\Virtutes Vocis"));

            Music.Add(MusicTrack.Death, Content.Load<Song>("Music\\Funeral March for Brass"));

            Music.Add(MusicTrack.Pope, Content.Load<Song>("Music\\bgMusic"));

            Music.Add(MusicTrack.TrapRoom, Content.Load<Song>("Music\\intense-gritty-hard-rock-270016"));

            Music.Add(MusicTrack.GoosiferFigth, Content.Load<Song>("Music\\Trap room"));

            Music.Add(MusicTrack.Menu, Content.Load<Song>("Music\\menu"));

            Music.Add(MusicTrack.Win, Content.Load<Song>("Music\\win"));

        }

        /// <summary>
        /// Adds locations to a dictionary
        /// Simon
        /// </summary>
        private void AddLocations()
        {
            Locations.Add(Location.Spawn, new Vector2(0, -2000));

            Locations.Add(Location.Test, new Vector2(500, 0));
            Locations.Add(Location.PuzzleOne, new Vector2(900, 2000));
            Locations.Add(Location.PuzzleTwo, new Vector2(900, 6000));
            Locations.Add(Location.PuzzleThree, new Vector2(900, 16000));

        }

        /// <summary>
        /// Sørger for at tjekke om det primære objekt har en kollision med øvrige objekter
        /// Simon
        /// </summary>
        /// <param name="gameObject">Primære objekt der skal tjekkes op mod</param>
        private void DoCollisionCheck(GameObject gameObject)
        {

            if (gameObject is ICollidable)
                foreach (GameObject other in gameObjects)
                {

                    if (gameObject == other || collisions.Contains((gameObject, other)) || collisions.Contains((other, gameObject)) || (gameObject.Type.GetType() == other.Type.GetType() && !((PuzzleType)gameObject.Type == PuzzleType.PuzzleObstacle)) || !(other is ICollidable) || !gameObject.IsAlive || !other.IsAlive)
                        continue;

                    if ((
                        gameObject.Type.GetType() == typeof(PlayerType) ||
                        gameObject.Type.GetType() == typeof(AttackType) ||
                        (PuzzleType)gameObject.Type == PuzzleType.PuzzleObstacle
                        ) && (
                        other.Type.GetType() == typeof(EnemyType) ||
                        other.Type.GetType() == typeof(NPCType) ||
                        other.Type.GetType() == typeof(ItemType) ||
                        other.Type.GetType() == typeof(PuzzleType) ||
                        other.Type.GetType() == typeof(WeaponType) ||
                        other.GetType() == typeof(AvSurface) ||
                        other.GetType() == typeof(GoosiferFire) ||
                        other.Type.GetType() == typeof(DoorType) //test remove

                        ))
                    {
                        if ((gameObject as ICollidable).CheckCollision(other as ICollidable))
                        {
                            bool handledCollision = false;
                            if ((gameObject is IPPCollidable && other is IPPCollidable))
                                if ((gameObject as IPPCollidable).PPCheckCollision(other as IPPCollidable))
                                    handledCollision = true;
                                else
                                    continue;
                            else if (gameObject is IPPCollidable && other is ICollidable)
                                if ((gameObject as IPPCollidable).DoHybridCheck((other as ICollidable).CollisionBox))
                                    handledCollision = true;
                                else
                                    continue;
                            else if (other is IPPCollidable && gameObject is ICollidable)
                                if ((other as IPPCollidable).DoHybridCheck((gameObject as ICollidable).CollisionBox))
                                    handledCollision = true;
                                else
                                    continue;
                            else
                                handledCollision = true;


                            if (handledCollision)
                            {
                                (gameObject as ICollidable).OnCollision(other as ICollidable);
                                if (!((PuzzleType)gameObject.Type == PuzzleType.PuzzleObstacle && (PuzzleType)other.Type == PuzzleType.PuzzleObstacle)) //Makes sure obstacles doesn't double collide.
                                    (other as ICollidable).OnCollision(gameObject as ICollidable);
                                collisions.Add((gameObject, other));
                            }

                        }
                    }

                }

        }


        //private void SpawnCutscene()
        //{
        //    int i = 0;

        //    lastSpawnCutscene += DeltaTime;

        //    if (lastSpawnCutscene > spawnCutsceneTime)
        //    {
        //        //SpawnObject(new );
        //    }
        //}

        /// <summary>
        /// Method for returning a HashSet of enemies near Player
        /// Simon
        /// </summary>
        /// <param name="range">Max distance for inclusion of Enemy</param>
        /// <returns>HashSet of enemies that fulfilled condition</returns>
        public HashSet<Enemy> EnemiesNearPlayer(float range)
        {

            HashSet<Enemy> nearbyEnemies = new HashSet<Enemy>();

            foreach (GameObject gameObject in gameObjects)
                if (gameObject is Enemy && Vector2.Distance(Player.Instance.Position, gameObject.Position) <= range)
                    nearbyEnemies.Add((Enemy)gameObject);

            return nearbyEnemies;
        }

#if DEBUG

        private void DrawCollisionBox(Rectangle gameObject)
        {

            Color color = Color.Red;
            Rectangle collisionBox = gameObject;
            Rectangle topLine = new Rectangle(collisionBox.X, collisionBox.Y, collisionBox.Width, 1);
            Rectangle bottomLine = new Rectangle(collisionBox.X, collisionBox.Y + collisionBox.Height, collisionBox.Width, 1);
            Rectangle rightLine = new Rectangle(collisionBox.X + collisionBox.Width, collisionBox.Y, 1, collisionBox.Height);
            Rectangle leftLine = new Rectangle(collisionBox.X, collisionBox.Y, 1, collisionBox.Height);
            _spriteBatch.Draw(Sprites[DebugEnum.Pixel][0], topLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
            _spriteBatch.Draw(Sprites[DebugEnum.Pixel][0], bottomLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
            _spriteBatch.Draw(Sprites[DebugEnum.Pixel][0], rightLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);
            _spriteBatch.Draw(Sprites[DebugEnum.Pixel][0], leftLine, null, color, 0, Vector2.Zero, SpriteEffects.None, 1f);

        }


        private void DrawIPPCollisionBoxes(IPPCollidable obj)
        {

            foreach (RectangleData item in obj.Rectangles)
                DrawCollisionBox(item.Rectangle);

        }

#endif

        /// <summary>
        /// Retrieves data for enemies from database
        /// </summary>
        /// <exception cref="Exception">Exception to be thrown upon database error</exception>
        private void GetEnemyStats()
        {

            try
            {

                using (Connection)
                {

                    Connection.Open();

                    string commandText = "SELECT * FROM EnemyTypes"; //Retrieves all data from all rows in the table EnemyTypes
                    SqliteCommand command = new SqliteCommand(commandText, Connection);
                    SqliteDataReader reader = command.ExecuteReader();

                    int id = reader.GetOrdinal("ID");
                    int damage = reader.GetOrdinal("Damage");
                    int health = reader.GetOrdinal("Max_HP");
                    int speed = reader.GetOrdinal("Speed");

                    while (reader.Read())
                        EnemyStats.Add((EnemyType)reader.GetInt32(id), (reader.GetInt32(health), reader.GetInt32(damage), reader.GetFloat(speed))); //Puts all the data into a Dictionary with EnemyType as its key and a named tuple with all the values retrieved

                }

            }
            catch
            {

                throw new Exception("Method GameWorld.GetEnemyStats didn't execute properly");

            }

        }



        #region Observer - Rikke
        public void Attach(IObserver observer)
        {
            listeners.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            listeners.Remove(observer);
        }

        public void Notify(StatusType statusType)
        {
            foreach (IObserver observer in listeners)
            {
                observer.OnNotify(statusType);
            }
        }

        public void ResetObsevers()
        {
            listeners.Clear();
        }

        #endregion

        #region menu metoder
        public void HandleButtonAction(ButtonAction action)
        {
            switch (action)
            {
                case ButtonAction.StartGame:
                    GameWorld.Instance.StartGame();
                    break;

                case ButtonAction.QuitGame:
                    GameWorld.Instance.ExitGame();
                    break;

                case ButtonAction.TryAgain:
                    GameWorld.Instance.ClearSaveAndRestart();
                    break;

                case ButtonAction.ResumeGame:
                    GameWorld.Instance.ResumeGame();
                    break;

                    //case ButtonAction.ToggleMusic:
                    //    AudioManager.Instance.ToggleMusic();
                    //    break;

                    //case ButtonAction.ToggleSound:
                    //    AudioManager.Instance.ToggleSound();
                    //    break;
            }
        }

        public void StartGame()
        {
            // Sætter spillet i gang – fx ved at skifte til spilverden
            CurrentMenu = MenuType.Playing;
            MediaPlayer.Play(Music[MusicTrack.Background]); // Eksempel på baggrundsmusik
        }

        public void ExitGame()
        {
            GameWorld.Instance.Exit(); // Lukker spillet
        }

        public void ClearSaveAndRestart()
        {
            // Sletter evt. gemt data (hvis relevant)
            new ClearSaveCommand(); // Hvis du har en save manager
            StartGame(); // Genstarter spillet
        }

        public void ResumeGame()
        {
            CurrentMenu = MenuType.Playing;
            //MediaPlayer.Resume(); // Fortsætter musikken, hvis den blev pauset
            MediaPlayer.Play(Music[MusicTrack.Background]);
            gamePaused = false;
        }
        #endregion

        #endregion
    }
}
