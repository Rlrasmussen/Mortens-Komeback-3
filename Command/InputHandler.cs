using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace Mortens_Komeback_3.Command
{
    public class InputHandler
    {
        #region Fields

        private Enum type;                                                         //Generic for defining object
        private Thread inputThread;                                             //Thread for running mouse input non-stop
        private Texture2D sprite;                                               //Custom mouse cursor texture
        private static InputHandler instance;
        private Vector2 position;
        private bool leftClick;
        private bool rightClick;
        private bool ranLeftClick = false;                                      //Blocks more than one run of event
        private bool ranRightClick = false;                                     //Blocks more than one run of event
        private Dictionary<Keys, ICommand> keybindsUpdate = new Dictionary<Keys, ICommand>();
        private Dictionary<Keys, ICommand> keybindsButtonDown = new Dictionary<Keys, ICommand>();
        private KeyboardState previousKeyState;
        private readonly object syncLock = new object();

        /// <summary>
        /// Left click eventhandler
        /// </summary>
        public Action LeftClickEventHandler;

        /// <summary>
        /// Right click eventhandler
        /// </summary>
        public Action RightClickEventHandler;


        public static InputHandler Instance
        {
            get
            {
                if (instance == null)
                    instance = new InputHandler(MenuType.Cursor);

                return instance;
            }
        }


        #endregion

        #region Properties

        /// <summary>
        /// CollisionBox for registering mouse/objects overlap
        /// </summary>
        public Rectangle CollisionBox
        {
            get { return new Rectangle((int)position.X, (int)position.Y, 1, 1); }
        }

        /// <summary>
        /// Used externally for mouse position referrencing
        /// </summary>
        public Vector2 MousePosition { get => position; }

        /// <summary>
        /// Handles mouse left-click event and external detection thereof
        /// </summary>
        public bool LeftClick
        {
            get => leftClick;
            private set
            {
                leftClick = value;
                if (leftClick && !ranLeftClick)
                {
                    LeftClickEventHandler?.Invoke();
                    ranLeftClick = true;
                }
                else if (!leftClick && ranLeftClick)
                {
                    ranLeftClick = false;
                }
            }
        }

        /// <summary>
        /// Handles mouse right-click event and external detection thereof
        /// </summary>
        public bool RightClick
        {
            get => rightClick;
            private set
            {
                rightClick = value;
                if (rightClick && !ranRightClick)
                {
                    RightClickEventHandler?.Invoke();
                    ranRightClick = true;
                }
                else if (!rightClick && ranRightClick)
                    ranRightClick = false;
            }
        }

        #endregion

        #region Constructor

        private InputHandler(Enum type)
        {

            this.type = type;

            if (GameWorld.Instance.Sprites.TryGetValue(type, out var sprites))
                sprite = sprites[0];
            else
                Debug.WriteLine("Kunne ikke sætte sprite for " + ToString());

            inputThread = new Thread(HandleInput);
            inputThread.IsBackground = true;
            inputThread.Start();
            LeftClickEventHandler += LeftClickAction;
            RightClickEventHandler += RightClickAction;

        }

        #endregion

        #region Method


        public void AddUpdateCommand(Keys key, ICommand command)
        {

            if (keybindsUpdate.ContainsKey(key) && keybindsUpdate[key].Equals(command))
                return;
            else
                keybindsUpdate.Add(key, command);

        }


        public void AddButtonDownCommand(Keys key, ICommand command)
        {

            if (keybindsButtonDown.ContainsKey(key) && keybindsButtonDown[key].Equals(command))
                return;
            else
                keybindsButtonDown.Add(key, command);

        }


        public void Draw(SpriteBatch spriteBatch)
        {

            if (sprite != null)
                spriteBatch.Draw(sprite, position, null, Color.White, 0f, Vector2.Zero, 0.5f, SpriteEffects.FlipHorizontally, 1f);

#if DEBUG
            if (GameWorld.Instance.DrawCollision)
                spriteBatch.DrawString(GameWorld.Instance.GameFont, $"X: {position.X}\nY: {position.Y}", position + new Vector2(0, 50), Color.Green, 0f, Vector2.Zero, 2f, SpriteEffects.None, 1f);
#endif

        }

        /// <summary>
        /// Method to run when left mouse is clicked
        /// </summary>
        private void LeftClickAction()
        {



        }

        /// <summary>
        /// Method to run when right mouse is clicked
        /// </summary>
        private void RightClickAction()
        {



        }

        /// <summary>
        /// Thread function to continuously loop HandleInput which translates player input
        /// </summary>
        private void HandleInput()
        {

            while (GameWorld.Instance.GameRunning)
            {
                MouseState mouseState = Mouse.GetState();
                position = Camera.Instance.RefactorPosition(mouseState.Position.ToVector2());
                LeftClick = mouseState.LeftButton == ButtonState.Pressed;
                RightClick = mouseState.RightButton == ButtonState.Pressed;
                Execute();
            }

        }

        /// <summary>
        /// Checks if mouse is on top of GameObject
        /// </summary>
        /// <param name="other">GameObject to be checked with</param>
        /// <returns>true if yes</returns>
        private bool CheckCollision(Rectangle collisionBox, Rectangle other) => collisionBox.Intersects(other);

        /// <summary>
        /// Method to interact with "collided" object
        /// </summary>
        /// <param name="other">GameObject to manipulate</param>
        private void OnCollision(GameObject other)
        {



        }



        private void Execute()
        {

            KeyboardState keyboardState = Keyboard.GetState();

            lock (syncLock)
                foreach (var pressedKey in keyboardState.GetPressedKeys())
                {
                    if (keybindsUpdate.TryGetValue(pressedKey, out ICommand command))
                    {
                        command.Execute();
                    }
                    if (!previousKeyState.IsKeyDown(pressedKey) && keyboardState.IsKeyDown(pressedKey))
                    {
                        if (keybindsButtonDown.TryGetValue(pressedKey, out ICommand commandButtonDown))
                        {
                            commandButtonDown.Execute();
                        }
                    }
                }

            previousKeyState = keyboardState;

        }


        public override string ToString()
        {
            return type.ToString();
        }


        #endregion
    }
}
