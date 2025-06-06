using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mortens_Komeback_3.Command;
using System.Collections.Generic;

namespace Mortens_Komeback_3.Menu
{
    public class MenuManager
    {
        #region Field
        private string text;
        private Menu currentMenu;

        private Dictionary<MenuType, Menu> menus = new Dictionary<MenuType, Menu>();

        #endregion

        #region Properties
        public bool Hovering { get; set; }

        #endregion

        #region Constructor

        #endregion

        #region Method

        public void CreateMenus()
        {
            var mainMenu = new Menu(MenuType.MainMenu);
            mainMenu.Sprite = GameWorld.Instance.Sprites[MenuType.MainMenu][0];
            mainMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(500, 300), "Start", ButtonAction.StartGame));
            mainMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(500, 400), "Quit", ButtonAction.QuitGame));

            var gameOverMenu = new Menu(MenuType.GameOver);
            gameOverMenu.Sprite = GameWorld.Instance.Sprites[MenuType.GameOver][0];
            gameOverMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(500, 200), "Reload", ButtonAction.Reload));
            gameOverMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(500, 400), "Try Again", ButtonAction.TryAgain));
            gameOverMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(500, 500), "Quit", ButtonAction.QuitGame));
            
            var winMenu = new Menu(MenuType.Win);
            winMenu.Sprite = GameWorld.Instance.Sprites[MenuType.Win][0];
            winMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(500, 400), "Try Again", ButtonAction.TryAgain));
            winMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(500, 500), "Quit", ButtonAction.QuitGame));

            var pauseMenu = new Menu(MenuType.Pause);
            pauseMenu.Sprite = GameWorld.Instance.Sprites[MenuType.Pause][0];

            pauseMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(Camera.Instance.Position.X - 500, Camera.Instance.Position.Y + 100f), "Resume", ButtonAction.ResumeGame));
            pauseMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(500, 200), "Reload", ButtonAction.Reload));
            pauseMenu.AddButtons(new Button(ButtonSpriteType.Button, new Vector2(Camera.Instance.Position.X - 500, Camera.Instance.Position.Y + 200f), "Quit", ButtonAction.QuitGame));
            //pauseMenu.AddButtons(new Button(ButtonSpriteType.ButtonSquare, new Vector2(Camera.Instance.Position.X - 500, Camera.Instance.Position.Y + 300f), "Music", ButtonAction.ToggleMusic));
            //pauseMenu.AddButtons(new Button(ButtonSpriteType.ButtonSquare, new Vector2(Camera.Instance.Position.X - 500, Camera.Instance.Position.Y + 400f), "Sound", ButtonAction.ToggleSound));

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


        #endregion

    }
}
