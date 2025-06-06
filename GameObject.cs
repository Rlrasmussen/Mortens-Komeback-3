using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;

namespace Mortens_Komeback_3
{
    public abstract class GameObject
    {
        #region Fields
        protected Enum type;
        private Vector2 position;
        protected Vector2 origin = Vector2.Zero;
        private bool isAlive = true;
        private Texture2D sprite;
        protected float layer = 0.5f;
        private float rotation = 0f;
        protected float scale = 1f;
        protected SpriteEffects spriteEffect = SpriteEffects.None;
        protected Color drawColor = Color.White;
        protected int damage; //The damage the object 
        

        #endregion
        #region Propertitties

        /// <summary>
        /// Bruges til automatisk fjernelse af objektet
        /// Simon
        /// </summary>
        public virtual bool IsAlive
        {

            get => isAlive;

            set
            {

                isAlive = value;

            }

        }

        /// <summary>
        /// Property for setting sprite in the class itself, or get info about it
        /// Simon
        /// </summary>
        public virtual Texture2D Sprite { get => sprite; protected set => sprite = value; }

        /// <summary>
        /// Used to manipulate objects position
        /// Simon
        /// </summary>
        public virtual Vector2 Position { get => position; set => position = value; }

        /// <summary>
        /// Used to rotate objects
        /// Simon
        /// </summary>
        public virtual float Rotation { get => rotation; set => rotation = value; }

        /// <summary>
        /// ID-tag for objects
        /// Simon
        /// </summary>
        public virtual Enum Type { get => type; set => type = value; }

        /// <summary>
        /// CollisionBox for objects
        /// Simon
        /// </summary>
        public virtual Rectangle CollisionBox
        {

            get
            {

                if (sprite != null)
                    return new Rectangle((int)(Position.X - (Sprite.Width / 2) * scale), (int)(Position.Y - (Sprite.Height / 2) * scale), (int)(Sprite.Width * scale), (int)(Sprite.Height * scale));
                else
                    return new Rectangle();

            }

        }

        /// <summary>
        /// Damage that the objects deal (if applicable)
        /// </summary>
        public int Damage { get => damage; set => damage = value; }

        #endregion
        #region Constructor

        /// <summary>
        /// Sætter automatisk sprite
        /// Simon
        /// </summary>
        /// <param name="type">Bruges til at angive hvilke sprites der skal vises for objektet</param>
        /// <param name="spawnPos">Angiver startposition for objektet</param>
        public GameObject(Enum type, Vector2 spawnPos)
        {

            this.type = type;
            position = spawnPos;

            if (GameWorld.Instance.Sprites.TryGetValue(type, out var sprites))
                Sprite = sprites[0];
#if DEBUG
            else
                Debug.WriteLine("Kunne ikke sætte sprite for " + ToString());
#endif
            if (sprite != null)
                origin = new Vector2(Sprite.Width / 2, Sprite.Height / 2);

        }

        #endregion
        #region Methods

        /// <summary>
        /// Står for at nulstille/klargøre objektets primære parametre
        /// Simon
        /// </summary>
        public virtual void Load()
        {

            isAlive = true;

        }


        public virtual void Update(GameTime gameTime) { }

        /// <summary>
        /// Håndterer visning af sprite
        /// Simon
        /// </summary>
        /// <param name="spriteBatch">Game-logic</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {

            if (sprite != null)
                spriteBatch.Draw(Sprite, Position, null, drawColor, Rotation, new Vector2(Sprite.Width / 2, Sprite.Height / 2), scale, spriteEffect, layer);

        }

        /// <summary>
        /// Kan bruges til at udskrive objektets type til string
        /// Simon
        /// </summary>
        /// <returns>Type-enum'et som string</returns>
        public override string ToString()
        {

            return type.ToString();

        }

        #endregion
    }
}
