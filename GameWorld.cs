using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using Mortens_Komeback_3.Command;
using Mortens_Komeback_3.Collider;
using Mortens_Komeback_3.Factory;
using Mortens_Komeback_3.Puzzles;
using Mortens_Komeback_3.Environment;

namespace Mortens_Komeback_3
{
    public class GameWorld : Game
    {

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
        public SpriteFont GameFont;
        private float deltaTime;
        private bool gamePaused = false;
        private bool gameRunning = true;
        public List<GameObject> gamePuzzles = new List<GameObject>();

        private float spawnEnemyTime = 5f;
        private float lastSpawnEnemy = 0f;

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


#if DEBUG
        /// <summary>
        /// Bool to change if collisionboxes are draw or not
        /// </summary>
        public bool DrawCollision { get; set; } = false;
#endif


        public Environment.Room CurrentRoom { get; set; }

        /// <summary>
        /// Constuctor used by Singleton
        /// </summary>
        private GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Handles loading of assets and basic functionality
        /// All
        /// </summary>
        protected override void Initialize()
        {

            LoadSprites();
            LoadSoundEffects();
            LoadMusic();
            GameFont = Content.Load<SpriteFont>("mortalKombatFont");
            AddLocations();

            SetScreenSize(new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height));
            InputHandler.Instance.AddButtonDownCommand(Keys.Escape, new ExitCommand());
            InputHandler.Instance.AddButtonDownCommand(Keys.Space, new DrawCommand());

            gameObjects.Add(Player.Instance);
            gameObjects.Add(EnemyPool.Instance.CreateSpecificGoose(EnemyType.AggroGoose, Vector2.Zero));


            //gameObjects.Add(EnemyPool.Instance.CreateSpecificGoose(EnemyType.AggroGoose, new Vector2(-200, -200)));


            base.Initialize();
        }

        /// <summary>
        /// Handles once-per start/restart logic
        /// All
        /// </summary>
        protected override void LoadContent()
        {

            gameObjects.Add(new WeaponMelee(WeaponType.Melee, Player.Instance.Position + new Vector2(-300, 0)));
            gameObjects.Add(new WeaponRanged(WeaponType.Ranged, Player.Instance.Position + new Vector2(-300, -100)));



            _spriteBatch = new SpriteBatch(GraphicsDevice);

          
            gameObjects.Add(new NPC(NPCType.Pope, new Vector2(200,200))); //Used for testing - To be removed
            DoorManager.Initialize();
            GameWorld.Instance.CurrentRoom = DoorManager.Rooms[0];

            foreach (var room in DoorManager.Rooms)
                gameObjects.Add(room);

            foreach (var door in DoorManager.Doors)
                gameObjects.Add(door);
            CurrentRoom = DoorManager.Rooms[0]; // Start i første rum

            foreach (GameObject gameObject in gameObjects)
                gameObject.Load();


            #region Puzzles
            OrderPuzzle orderPuzzle = new OrderPuzzle(PuzzleType.OrderPuzzle, new Vector2(1190, 400), DoorManager.Doors.Find(x => x.Position == new Vector2(1190, 2000)), new Vector2(300, 500), new Vector2(100, 500), new Vector2(-100, 500));
            gameObjects.Add(orderPuzzle);
            gamePuzzles.Add(orderPuzzle);
            ShootPuzzle shootPuzzle2 = new ShootPuzzle(PuzzleType.ShootPuzzle, new Vector2(1190, 5600), DoorManager.Doors.Find(x => x.Position == new Vector2(1190, 6000)),new Vector2(0, 5700), 0, new Vector2(0, 6300), 0);
            gameObjects.Add(shootPuzzle2);
            gamePuzzles.Add(shootPuzzle2);
            #endregion

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
                gameObject.Update(gameTime);
                DoCollisionCheck(gameObject);
            }

            //SpawnEnemies();

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
                }
