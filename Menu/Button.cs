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
        private Vector2 position;
        #endregion

        #region Properties
        //public Vector2 cameraCenter = Camera.Instance.Position;
        //private Vector2 buttonOffset = new Vector2(0, 100); // fx under midten
        //private Vector2 buttonPosition; 
        public string ButtonText { get; set; }
        public ICommand Command { get; set; }
        public bool Hovering { get; set; }
        public bool IsEnabled { get; set; }
        public Action OnClick { get; set; }
        public float Layer { get; set; }

        public Enum Type { get; set; }
        //public Rectangle CollisionBox => new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height);
        //public Rectangle CollisionBox
        //{
        //    get
        //    {
        //        Vector2 origin = new Vector2(Sprite.Width / 2f, Sprite.Height / 2f);
        //        return new Rectangle(
        //            (int)(Position.X - origin.X),
        //            (int)(Position.Y - origin.Y),
        //            Sprite.Width,
        //            Sprite.Height
        //        );
        //    }
        //}
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
        public Vector2 screenCenter;


        public Texture2D Sprite { get; set; }
        //public Vector2 ButtonPosition { get => buttonPosition; set => buttonPosition = value; }
        #endregion

        #region Constructor
        public Button(ButtonType type, Vector2 spawnPos, string buttonText, ICommand command) 
        {
            this.Type = type;
            //spawnPos = cameraCenter + buttonOffset;
            this.Position = spawnPos;
            this.ButtonText = buttonText;
            Layer = 0.9f;
            //Hovering = false;


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
            //Hovering = CollisionBox.Contains(mousePos.ToPoint());
            Hovering = CollisionBox.Contains(InputHandler.Instance.MousePosition.ToPoint());


            if (Hovering && isClicking)
            {
                OnClick?.Invoke();
            }
            

        }
      


        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            Color color = Hovering ? Color.HotPink : Color.White;

            // Midten af knappen
            //Vector2 origin = new Vector2(Sprite.Width / 2f, Sprite.Height / 2f);

            // Tegn knappen
            spriteBatch.Draw(Sprite, Position, null, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.8f);

            // Beregn tekstens størrelse
            //Vector2 textSize = font.MeasureString(ButtonText);
            //Vector2 textOrigin = textSize / 2f;

            Vector2 textSize = font.MeasureString(ButtonText);
            Vector2 textPos = Position + new Vector2((Sprite.Width - textSize.X) / 2f, (Sprite.Height - textSize.Y) / 2f);


            // Tegn teksten centreret over knappen
            spriteBatch.DrawString(font, ButtonText, textPos, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);
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
