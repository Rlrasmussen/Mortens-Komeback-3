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
        private HashSet<(GameObject, GameObject)> collisions = new HashSet<(GameObject, GameObject)>();
        public Dictionary<Location, Vector2> Locations = new Dictionary<Location, Vector2>();
        public Dictionary<Enum, Texture2D[]> Sprites = new Dictionary<Enum, Texture2D[]>();
        public Dictionary<Sound, SoundEffect> Sounds = new Dictionary<Sound, SoundEffect>();
        public Dictionary<MusicTrack, Song> Music = new Dictionary<MusicTrack, Song>();
        public SpriteFont GameFont;
        private float deltaTime;
        private bool gamePaused = false;
        private bool gameRunning = true;


        public static GameWorld Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameWorld();

                return instance;
            }
        }


        public float DeltaTime { get => deltaTime; }


        public Random Random { get => random; }


        public bool GameRunning { get => gameRunning; }


        public bool GamePaused { get => gamePaused; set => gamePaused = value; }


        private GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            LoadSprites();
            LoadSoundEffects();
            LoadMusic();
            GameFont = Content.Load<SpriteFont>("mortalKombatFont");
            AddLocations();

            SetScreenSize(new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height));

            gameObjects.Add(Player.Instance);
            gameObjects.Add(EnemyPool.Instance.CreateSpecificGoose(EnemyType.AggroGoose, Vector2.Zero));

            base.Initialize();
        }

        protected override void LoadContent()
        {


            _spriteBatch = new SpriteBatch(GraphicsDevice);

            foreach (GameObject gameObject in gameObjects)
                gameObject.Load();

        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                gameRunning = false;
                Exit();
            }

            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(gameTime);
                DoCollisionCheck(gameObject);
            }

            CleanUp();

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);

            _spriteBatch.Begin(transformMatrix: Camera.Instance.GetTransformation(), samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.FrontToBack);

            foreach (GameObject gameObject in gameObjects)
                gameObject.Draw(_spriteBatch);

            InputHandler.Instance.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Sætter skærmstørrelsen til at være de angivne dimensioner i vektor'en
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

        private void LoadSprites()
        {

            #region Rooms

            Sprites.Add(Roomtype.Single, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Rooms\\room_single") });

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
            Sprites.Add(ItemType.Sling, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Items\\sling") });
            Sprites.Add(ItemType.WallTurkey, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Items\\wallTurkey") });

            Texture2D[] sword = new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Items\\sword") };
            Sprites.Add(MenuType.Cursor, sword);
            Sprites.Add(ItemType.Sword, sword);

            #endregion
            #region Menu

            Sprites.Add(MenuType.Win, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Menu\\winScreen") });
            Sprites.Add(MenuType.GameOver, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Menu\\looseScreen") });

            #endregion
            #region NPC

            Sprites.Add(NPCType.Monk, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\NPC\\monkNPCbible") });
            Sprites.Add(NPCType.Nun, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\NPC\\nunNPCrosary") });

            #endregion
            #region Overlay

            Sprites.Add(OverlayObjects.Heart, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Overlay\\heartSprite") });
            Sprites.Add(OverlayObjects.Dialog, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Overlay\\talk") });

            #endregion
            #region Environment

            Sprites.Add(DoorType.Open, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\doorOpen_shadow") });
            Sprites.Add(DoorType.Locked, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\doorLocked") });
            Sprites.Add(DoorType.Closed, new Texture2D[1] { Content.Load<Texture2D>("Sprites\\Environment\\doorClosed_shadow") });

            #endregion

        }


        private void LoadSoundEffects()
        {

            #region Enemy

            Sounds.Add(Sound.GooseSound, Content.Load<SoundEffect>("Sounds\\Enemy\\gooseSound_Short"));

            #endregion
            #region Environment

            Sounds.Add(Sound.EggSmash, Content.Load<SoundEffect>("Sounds\\Environment\\eggSmashSound"));

            #endregion
            #region Player

            Sounds.Add(Sound.PlayerDamage, Content.Load<SoundEffect>("Sounds\\Player\\morten_Av"));
            Sounds.Add(Sound.PlayerHeal, Content.Load<SoundEffect>("Sounds\\Player\\playerHeal"));
            Sounds.Add(Sound.PlayerShoot, Content.Load<SoundEffect>("Sounds\\Player\\shootSound"));
            Sounds.Add(Sound.PlayerWalk1, Content.Load<SoundEffect>("Sounds\\Player\\walkSound"));
            Sounds.Add(Sound.PlayerWalk2, Content.Load<SoundEffect>("Sounds\\Player\\walkSound2"));
            Sounds.Add(Sound.PlayerSwordAttack, Content.Load<SoundEffect>("Sounds\\Player\\playerSwordAttack"));

            #endregion

        }


        private void LoadMusic()
        {

            Music.Add(MusicTrack.Battle, Content.Load<Song>("Music\\battleMusic"));

            Music.Add(MusicTrack.Background, Content.Load<Song>("Music\\bgMusic"));

        }


        private void AddLocations()
        {

            Locations.Add(Location.Spawn, new Vector2(-250, 250));

        }

        /// <summary>
        /// Sørger for at tjekke om det primære objekt har en kollision med øvrige objekter
        /// </summary>
        /// <param name="gameObject">Primære objekt der skal tjekkes op mod</param>
        private void DoCollisionCheck(GameObject gameObject)
        {

            if (gameObject is ICollidable)
                foreach (GameObject other in gameObjects)
                {

                    if (gameObject == other || collisions.Contains((gameObject, other)) || collisions.Contains((other, gameObject)) || gameObject.Type.GetType() == other.Type.GetType() || !(other is ICollidable))
                        continue;

                    if ((
                        gameObject.Type.GetType() == typeof(PlayerType) ||
                        gameObject.Type.GetType() == typeof(AttackType)
                        ) && (
                        other.Type.GetType() == typeof(EnemyType) ||
                        other.Type.GetType() == typeof(PuzzleType)
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

    }
}
