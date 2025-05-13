using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using Mortens_Komeback_3.Command;

namespace Mortens_Komeback_3
{
    public class GameWorld : Game
    {

        private static GameWorld instance;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Random random;
        private List<GameObject> gameObjects = new List<GameObject>();
        private List<GameObject> newGameObjects = new List<GameObject>();
        public Dictionary<Enum, Texture2D[]> Sprites = new Dictionary<Enum, Texture2D[]>();
        public Dictionary<Sounds, SoundEffect> Sounds = new Dictionary<Sounds, SoundEffect>();
        public Dictionary<MusicTrack, Song> Music = new Dictionary<MusicTrack, Song>();
        public SpriteFont GameFont;
        private float deltaTime;
        private bool gamePaused = false;


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


        public GameWorld()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }


        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(gameTime);
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

        }
    }
}
