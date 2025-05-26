using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Mortens_Komeback_3.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.Direct3D9;

namespace Mortens_Komeback_3.Menu
{
    public class MenuManager
    {

        

        public bool Hovering { get; set; }


        private Menu currentMenu;

        private Dictionary<MenuType, Menu> menus = new Dictionary<MenuType, Menu>();


        public void CreateMenus()
        {
        

            var startMenu = new Menu(MenuType.Start);
            //startMenu.Sprite = GameWorld.Instance.Sprites[MenuType.Start][0];
            startMenu.AddButtons(new Button(ButtonType.Button, new Vector2(500, 300), "Start", new StartGameCommand()));
            startMenu.AddButtons(new Button(ButtonType.Button, new Vector2(500, 400), "Quit", new ExitCommand()));

            var gameOverMenu = new Menu(MenuType.GameOver);
            //gameOverMenu.Sprite = GameWorld.Instance.Sprites[MenuType.GameOver][0];
            //gameOverMenu.AddButtons(new Button(ButtonType.Button, new Vector2(500, 400), "Try Again", new RestartCommand()));
            gameOverMenu.AddButtons(new Button(ButtonType.Button, new Vector2(500, 500), "Quit", new ExitCommand()));

            var pauseMenu = new Menu(MenuType.Pause);
            pauseMenu.Sprite = GameWorld.Instance.Sprites[MenuType.Pause][0];
            //gameOverMenu.AddButtons(new Button(ButtonType.Button, new Vector2(500, 400), "Try Again", new RestartCommand()));
            pauseMenu.AddButtons(new Button(ButtonType.Button, new Vector2(Camera.Instance.Position.X + 100f, Camera.Instance.Position.X + 100f), "Resume", new ResumeCommand()));
            pauseMenu.AddButtons(new Button(ButtonType.Button, Player.Instance.Position, "Quit", new ExitCommand()));

            menus.Add(MenuType.Start, startMenu);
            menus.Add(MenuType.GameOver, gameOverMenu);
            menus.Add(MenuType.Pause, pauseMenu);
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

       
        public void Update(Vector2 mousePos, bool isClicking)
        {
            currentMenu?.Update(InputHandler.Instance.MousePosition, isClicking);

            if (InputHandler.Instance.LeftClick /*&& Hovering*/)
            {
                GameWorld.Instance.MenuManager.CloseMenu();
            }
           

        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            
            currentMenu?.Draw(spriteBatch, font);
        }
    }
}
