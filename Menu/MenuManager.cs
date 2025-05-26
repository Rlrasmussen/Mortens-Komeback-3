using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Mortens_Komeback_3.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D9;
using Microsoft.Xna.Framework.Media;

namespace Mortens_Komeback_3.Menu
{
    public class MenuManager
    {

        private string text;

        public bool Hovering { get; set; }


        private Menu currentMenu;

        private Dictionary<MenuType, Menu> menus = new Dictionary<MenuType, Menu>();

        public MenuManager()
        {
            //switch (text)
            //{
            //    case "Start":

            //        break;
            //    case "Quit":
            //        break;
            //    case "Resume":
            //        //CloseMenu();
            //        break;
            //    case "Try again":
            //        break;
            //    case "Music On/Off":
            //        break;
            //    case "Sound On/Off":
            //        break;
            //}

        }

        public void CreateMenus()
        {


            var mainMenu = new Menu(MenuType.MainMenu);
            mainMenu.Sprite = GameWorld.Instance.Sprites[MenuType.MainMenu][0];
            mainMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(500, 300), "Start", new StartGameCommand()));
            mainMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(500, 400), "Quit", new ExitCommand()));

            var gameOverMenu = new Menu(MenuType.GameOver);
            gameOverMenu.Sprite = GameWorld.Instance.Sprites[MenuType.GameOver][0];
            gameOverMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(500, 400), "Try Again!!", new ClearSaveCommand()));
            gameOverMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(500, 500), "Quit!!", new ExitCommand()));
            gameOverMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(Camera.Instance.Position.X, Camera.Instance.Position.Y + 100f), "Resume!", new ResumeCommand()));


            var winMenu = new Menu(MenuType.Win);
            winMenu.Sprite = GameWorld.Instance.Sprites[MenuType.Win][0];
            gameOverMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(500, 400), "Try Again", new ClearSaveCommand()));
            gameOverMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(500, 500), "Quit", new ExitCommand()));

            var pauseMenu = new Menu(MenuType.Pause);
            pauseMenu.Sprite = GameWorld.Instance.Sprites[MenuType.Pause][0];
            pauseMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(Camera.Instance.Position.X, Camera.Instance.Position.Y + 100f), "Resume", new ResumeCommand()));
            pauseMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(Camera.Instance.Position.X, Camera.Instance.Position.Y + 200f), "Quit", new ExitCommand()));
            pauseMenu.AddButtons(new Button(ButtonSpriteType.ButtonSquare, new Vector2(Camera.Instance.Position.X, Camera.Instance.Position.Y + 220f), "Music", new ExitCommand())); // music toggle square
            pauseMenu.AddButtons(new Button(ButtonSpriteType.ButtonSquare, new Vector2(Camera.Instance.Position.X, Camera.Instance.Position.Y + 230f), "Sound", new ExitCommand())); // sound toggle square

            menus.Add(MenuType.MainMenu, mainMenu);
            menus.Add(MenuType.GameOver, gameOverMenu);
            menus.Add(MenuType.Pause, pauseMenu);
            menus.Add(MenuType.Win, winMenu);
        }






        public void Update(Vector2 mousePos, bool isClicking)
        {
            currentMenu?.Update(InputHandler.Instance.MousePosition, isClicking);

            //if (InputHandler.Instance.LeftClick)
            //{
            //    GameWorld.Instance.MenuManager.CloseMenu();
            //}


        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {

            currentMenu?.Draw(spriteBatch, font);
        }

        public void OpenMenu(MenuType type)
        {
            if (menus.TryGetValue(type, out Menu menu))
            {
                currentMenu = menu;
            }
        }

        public void CloseMenu()
        {
            currentMenu = null;
        }

        /// <summary>
        /// Irene
        /// </summary>
        public void Pause()
        {
            if (!GameWorld.Instance.GamePaused)
            {
                //gamePaused = false;
                GameWorld.Instance.GamePaused = true;
                OpenMenu(MenuType.Pause);
            }


        }

        public void ResumeGame()
        {
            if (GameWorld.Instance.GamePaused)
            {
                GameWorld.Instance.GamePaused = false;
                CloseMenu();
            }
            else
            {
                GameWorld.Instance.GamePaused = true;
                //MediaPlayer.Play(GameWorld.Instance.Music[MusicTrack.Menu]);
            }

        }

        public void ShowMainMenu()
        {
       

        }

        public void GameOverMenu()
        {
            if (Player.Instance.IsAlive == false && !GameWorld.Instance.GamePaused)
            {
                GameWorld.Instance.GamePaused = true;
                OpenMenu(MenuType.GameOver);
            }

        }

        public void SoundToggle()
        {
            if (GameWorld.Instance.GamePaused && GameWorld.Instance.CurrentMenu == MenuType.Pause)
            {

                MediaPlayer.Play(GameWorld.Instance.Music[MusicTrack.Background]);
            }
            else
            {
                GameWorld.Instance.GamePaused = true;
                MediaPlayer.Play(GameWorld.Instance.Music[MusicTrack.Menu]);
            }

        }

        public void ShowWinMenu()
        {
            if (GameWorld.Instance.GamePaused)
            {
                //gamePaused = false;

                MediaPlayer.Play(GameWorld.Instance.Music[MusicTrack.Background]);
            }
            else
            {
                GameWorld.Instance.GamePaused = true;
                MediaPlayer.Play(GameWorld.Instance.Music[MusicTrack.Menu]);
            }

        }

        public void StartGame()
        {
            if (GameWorld.Instance.GamePaused)
            {
                //gamePaused = false;

                MediaPlayer.Play(GameWorld.Instance.Music[MusicTrack.Background]);
            }
            else
            {
                GameWorld.Instance.GamePaused = true;
                MediaPlayer.Play(GameWorld.Instance.Music[MusicTrack.Menu]);
            }

        }

        public void ExitGame()
        {
            if (GameWorld.Instance.GamePaused)
            {
                //gamePaused = false;

                MediaPlayer.Play(GameWorld.Instance.Music[MusicTrack.Background]);
            }
            else
            {
                GameWorld.Instance.GamePaused = true;
                MediaPlayer.Play(GameWorld.Instance.Music[MusicTrack.Menu]);
            }

        }
    }
}
