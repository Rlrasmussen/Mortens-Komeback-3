using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Mortens_Komeback_3.Factory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3
{
    public abstract class GameObject
    {
        #region Fields

        protected SpriteEffects spriteEffect = SpriteEffects.None;
        protected Color drawColor = Color.White;
        protected Vector2 origin = Vector2.Zero;
        protected Enum type;
        protected float scale = 1f;
        protected float layer = 0.5f;
        private float rotation = 0f;
        private bool isAlive = true;
        private Texture2D sprite;
        private Vector2 position;

        protected int damage; //The damage the object 

        #endregion
        #region Propertitties

        /// <summary>
        /// Bruges til automatisk fjernelse af objektet
        /// </summary>
        public virtual bool IsAlive
        {

            get => isAlive;

            set
            {

                isAlive = value;

            }

        }


        public virtual Texture2D Sprite { get => sprite; protected set => sprite = value; }


        public virtual Vector2 Position { get => position; set => position = value; }


        public virtual float Rotation { get => rotation; set => rotation = value; }


        public virtual Enum Type { get => type; }


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

        public int Damage { get => damage; set => damage = value; }

        #endregion
        #region Constructor

        /// <summary>
        /// Sætter automatisk sprite
        /// </summary>
        /// <param name="type">Bruges til at angive hvilke sprites der skal vises for objektet</param>
        /// <param name="spawnPos">Angiver startposition for objektet</param>
        public GameObject(Enum type, Vector2 spawnPos)
        {

            this.type = type;
            position = spawnPos;

            if (GameWorld.Instance.Sprites.TryGetValue(type, out var sprites))
                Sprite = sprites[0];
            else
                Debug.WriteLine("Kunne ikke sætte sprite for " + ToString());

            if (sprite != null)
                origin = new Vector2(Sprite.Width / 2, Sprite.Height / 2);

        }

        #endregion
        #region Methods

        /// <summary>
        /// Står for at nulstille/klargøre objektets primære parametre
        /// </summary>
        public virtual void Load()
        {

            isAlive = true;

        }


        public virtual void Update(GameTime gameTime) { }

        /// <summary>
        /// Håndterer visning af sprite
        /// </summary>
        /// <param name="spriteBatch">Game-logic</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {

            if (sprite != null)
                spriteBatch.Draw(Sprite, Position, null, drawColor, Rotation, origin, scale, spriteEffect, layer);

        }

        /// <summary>
        /// Kan bruges til at udskrive objektets type til string
        /// </summary>
        /// <returns>Type-enum'et som string</returns>
        public override string ToString()
        {

            return type.ToString();

        }

        #endregion
    }
}
