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
        //private Button myButton;
        public List<Button> buttonList = new List<Button>();
        #endregion

        #region Properties
        public string ButtonText { get; set; }
        ////public Enum ButtonType { get; set; }
        public ICommand Command { get; set; }
        public bool Hovering { get; set; }
        public bool IsEnabled { get; set; }
        public Action OnClick { get; set; }
        public float Layer { get; set; }

        public Enum Type { get; set; }
        public Rectangle CollisionBox => new Rectangle((int)Position.X, (int)Position.Y, Sprite.Width, Sprite.Height);

        public Vector2 Position { get; set; }

        public Texture2D Sprite { get; set; }
        #endregion

        #region Constructor
        //public Button(ButtonType bg, Vector2 spawnPos, string buttonText, ICommand command)
        public Button(ButtonType bg, Vector2 spawnPos, string buttonText)
        {
            this.Type = bg;
            this.Position = spawnPos;
            this.ButtonText = buttonText;
            Layer = 0.7f;
            //this.Command = command;


            if (GameWorld.Instance.Sprites.TryGetValue(bg, out var spriteArray))
            {
                Sprite = spriteArray[0];
            }
            else
            {
                throw new Exception("Sprite ikke fundet for " + bg);
            }

            switch (buttonText)
            {
                case "Start":
                    break;

                case "Continue":
                    break;

                case "Quit":
                    break;

                case "Try again?": //Reload
                    break;

                default:
                    break;
            }

           



        }
        #endregion

        #region Method

      

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            Color color = Hovering ? Color.Gray : Color.White;

            spriteBatch.Draw(Sprite, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.8f);

            // Tegn knaptekst centreret på knappen
            Vector2 textSize = font.MeasureString(ButtonText);
            Vector2 textPos = Position + new Vector2((Sprite.Width - textSize.X) / 2, (Sprite.Height - textSize.Y) / 2);
            spriteBatch.DrawString(font, ButtonText, textPos, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);

            
        }

        public void Update()
        {

        }

        public void AddButtons()
        {
            buttonList.Add(new Button(ButtonType.Button, new Vector2(Player.Instance.Position.X, Player.Instance.Position.Y + 200), "Start"));
            buttonList.Add(new Button(ButtonType.Button, new Vector2(Player.Instance.Position.X, Player.Instance.Position.Y + 400), "Quit"));

            
        }

        //public void Loadcontent()
        //{
        //    myButton.Add(new Button(bg, new Vector2(Player.Instance.Position.X, Player.Instance.Position.Y + 200), "Start"));
        //    myButton.Add(new Button(ButtonType.Button, new Vector2(Player.Instance.Position.X, Player.Instance.Position.Y + 200), "Quit"));
        //}

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
