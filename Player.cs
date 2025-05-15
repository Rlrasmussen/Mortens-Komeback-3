using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mortens_Komeback_3.Command;
using Mortens_Komeback_3.Collider;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;

namespace Mortens_Komeback_3
{
    public class Player : GameObject, ICollidable, IPPCollidable, IAnimate
    {
        #region Fields

        private static Player instance;
        private SoundEffect currentWalkSound;
        private Weapon equippedWeapon;
        private List<Weapon> availableWeapons = new List<Weapon>();
        private Vector2 velocity;
        private float speed = 300f;
        private float walkTimer = 0.5f;
        private int health;
        private int maxHealth = 100;
        private bool attacking = false;

        #endregion

        #region Properties


        public static Player Instance
        {
            get
            {
                if (instance == null)
                    instance = new Player(PlayerType.Morten, GameWorld.Instance.Locations[Location.Spawn]);

                return instance;
            }
        }


        public int Health
        {
            get => health;
            set
            {
                if (value <= 0)
                    IsAlive = false;

                health = value;
            }
        }


        public override bool IsAlive
        {
            get => base.IsAlive;
            set => base.IsAlive = value;
        }


        public Vector2 Velocity { get => velocity; set => velocity = value; }


        public List<RectangleData> Rectangles { get; set; } = new List<RectangleData>();


        public float FPS { get; set; } = 8;


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

            layer = 0.6f;

            AddCommands();

            this.Damage = 1;

        }

        #endregion

        #region Method

        public override void Load()
        {

            base.Load();

            health = maxHealth;

        }


        public override void Update(GameTime gameTime)
        {

            walkTimer += GameWorld.Instance.DeltaTime;

            if (InputHandler.Instance.MousePosition.X < Position.X)
                spriteEffect = SpriteEffects.FlipHorizontally;
            else
                spriteEffect = SpriteEffects.None;

            if (velocity != Vector2.Zero && !attacking)
            {
                (this as IAnimate).Animate();
                Move();
                (this as IPPCollidable).UpdateRectangles();
                PlayWalkSound();
            }
            else if (attacking)
                (this as IAnimate).Animate();

            base.Update(gameTime);

        }


        private void Move()
        {

            velocity.Normalize();

            Position += velocity * speed * GameWorld.Instance.DeltaTime;

            velocity = Vector2.Zero;

        }


        public override void Draw(SpriteBatch spriteBatch)
        {

            Vector2 correction = Vector2.Zero;
            if (attacking)
                correction = new Vector2(0, 40);

            if (Sprites != null)
                spriteBatch.Draw(Sprites[CurrentIndex], Position - correction, null, drawColor, Rotation, origin, scale, spriteEffect, layer);
            else
                base.Draw(spriteBatch);

            if (attacking && CurrentIndex >= Sprites.Length - 1 && GameWorld.Instance.Sprites.TryGetValue(PlayerType.Morten, out var sprites))
            {
                Sprites = sprites;
                attacking = false;
                CurrentIndex = 0;
                ElapsedTime = 0;
                FPS = 8;
            }

        }


        public void OnCollision(ICollidable other)
        {
            if (other.Type.GetType() == typeof(EnemyType))
            {
                (other as GameObject).IsAlive = false;
            }
            else
                switch (other.Type)
                {
                    default:
                        break;
                }

        }


        private void Attack()
        {

            if (!GameWorld.Instance.GamePaused && equippedWeapon != null && !attacking)
            {
                equippedWeapon.Attack();
            }

            if (GameWorld.Instance.Sprites.TryGetValue(PlayerType.MortenAngriber, out var sprites)) //Skal rykkes ind i samme loop som equippedWeapon.Attack();
            {
                Sprites = sprites;
                attacking = true;
                CurrentIndex = 0;
                ElapsedTime = 0;
                FPS = 30;
                GameWorld.Instance.Sounds[Sound.PlayerSwordAttack].Play();
            }

        }


        public void ChangeWeapon(WeaponType weapon)
        {

            equippedWeapon = availableWeapons.Find(x => (WeaponType)x.Type == weapon);

        }


        private void PlayWalkSound()
        {
            if (walkTimer > 0.4f)
            {
                walkTimer = 0;
                if (currentWalkSound == GameWorld.Instance.Sounds[Sound.PlayerWalk2])
                {
                    GameWorld.Instance.Sounds[Sound.PlayerWalk1].Play();
                    currentWalkSound = GameWorld.Instance.Sounds[Sound.PlayerWalk1];
                }
                else
                {
                    GameWorld.Instance.Sounds[Sound.PlayerWalk2].Play();
                    currentWalkSound = GameWorld.Instance.Sounds[Sound.PlayerWalk2];
                }
            }
        }


        private void AddCommands()
        {

            InputHandler.Instance.LeftClickEventHandler += Attack;
            InputHandler.Instance.AddUpdateCommand(Keys.A, new MoveCommand(this, new Vector2(-1, 0)));
            InputHandler.Instance.AddUpdateCommand(Keys.D, new MoveCommand(this, new Vector2(1, 0)));
            InputHandler.Instance.AddUpdateCommand(Keys.W, new MoveCommand(this, new Vector2(0, -1)));
            InputHandler.Instance.AddUpdateCommand(Keys.S, new MoveCommand(this, new Vector2(0, 1)));
            InputHandler.Instance.AddButtonDownCommand(Keys.D1, new ChangeWeaponCommand(this, WeaponType.Melee));
            InputHandler.Instance.AddButtonDownCommand(Keys.D2, new ChangeWeaponCommand(this, WeaponType.Ranged));

        }

        #endregion
    }
}
