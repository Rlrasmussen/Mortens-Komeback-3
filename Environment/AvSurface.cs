using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mortens_Komeback_3.Collider;
using System;

namespace Mortens_Komeback_3.Environment
{
    public class AvSurface : GameObject, IAnimate, ICollidable
    {
        #region Fields

        private float gracePeriod = 2.1f;

        #endregion

        #region Properties

        public float FPS { get; set; }
        public Texture2D[] Sprites { get; set; }
        public float ElapsedTime { get; set; }
        public int CurrentIndex { get; set; }

        public override Rectangle CollisionBox //Simon
        {
            get
            {
                if (Sprite != null)
                    if (Rotation == 0 || Rotation == MathHelper.Pi)
                        return new Rectangle((int)(Position.X - (Sprite.Width / 2) * scale), (int)(Position.Y - (Sprite.Height / 2) * scale), (int)(Sprite.Width * scale), (int)(Sprite.Height * scale));
                    else
                        return new Rectangle((int)(Position.X - (Sprite.Height / 2) * scale), (int)(Position.Y - (Sprite.Width / 2) * scale), (int)(Sprite.Height * scale), (int)(Sprite.Width * scale));
                else return new Rectangle();
            }
        }

        #endregion

        #region Constructor
        public AvSurface(Enum type, Vector2 spawnPos, float rotation) : base(type, spawnPos)
        {
            this.scale = 0.4f;
            FPS = 6;

            if (GameWorld.Instance.Sprites.TryGetValue(type, out var sprites))
                Sprites = sprites;

            Rotation = rotation;
            damage = 1;
        }

        #endregion

        #region Method

        public override void Update(GameTime gameTime)
        {
            (this as IAnimate).Animate();

            gracePeriod += GameWorld.Instance.DeltaTime;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Sprites != null)
                spriteBatch.Draw(Sprites[CurrentIndex], Position, null, drawColor, Rotation, origin, scale, spriteEffect, layer);
            else
                base.Draw(spriteBatch);
        }

        /// <summary>
        /// Handles interaction with a collided object based on a switch case 
        /// Simon
        /// </summary>
        /// <param name="other">Object collided with</param>
        public virtual void OnCollision(ICollidable other)
        {

            switch (other)
            {
                case Player: //If other is "Player"
                    Player player = (Player)other;

                    if (gracePeriod >= 2f) //Damages the player if he hasn't been "burned" recently 
                    {
                        player.Health -= Damage;
                        GameWorld.Instance.Sounds[Sound.PlayerDamage].Play();
                        gracePeriod = 0f;
                    }

                    float distanceThrown = 150f; //"Throws" Player away from the fire
                    if (player.Position.X > CollisionBox.Right - 5)
                    {
                        if (!(player.Position.X + distanceThrown > GameWorld.Instance.CurrentRoom.CollisionBox.Right - 225))
                        {
                            player.Position = new Vector2(player.Position.X + distanceThrown, player.Position.Y);
                        }
                    }
                    else if (player.Position.X < CollisionBox.Left - 5)
                    {
                        if (!(player.Position.X - distanceThrown < GameWorld.Instance.CurrentRoom.CollisionBox.Left + 225))
                        {
                            player.Position = new Vector2(player.Position.X - distanceThrown, player.Position.Y);
                        }
                    }
                    if (player.Position.Y > CollisionBox.Bottom+5)
                    {
                        if (!(player.Position.Y + distanceThrown > GameWorld.Instance.CurrentRoom.CollisionBox.Bottom - 225))
                        {
                            player.Position = new Vector2(player.Position.X, player.Position.Y + distanceThrown);
                        }
                    }
                    else if (player.Position.Y < CollisionBox.Top-5)
                    {
                        if (!(player.Position.Y - distanceThrown < GameWorld.Instance.CurrentRoom.CollisionBox.Top + 105))
                        {
                            player.Position = new Vector2(player.Position.X, player.Position.Y - distanceThrown);
                        }
                    }

                    break;
                default:
                    break;
            }

        }

        #endregion
    }
}