#endif

            }
            
            foreach(var GO in DoorManager.Rooms.Find(x => (RoomType)x.Type == RoomType.PopeRoom).Tiles)
            {
                DrawCollisionBox(GO.Value.CollisionBox);
            }

            InputHandler.Instance.Draw(_spriteBatch);

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

            Sprites.Add(RoomType.PopeRoom, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Rooms\\room_single") });
            Sprites.Add(RoomType.Stairs, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Rooms\\room_single") });
            Sprites.Add(RoomType.CatacombesA, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Rooms\\room_single") });
            Sprites.Add(RoomType.CatacombesB, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Rooms\\room_single") });
            Sprites.Add(RoomType.CatacombesC, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Rooms\\room_single") });
            Sprites.Add(RoomType.CatacombesD, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Rooms\\room_single") });
            Sprites.Add(RoomType.CatacombesE, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Rooms\\room_single") });
            Sprites.Add(RoomType.CatacombesF, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Rooms\\room_single") });
            Sprites.Add(RoomType.CatacombesG, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Rooms\\room_single") });
            Sprites.Add(RoomType.CatacombesH, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Rooms\\room_single") });
            Sprites.Add(RoomType.TrapRoom, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Rooms\\room_single") });


            #endregion
            #region Player

            Texture2D[] crusaderWalk = new Texture2D[5];
            for (int i = 0; i < crusaderWalk.Length; i++)
            {
                crusaderWalk[i] = Content.Load<Texture2D>($"Sprites\\Player\\mortenCrusader{i}");
            }
            Sprites.Add(PlayerType.Morten, crusaderWalk);

            Texture2D[] crusaderAttack = new Texture2D[11];
            for (int i = 0; i < crusaderAttack.Length; i++)
            {
                crusaderAttack[i] = Content.Load<Texture2D>($"Sprites\\Player\\attack{i}");
            }
            Sprites.Add(PlayerType.MortenAngriber, crusaderAttack);

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

            #endregion
            #region Menu

            Sprites.Add(MenuType.Win, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Menu\\winScreen") });
            Sprites.Add(MenuType.GameOver, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Menu\\looseScreen") });

            #endregion
            #region NPC

            Sprites.Add(NPCType.Monk, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\NPC\\monkNPCbible") });
            Sprites.Add(NPCType.Nun, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\NPC\\nunNPCrosary") });
            Sprites.Add(NPCType.Pope, new Texture2D[2] { Content.Load<Texture2D>("Sprites\\NPC\\pope0"), Content.Load<Texture2D>("Sprites\\NPC\\pope1") });

            #endregion
            #region Overlay

            Sprites.Add(OverlayObjects.Heart, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Overlay\\heartSprite") });
            Sprites.Add(OverlayObjects.Dialog, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Overlay\\talk") });
            Sprites.Add(OverlayObjects.InteractBubble, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Overlay\\interact") });

            #endregion
            #region Environment

            Sprites.Add(DoorType.Open, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\doorOpen_shadow") });
            Sprites.Add(DoorType.Locked, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\doorLocked") });
            Sprites.Add(DoorType.Closed, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\doorClosed_shadow") });
            Sprites.Add(SurfaceType.AvSurface, new Texture2D[4] { Content.Load<Texture2D>("Sprites\\Environment\\avsurfaceILD1"),
                Content.Load<Texture2D>("Sprites\\Environment\\avsurfaceILD2"),
                Content.Load<Texture2D>("Sprites\\Environment\\avsurfaceILD3"),
                Content.Load<Texture2D>("Sprites\\Environment\\avsurfaceILD4") });
            Sprites.Add(DoorType.Stairs, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\stair1") });
            Sprites.Add(DoorType.StairsLocked, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\stair0") });
            #endregion
            #region Puzzle
            Sprites.Add(PuzzleType.OrderPuzzle, new Texture2D[2] { Content.Load<Texture2D>("Sprites\\Environment\\doorLocked"), Content.Load<Texture2D>("Sprites\\Environment\\doorOpen_Shadow") });
            Sprites.Add(PuzzleType.OrderPuzzlePlaque, new Texture2D[3] { Content.Load<Texture2D>("Sprites\\Items\\wallTurkey"), Content.Load<Texture2D>("Sprites\\Items\\sling"), Content.Load<Texture2D>("Sprites\\Items\\key") });
            Sprites.Add(PuzzleType.ShootPuzzle, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Overlay\\heartSprite") });

            #endregion
            #region Decorations


            #endregion
            #region VFX

            Texture2D[] swordSwoosh = new Texture2D[11];
            for (int i = 0; i < swordSwoosh.Length; i++)
            {
                swordSwoosh[i] = Content.Load<Texture2D>($"Sprites\\VFX\\swing{i}");
            }
            Sprites.Add(AttackType.Swing, swordSwoosh);

            #endregion
            #region Debug
            Sprites.Add(DebugEnum.Pixel, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Debug\\pixel") });
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
            Sounds.Add(Sound.CatacombDoor, Content.Load<SoundEffect>("Sounds\\Environment\\Catacomb door"));


            #endregion
            #region Player

            Sounds.Add(Sound.PlayerDamage, Content.Load<SoundEffect>("Sounds\\Player\\morten_Av"));
            Sounds.Add(Sound.PlayerHeal, Content.Load<SoundEffect>("Sounds\\Player\\playerHeal"));
            Sounds.Add(Sound.PlayerShoot, Content.Load<SoundEffect>("Sounds\\Player\\086123_slingshot-81843"));
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

            Music.Add(MusicTrack.Background, Content.Load<Song>("Music\\bgMusic"));

            Music.Add(MusicTrack.Death, Content.Load<Song>("Music\\Funeral March for Brass"));

            Music.Add(MusicTrack.Pope, Content.Load<Song>("Music\\Virtutes Vocis"));

            Music.Add(MusicTrack.GoosiferFigth, Content.Load<Song>("Music\\intense-gritty-hard-rock-270016"));

            Music.Add(MusicTrack.TrapRoom, Content.Load<Song>("Music\\Trap room"));

            Music.Add(MusicTrack.Menu, Content.Load<Song>("Music\\menu"));

            Music.Add(MusicTrack.Win, Content.Load<Song>("Music\\win"));

        }

        /// <summary>
        /// Adds locations to a dictionary
        /// Simon
        /// </summary>
        private void AddLocations()
        {

            Locations.Add(Location.Spawn, new Vector2(-250, 250));

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

                    if (gameObject == other || collisions.Contains((gameObject, other)) || collisions.Contains((other, gameObject)) || gameObject.Type.GetType() == other.Type.GetType() || !(other is ICollidable) || !gameObject.IsAlive || !other.IsAlive)
                        continue;

                    if ((
                        gameObject.Type.GetType() == typeof(PlayerType) ||
                        gameObject.Type.GetType() == typeof(AttackType)
                        ) && (
                        other.Type.GetType() == typeof(EnemyType) ||
                        other.Type.GetType() == typeof(PuzzleType) ||
                        other.Type.GetType() == typeof(WeaponType) ||
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
                                (other as ICollidable).OnCollision(gameObject as ICollidable);
                                collisions.Add((gameObject, other));
                            }

                        }
                    }

                }

        }


        private void SpawnEnemies()
        {
            lastSpawnEnemy += DeltaTime;

            if (lastSpawnEnemy > spawnEnemyTime)
            {
                SpawnObject(EnemyPool.Instance.GetObject());
                lastSpawnEnemy = 0f;
            }
        }

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

    }
}
