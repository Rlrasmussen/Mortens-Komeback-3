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
using Mortens_Komeback_3.Puzzles;

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
        private Vector2 meleeAttackDirection;
        private float speed = 500f;
        private float walkTimer = 0.5f;
        private int health;
        private int maxHealth = 100;
        private bool attacking = false;

        #endregion

        #region Properties

        /// <summary>
        /// Singleton property
        /// </summary>
        public static Player Instance
        {
            get
            {
                if (instance == null)
                    instance = new Player(PlayerType.Morten, GameWorld.Instance.Locations[Location.Spawn]);

                return instance;
            }
        }

        /// <summary>
        /// Used for handling logic when Player takes damage
        /// </summary>
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

        /// <summary>
        /// Used for doing actions if the player spawns/dies
        /// </summary>
        public override bool IsAlive
        {
            get => base.IsAlive;
            set => base.IsAlive = value;
        }

        /// <summary>
        /// Used for movement
        /// Simon
        /// </summary>
        public Vector2 Velocity { get => velocity; set => velocity = value; }

        /// <summary>
        /// Used for pixel-perfect collision
        /// Simon
        /// </summary>
        public List<RectangleData> Rectangles { get; set; } = new List<RectangleData>();

        /// <summary>
        /// Used for animation
        /// Simon
        /// </summary>
        public float FPS { get; set; } = 8;

        /// <summary>
        /// Array of Sprites used for animation
        /// Simon
        /// </summary>
        public Texture2D[] Sprites { get; set; }

        /// <summary>
        /// Used for animation
        /// Simon
        /// </summary>
        public float ElapsedTime { get; set; }

        /// <summary>
        /// Used for animation
        /// Simon
        /// </summary>
        public int CurrentIndex { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Sets animation sprites for Player and other basic logic, most notably where commands are added
        /// </summary>
        /// <param name="type">The starting sprites</param>
        /// <param name="spawnPos">Position the Player spawns</param>
        private Player(Enum type, Vector2 spawnPos) : base(type, spawnPos)
        {

            if (GameWorld.Instance.Sprites.TryGetValue(type, out var sprites))
                Sprites = sprites;
            else
                Debug.WriteLine("Kunne ikke sætte sprites for " + ToString());

            layer = 0.6f;

            AddCommands();

        }

        #endregion

        #region Method

        /// <summary>
        /// (Re)sets health and alive-status
        /// </summary>
        public override void Load()
        {

            base.Load();

            health = maxHealth;

        }

        /// <summary>
        /// Handles on-frame update logic for animations, timers, spriteeffects, pixel perfect collisionboxes position, camera movement and movement
        /// Simon
        /// </summary>
        /// <param name="gameTime">DeltaTime, obsolete</param>
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
                (this as IPPCollidable).UpdateRectangles(spriteEffect == SpriteEffects.FlipHorizontally);
                PlayWalkSound();
            }
            else if (attacking)
                (this as IAnimate).Animate();

            foreach (Weapon weapon in availableWeapons)
                weapon.Update(gameTime);

            Camera.Instance.Position = Position;

            base.Update(gameTime);

        }

        /// <summary>
        /// Handles Player movement logic
        /// Simon
        /// </summary>
        private void Move()
        {

            velocity.Normalize();

            Position += velocity * speed * GameWorld.Instance.DeltaTime;

            velocity = Vector2.Zero;

        }

        /// <summary>
        /// Handles drawing of animation and swords "swoosh"-effect and resetting sprites to standard when attack-animation is done
        /// Simon (ChatGPT made equation for the sword-swoosh effect)
        /// </summary>
        /// <param name="spriteBatch">Used for drawing sprites</param>
        public override void Draw(SpriteBatch spriteBatch)
        {

            Vector2 correction = Vector2.Zero; //Attack animation is 40'ish pixels higher than regular sprites

            if (attacking && equippedWeapon != null && (WeaponType)equippedWeapon.Type == WeaponType.Melee)
            {
                correction.Y = 40;
                // 1. Calculate angle of attack
                float angle = (float)Math.Atan2(meleeAttackDirection.Y, meleeAttackDirection.X);

                // 2. Set VFX origin (center-bottom or custom based on your sprite)
                Texture2D vfxTexture = (equippedWeapon as WeaponMelee).VFX[CurrentIndex];
                Vector2 vfxOrigin = new Vector2(0, vfxTexture.Height / 2f); // left center

                // 3. Set offset from character Position in the direction of attack
                Vector2 vfxOffset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * Sprite.Width / 2f;

                // 4. Draw at Position + offset
                spriteBatch.Draw(vfxTexture, Position + vfxOffset, null, drawColor, angle, vfxOrigin, scale, SpriteEffects.None, layer + 0.01f);
            }

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

        /// <summary>
        /// Effect that happens when a collision is triggered in GameWorld
        /// Simon
        /// </summary>
        /// <param name="other"></param>
        public void OnCollision(ICollidable other)
        {
            if (other.Type.GetType() == typeof(EnemyType))
            {
                
            }
            else
                switch (other.Type)
                {
                    case WeaponType.Melee:
                        if (other is Weapon)
                            availableWeapons.Add(other as Weapon);
                        (other as Weapon).IsAlive = false;
                        if (equippedWeapon == null)
                            equippedWeapon = (other as Weapon);
                        break;
                    case WeaponType.Ranged:
                        if (other is Weapon)
                            availableWeapons.Add(other as Weapon);
                        (other as Weapon).IsAlive = false;
                        break;
                    default:
                        break;
                }

        }

        /// <summary>
        /// Handles attack logic and dependent on attack type triggers change of sprites
        /// Simon
        /// </summary>
        private void Attack()
        {

            if (!GameWorld.Instance.GamePaused && equippedWeapon != null && !attacking)
            {
                equippedWeapon.Attack();
                if ((WeaponType)equippedWeapon.Type == WeaponType.Melee && GameWorld.Instance.Sprites.TryGetValue(PlayerType.MortenAngriber, out var sprites)) //Skal rykkes ind i samme loop som equippedWeapon.Attack();
                {
                    Sprites = sprites;
                    attacking = true;
                    CurrentIndex = 0;
                    ElapsedTime = 0;
                    FPS = 22;
                    GameWorld.Instance.Sounds[Sound.PlayerSwordAttack].Play();
                    meleeAttackDirection = InputHandler.Instance.MousePosition - Position;
                }
            }


        }

        /// <summary>
        /// Used by InputHandler to change the type of weapon that's equipped
        /// </summary>
        /// <param name="weapon">Type of weapon to change to</param>
        public void ChangeWeapon(WeaponType weapon)
        {

            if (!attacking)
                equippedWeapon = availableWeapons.Find(x => (WeaponType)x.Type == weapon);

        }

        /// <summary>
        /// Old solution from earlier game version to alternate walking-sound
        /// Philip
        /// </summary>
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

        /// <summary>
        /// Adds all players input to InputHandler
        /// Simon, Philip
        /// </summary>
        private void AddCommands()
        {

            InputHandler.Instance.LeftClickEventHandler += Attack;
            InputHandler.Instance.AddUpdateCommand(Keys.A, new MoveCommand(this, new Vector2(-1, 0)));
            InputHandler.Instance.AddUpdateCommand(Keys.D, new MoveCommand(this, new Vector2(1, 0)));
            InputHandler.Instance.AddUpdateCommand(Keys.W, new MoveCommand(this, new Vector2(0, -1)));
            InputHandler.Instance.AddUpdateCommand(Keys.S, new MoveCommand(this, new Vector2(0, 1)));
            InputHandler.Instance.AddButtonDownCommand(Keys.D1, new ChangeWeaponCommand(this, WeaponType.Melee));
            InputHandler.Instance.AddButtonDownCommand(Keys.D2, new ChangeWeaponCommand(this, WeaponType.Ranged));
            InputHandler.Instance.AddButtonDownCommand(Keys.E, new InteractCommand());

        }


        public void Interact(GameObject gameObject)
        {
            switch(gameObject.Type)
            {
                case PuzzleType.OrderPuzzlePlaque:
                    (gameObject as OrderPuzzlePlaque).ChangePlaque();
                    break;
                case PuzzleType.OrderPuzzle:
                    (gameObject as OrderPuzzle).TrySolve();
                    break;
                default:
                    break;
            }
             
        }

        #endregion
    }
}
