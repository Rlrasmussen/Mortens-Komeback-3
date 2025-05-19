using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mortens_Komeback_3.Collider;
using Mortens_Komeback_3.Command;

namespace Mortens_Komeback_3.Menu
{
    public class Button : ICollidable, ICommand
    {
        #region Fields

        #region Properties
        public string ButtonText { get; set; }
        public Enum ButtonType { get; set; }
        public ICommand Command { get; set; }
        public bool Hovering { get; set; }
        public bool IsEnabled { get; set; }
        public Action OnClick { get; set; }



        public Enum Type => throw new NotImplementedException();
        public Rectangle CollisionBox => new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height);

        public Vector2 Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Texture2D Sprite => throw new NotImplementedException();
        #endregion

        #region Constructor
        public Button(Enum bg, Vector2 spawnPos, string buttonText, ICommand command)
        {
            ButtonType = bg;
            this.Position = spawnPos;
            this.ButtonText = buttonText;
            this.Command = command;
        }
        #endregion

        #region Method

      

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            Color color = Hovering ? Color.Gray : Color.White;

            spriteBatch.Draw(Sprite, Position, color);

            // Tegn knaptekst centreret på knappen
            Vector2 textSize = font.MeasureString(ButtonText);
            Vector2 textPos = Position + new Vector2((Sprite.Width - textSize.X) / 2, (Sprite.Height - textSize.Y) / 2);
            spriteBatch.DrawString(font, ButtonText, textPos, Color.Black);
        }

       

       

       

        public void changeVolume()
        {

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
