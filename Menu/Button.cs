﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mortens_Komeback_3.Collider;
using Mortens_Komeback_3.Command;
using System;

namespace Mortens_Komeback_3.Menu
{
    public class Button : ICollidable, ICommand
    {
        #region Fields
        private Vector2 position;
        #endregion

        #region Properties
        public string ButtonText { get; set; }
        public ICommand Command { get; set; }
        public bool Hovering { get; set; }
        public bool IsEnabled { get; set; }
        public Action OnClick { get; set; }
        public ButtonAction Action { get; set; }
        public float Layer { get; set; }

        public Enum Type { get; set; }
        public bool IsClicked(MouseState mouse, MouseState prevMouse)
        {
            return CollisionBox.Contains(mouse.Position) &&
                   mouse.LeftButton == ButtonState.Pressed &&
                   prevMouse.LeftButton == ButtonState.Released;
        }

        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    Sprite.Width,
                    Sprite.Height
                );
            }
        }


        public Vector2 Position { get; set; }
        //public Vector2 screenCenter;


        public Texture2D Sprite { get; set; }
        #endregion

        #region Constructor
        public Button(
            ButtonSpriteType type,
            Vector2 spawnPos,
            string buttonText,
            ButtonAction action,
            ICommand command = null) 
        {
            this.Type = type;
            this.Position = spawnPos;
            this.ButtonText = buttonText;
            this.IsEnabled = true;
            if (command != null)
            {
                this.Command = command;
            }
            Layer = 0.9f;
            Action = action;
      

            if (GameWorld.Instance.Sprites.TryGetValue(type, out var spriteArray))
            {
                Sprite = spriteArray[0];
            }
            else
            {
                throw new Exception("Sprite ikke fundet for " + type);
            }

        }

        #endregion

        #region Method

        public void Update(Vector2 mousePos, bool isClicking)
        {
            Hovering = CollisionBox.Contains(InputHandler.Instance.MousePosition.ToPoint());

            if (Hovering && isClicking)
            {
                GameWorld.Instance.HandleButtonAction(Action);
            }

        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            Color color = Hovering ? Color.DarkSlateGray : Color.White;
            Color fontColor = Hovering ? Color.AntiqueWhite : Color.Black;

            spriteBatch.Draw(Sprite, Position, null, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);

            // Beregn tekstens størrelse
            Vector2 textSize = font.MeasureString(ButtonText);
            Vector2 textPos = Position + new Vector2((Sprite.Width - textSize.X) / 2f, (Sprite.Height - textSize.Y) / 2f);

            // Tegn teksten centreret over knappen
            spriteBatch.DrawString(font, ButtonText, textPos, fontColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.92f);
        }
            

        public void Execute()
        {
            throw new NotImplementedException();
        }

        public void OnCollision(ICollidable other)
        {
            throw new NotImplementedException();
        }

        
        #endregion
    }
}
