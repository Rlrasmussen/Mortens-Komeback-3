﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mortens_Komeback_3.Command;
using System;
using System.Collections.Generic;

namespace Mortens_Komeback_3.Menu
{
    public class Menu
    {
        #region Fields
        //private bool menuActive = false;
        public List<Button> buttonList = new List<Button>();
        private float scale = 1f;
        private Texture2D sprite;
        //public MenuType background { get; set; }
        //public Vector2 Position { get; set; } = Player.Instance.Position; 

        //public bool MenuActive { get => menuActive; set => menuActive = value; } 

        #endregion

        #region Properties
        public Vector2 Position { get; set; }
        public Enum Type { get; set; }
        public Texture2D Sprite
        {
            get => sprite;
            set
            {
                sprite = value;
                scale = (float)(GameWorld.Instance.ScreenSize.Y / sprite.Height);
            }
        }
        #endregion

        #region Constructor

        public Menu(MenuType type)
        {
            this.Type = type;
            AddCommands();
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
            Position = new Vector2(Camera.Instance.Position.X, Camera.Instance.Position.Y - 105f);

            // Flyt knapper relativt til menuens Position
            for (int i = 0; i < buttonList.Count; i++)
            {
                Vector2 offset = new Vector2(-97, i * (buttonList[i].Sprite.Height + 22)); // placér dem vertikalt
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

                spriteBatch.Draw(Sprite, Position, null, Color.White, 0f, origin, scale, SpriteEffects.None, 0.8f);
            }

            //spriteBatch.Draw(Sprite, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.8f);
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
