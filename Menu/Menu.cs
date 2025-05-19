using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public bool menuActive = false;
        public List<Button> buttonList = new List<Button>();

        #endregion

        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Method
        public void AddButtons(Button button)
        {
            buttonList.Add(button);
        }

        public void Update(GameTime gametime)
        {
            foreach (var button in buttonList)
            {
                button.Hovering = button.CollisionBox.Contains(InputHandler.Instance.MousePosition.ToPoint());
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont buttonFont)
        {
            foreach (Button button in buttonList)
            {
                button.Draw(spriteBatch, buttonFont);
            }
        }

        private void AddCommands()
        {

            InputHandler.Instance.LeftClickEventHandler += HandleLeftClick;
        }

        public void HandleLeftClick()
        {
            foreach (Button button in buttonList)
            {
                if (button.CollisionBox.Contains(InputHandler.Instance.MousePosition.ToPoint()))
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
        #endregion
    }
}
