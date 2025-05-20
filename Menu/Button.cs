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
        //public List<Button> buttonList = new List<Button>();
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
        public Button(ButtonType bg, Vector2 spawnPos, string buttonText) 
        {
            this.Type = bg;
            this.Position = spawnPos;
            this.ButtonText = buttonText;
            Layer = 0.7f;


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

                case "Resume":
                    break;

                case "Quit":
                    break;

                case "Try again?": //Reload
                    break;

                case "Music on/off": //Reload
                    break;

                case "Sound on/off": //Reload
                    break;

                default:
                    break;
            }

        }

        public Button(ButtonType bg, Vector2 spawnPos, string buttonText, ICommand command) : this(bg, spawnPos, buttonText) // Kalder eksisterende konstruktor
        {
            this.Command = command;
        }

        #endregion

        #region Method



        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            Color color = Hovering ? Color.HotPink : Color.White;

            spriteBatch.Draw(Sprite, Position, null, color, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.8f);

            // Tegn knaptekst centreret på knappen
            Vector2 textSize = font.MeasureString(ButtonText);
            Vector2 textPos = Position + new Vector2((Sprite.Width - textSize.X) / 2, (Sprite.Height - textSize.Y) / 2);
            spriteBatch.DrawString(font, ButtonText, textPos, Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.9f);

            
        }

        public void Update()
        {
            Hovering = CollisionBox.Contains(InputHandler.Instance.MousePosition.ToPoint());

            if (Hovering && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Command?.Execute();
            }
        }

        

        public static void AddButtons()
        {
            GameWorld.Instance.buttonList.Add(new Button(ButtonType.Button, new Vector2(Player.Instance.Position.X, Player.Instance.Position.Y + 200), "Start"), new StartGameCommand());
            GameWorld.Instance.buttonList.Add(new Button(ButtonType.Button, new Vector2(Player.Instance.Position.X, Player.Instance.Position.Y + 300), "Resume"), new ExitCommand());
            //GameWorld.Instance.buttonList.Add(new Button(ButtonType.Button, new Vector2(Player.Instance.Position.X, Player.Instance.Position.Y + 400), "Quit"));
            //GameWorld.Instance.buttonList.Add(new Button(ButtonType.Button, new Vector2(Player.Instance.Position.X, Player.Instance.Position.Y + 100), "Music on/off")); //musictoggle
            //GameWorld.Instance.buttonList.Add(new Button(ButtonType.Button, new Vector2(Player.Instance.Position.X, Player.Instance.Position.Y + 0), "Sound on/off")); //soundtoggle

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
