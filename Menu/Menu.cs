using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mortens_Komeback_3.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3.Menu
{
    public class Menu
    {
        #region Fields
        //private bool menuActive = false;
        public List<Button> buttonList = new List<Button>();
        //public MenuType background { get; set; }
        //public Vector2 Position { get; set; } = Player.Instance.Position; 
        public Vector2 Position { get; set; }
        public Enum Type { get; set; }
        public Texture2D Sprite { get; set; }
        //public bool MenuActive { get => menuActive; set => menuActive = value; } 



        #endregion

        #region Properties

        #endregion

        #region Constructor

        public Menu(MenuType type)
        {
            this.Type = type;

        }

        #endregion

        #region Method

        public void Update(Vector2 mousePos, bool isClicking)
        {
            foreach (var button in buttonList)
            {
                button.Update(mousePos, isClicking); 
            }

            // Opdater menuens center hver frame
            Position = Camera.Instance.Position;

            // Flyt knapper relativt til menuens Position
            for (int i = 0; i < buttonList.Count; i++)
            {
                Vector2 offset = new Vector2(0, i * (buttonList[i].Sprite.Height + 10)); // placér dem vertikalt
                buttonList[i].Position = Position + offset;
                buttonList[i].Update(mousePos, isClicking);
            }

        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont buttonFont)
        {
            if (Sprite != null)
            {
                Vector2 Position = Camera.Instance.Position;
                Vector2 origin = new Vector2(Sprite.Width / 2f, Sprite.Height / 2f);

                spriteBatch.Draw(Sprite, Position, null, Color.White, 0f, origin, 1f, SpriteEffects.None, 0.8f);
            }

            //spriteBatch.Draw(Sprite, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.8f);
            foreach (Button button in buttonList)
            {
                button.Draw(spriteBatch, buttonFont);
            }
        }

        //private void AddCommands()
        //{

        //    InputHandler.Instance.LeftClickEventHandler += HandleLeftClick;
        //}

        public void HandleLeftClick()
        {
            foreach (Button button in buttonList)
            {
                if (button.CollisionBox.Intersects(InputHandler.Instance.CollisionBox))
                {
                    if (button.IsEnabled)
                    {
                        button.Command?.Execute();
                        button.OnClick?.Invoke();

                        break; // Klik kun én knap
                    }
                }
            }
        }
        public void AddButtons(Button button)
        {
            buttonList.Add(button);
        }
       
        #endregion
    }
}
