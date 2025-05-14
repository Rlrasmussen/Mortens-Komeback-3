using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mortens_Komeback_3.Command;
using Mortens_Komeback_3.Collider;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3
{
    public class Player : GameObject, ICollidable, IPPCollidable, IAnimate
    {
        #region Fields

        private static Player instance;
        private Vector2 velocity;
        private float speed = 200f;

        #endregion

        #region Properties

        public static Player Instance
        {
            get
            {
                if (instance == null)
                    instance = new Player(PlayerType.Morten, Vector2.Zero);

                return instance;
            }
        }

        public Vector2 Velocity { get => velocity; set => velocity = value; }

        public List<RectangleData> Rectangles { get; set; } = new List<RectangleData>();
        public float FPS { get; set; } = 6;
        public Texture2D[] Sprites { get; set; }
        public float ElapsedTime { get; set; }
        public int CurrentIndex { get; set; }

        #endregion

        #region Constructor


        private Player(Enum type, Vector2 spawnPos) : base(type, spawnPos)
        {

            if (GameWorld.Instance.Sprites.TryGetValue(type, out var sprites))
                Sprites = sprites;
            else
                Debug.WriteLine("Kunne ikke sætte sprites for " + ToString());

        }

        #endregion

        #region Method


        public override void Update(GameTime gameTime)
        {

            if (InputHandler.Instance.Position.X < Position.X)
                spriteEffect = SpriteEffects.FlipHorizontally;
            else
                spriteEffect = SpriteEffects.None;

            if (velocity != Vector2.Zero)
            {
                Move();
                (this as IAnimate).Animate();
                (this as IPPCollidable).UpdateRectangles();
            }

            base.Update(gameTime);

            velocity = Vector2.Zero;

        }


        private void Move()
        {

            velocity.Normalize();

            Position += velocity * speed * GameWorld.Instance.DeltaTime;

        }


        public override void Draw(SpriteBatch spriteBatch)
        {

            if (Sprites != null)
                spriteBatch.Draw(Sprites[CurrentIndex], Position, null, drawColor, Rotation, origin, scale, spriteEffect, layer);
            else
                base.Draw(spriteBatch);

        }


        public void OnCollision(ICollidable other)
        {

            switch (other.Type)
            {

                default:
                    break;
            }

        }

        #endregion
    }
}
