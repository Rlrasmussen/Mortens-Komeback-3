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

      

        public void CreateMenus()
        {
            var mainMenu = new Menu(MenuType.MainMenu);
            mainMenu.Sprite = GameWorld.Instance.Sprites[MenuType.MainMenu][0];
            mainMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(500, 300), "Start", ButtonAction.StartGame));
            mainMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(500, 400), "Quit", ButtonAction.QuitGame));

            var gameOverMenu = new Menu(MenuType.GameOver);
            gameOverMenu.Sprite = GameWorld.Instance.Sprites[MenuType.GameOver][0];
            //gameOverMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(Camera.Instance.Position.X, Camera.Instance.Position.Y + 100f), "Resume", ButtonAction.ResumeGame));
            gameOverMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(500, 200), "Reload", ButtonAction.Reload));
            gameOverMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(500, 400), "Try Again", ButtonAction.TryAgain));
            gameOverMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(500, 500), "Quit", ButtonAction.QuitGame));
            
            // Not sure if the resume command needs to be added here
            //gameOverMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(Camera.Instance.Position.X, Camera.Instance.Position.Y + 100f), "Resume", ButtonAction.ResumeGame));

            var winMenu = new Menu(MenuType.Win);
            winMenu.Sprite = GameWorld.Instance.Sprites[MenuType.Win][0];
            winMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(500, 400), "Try Again", ButtonAction.TryAgain));
            winMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(500, 500), "Quit", ButtonAction.QuitGame));

            var pauseMenu = new Menu(MenuType.Pause);
            pauseMenu.Sprite = GameWorld.Instance.Sprites[MenuType.Pause][0];
            pauseMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(Camera.Instance.Position.X - 500, Camera.Instance.Position.Y + 100f), "Resume", ButtonAction.ResumeGame));
            pauseMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(Camera.Instance.Position.X - 500, Camera.Instance.Position.Y + 200f), "Quit", ButtonAction.QuitGame));
            pauseMenu.AddButtons(new Button(ButtonSpriteType.ButtonSquare, new Vector2(Camera.Instance.Position.X - 500, Camera.Instance.Position.Y + 300f), "Music", ButtonAction.ToggleMusic));
            pauseMenu.AddButtons(new Button(ButtonSpriteType.ButtonSquare, new Vector2(Camera.Instance.Position.X - 500, Camera.Instance.Position.Y + 400f), "Sound", ButtonAction.ToggleSound));

            menus.Add(MenuType.MainMenu, mainMenu);
            menus.Add(MenuType.GameOver, gameOverMenu);
            menus.Add(MenuType.Pause, pauseMenu);
            menus.Add(MenuType.Win, winMenu);
        }



        public void Update(Vector2 mousePos, bool isClicking)
        {
            currentMenu?.Update(InputHandler.Instance.MousePosition, isClicking);

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
