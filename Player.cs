using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mortens_Komeback_3.Collider;
using Mortens_Komeback_3.Command;
using Mortens_Komeback_3.Factory;
using Mortens_Komeback_3.Puzzles;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mortens_Komeback_3
{
    public class Player : GameObject, ICollidable, IPPCollidable, IAnimate
    {
        #region Fields
        private static Player instance;
        private SoundEffect currentWalkSound;
        private Weapon equippedWeapon;
        private List<GameObject> inventory = new List<GameObject>();
        private Vector2 velocity;
        private Vector2 meleeAttackDirection;
        private float speed = 500f;
        private float walkTimer = 0.5f;
        private int health;
        private int maxHealth = 6;
        private bool swordAttacking = false;
        private bool slingAttacking = false;
        private float colorTimer = 2f;
        private float damageTimer = 2f;
        private float damageGracePeriode = 2f;
        private int portionHealth = 2;

        #endregion

        #region Properties

        /// <summary>
        /// Singleton property
        /// Simon
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
        /// Simon
        /// </summary>
        public int Health
        {
            get => health;
            set
            {

                if (value >= health)
                {
                    health = value;
                }
                else
                {
                    if (value <= 0)
                    {
                        IsAlive = false;

                        //If Player is dead alle the Enemies og Projectile is being released back to the inactive stack i ObjectPool
                        EnemyPool.Instance.PlayerDead();
                        ProjectilePool.Instance.PlayerDead();
                        GameWorld.Instance.Notify(StatusType.PlayerDead);
                        GameWorld.Instance.DeathMusic = true;
                    }
                    health = value;
                    colorTimer = 0f;
                }

                GameWorld.Instance.Notify(StatusType.Health);

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
        /// Returns the "Inventory" for save-functionality
        /// Simon
        /// </summary>
        public List<GameObject> Inventory { get => inventory; }

        /// <summary>
        /// Returns which weapon (if any) is currently equipped for save-functionality
        /// Simon
        /// </summary>
        public Weapon EquippedWeapon { get => equippedWeapon; set => equippedWeapon = value; }

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
        public float Speed { get => speed; set => speed = value; }

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
#if DEBUG
            else
                Debug.WriteLine("Kunne ikke sætte sprites for " + ToString());
#endif
            layer = 0.6f;

            AddCommands();

        }

        #endregion

        #region Method

        /// <summary>
        /// (Re)sets health and alive-status
        /// Simon
        /// </summary>
        public override void Load()
        {
            CurrentIndex = 0;
            FPS = 8;
            base.Load();

            switch (equippedWeapon)
            {
                case null:
                case WeaponMelee:
                    if (GameWorld.Instance.Sprites.TryGetValue(PlayerType.Morten, out var meleeSprites))
                    {
                        Sprites = meleeSprites;
                        Sprite = Sprites[0];
                        origin = new Vector2(Sprite.Width / 2, Sprite.Height / 2);
                    }
                    Type = PlayerType.Morten;
                    Rectangles = (this as IPPCollidable).CreateRectangles();
                    break;
                case WeaponRanged:
                    if (GameWorld.Instance.Sprites.TryGetValue(PlayerType.MortenMunk, out var rangedSprites))
                    {
                        Sprites = rangedSprites;
                        Sprite = Sprites[0];
                        origin = new Vector2(Sprite.Width / 2, Sprite.Height / 2);
                    }
                    Type = PlayerType.MortenMunk;
                    Rectangles = (this as IPPCollidable).CreateRectangles();
                    break;
            }

            swordAttacking = false;
            speed = 500f;
            health = maxHealth;
            colorTimer = 2f;
            //GameWorld.Instance.DeathMusic = false;

            GameWorld.Instance.Notify(StatusType.Health);
        }

        /// <summary>
        /// Handles on-frame update logic for animations, timers, spriteeffects, pixel perfect collisionboxes position, camera movement and movement
        /// Simon
        /// </summary>
        /// <param name="gameTime">DeltaTime, obsolete</param>
        public override void Update(GameTime gameTime)
        {

            walkTimer += GameWorld.Instance.DeltaTime;
            colorTimer += GameWorld.Instance.DeltaTime;

            if (colorTimer >= 2f)
                drawColor = Color.White;
            else
                drawColor = Color.Red;

            if (InputHandler.Instance.MousePosition.X < Position.X)
                spriteEffect = SpriteEffects.FlipHorizontally;
            else
                spriteEffect = SpriteEffects.None;

            if (velocity != Vector2.Zero && !swordAttacking)
            {
                if (Sprites != null)
                    (this as IAnimate).Animate();
                Move();
                PlayWalkSound();
            }
            else if (swordAttacking)
                (this as IAnimate).Animate();
            else if (slingAttacking)
            { (this as IAnimate).Animate(); }

            foreach (GameObject go in inventory)
                go.Update(gameTime);


            (this as IPPCollidable).UpdateRectangles(spriteEffect == SpriteEffects.FlipHorizontally);

            Camera.Instance.Position = Position;

            //Grace periode for attacks from Enemy
            damageTimer += GameWorld.Instance.DeltaTime;

            base.Update(gameTime);

        }

        /// <summary>
        /// Handles Player movement logic
        /// Simon
        /// </summary>
        private void Move()
        {

            velocity.Normalize();
            //Player only moves if it doens't leave a rooms collisonbox, unless it's a double room. - Philip
            if (
                   (GameWorld.Instance.CurrentRoom.LeftSideOfBigRoom //If room is leftside, player is stopped from leaving, unless it's through right side
                   && !((Position.X - Sprite.Width / 2 + (velocity.X * speed * GameWorld.Instance.DeltaTime)) < GameWorld.Instance.CurrentRoom.CollisionBox.Left + 220)
                   && !((Position.Y - Sprite.Height / 2 + (velocity.Y * speed * GameWorld.Instance.DeltaTime)) < GameWorld.Instance.CurrentRoom.CollisionBox.Top + 100)
                   && !((Position.Y + Sprite.Height / 2 + (velocity.Y * speed * GameWorld.Instance.DeltaTime)) > GameWorld.Instance.CurrentRoom.CollisionBox.Bottom - 220))
                   ||
                   (GameWorld.Instance.CurrentRoom.RightSideOfBigRoom //If room is rightside, player is stopped from leaving, unless it's through left side
                   && !((Position.X + Sprite.Width / 2 + (velocity.X * speed * GameWorld.Instance.DeltaTime)) > GameWorld.Instance.CurrentRoom.CollisionBox.Right - 220)
                   && !((Position.Y - Sprite.Height / 2 + (velocity.Y * speed * GameWorld.Instance.DeltaTime)) < GameWorld.Instance.CurrentRoom.CollisionBox.Top + 100)
                   && !((Position.Y + Sprite.Height / 2 + (velocity.Y * speed * GameWorld.Instance.DeltaTime)) > GameWorld.Instance.CurrentRoom.CollisionBox.Bottom - 220))
                   ||
                   (GameWorld.Instance.CurrentRoom.TopSideOfBigRoom //If room is topside, player is stopped from leaving, unless it's through buttom
                   && !((Position.X + Sprite.Width / 2 + (velocity.X * speed * GameWorld.Instance.DeltaTime)) > GameWorld.Instance.CurrentRoom.CollisionBox.Right - 220)
                   && !((Position.X - Sprite.Width / 2 + (velocity.X * speed * GameWorld.Instance.DeltaTime)) < GameWorld.Instance.CurrentRoom.CollisionBox.Left + 220)
                   && !((Position.Y - Sprite.Height / 2 + (velocity.Y * speed * GameWorld.Instance.DeltaTime)) < GameWorld.Instance.CurrentRoom.CollisionBox.Top + 100)
                   ||
                   (GameWorld.Instance.CurrentRoom.ButtomSideOfBigRoom //If room is buttom, player is stopped from leaving, unless it's through top
                   && !((Position.X + Sprite.Width / 2 + (velocity.X * speed * GameWorld.Instance.DeltaTime)) > GameWorld.Instance.CurrentRoom.CollisionBox.Right - 220)
                   && !((Position.X - Sprite.Width / 2 + (velocity.X * speed * GameWorld.Instance.DeltaTime)) < GameWorld.Instance.CurrentRoom.CollisionBox.Left + 220)
                   && !((Position.Y + Sprite.Height / 2 + (velocity.Y * speed * GameWorld.Instance.DeltaTime)) > GameWorld.Instance.CurrentRoom.CollisionBox.Bottom - 220))
                   || //Else player can't move out of rooms collisonbox
                   (!((Position.X + Sprite.Width / 2 + (velocity.X * speed * GameWorld.Instance.DeltaTime)) > GameWorld.Instance.CurrentRoom.CollisionBox.Right - 220)
                   && !((Position.X - Sprite.Width / 2 + (velocity.X * speed * GameWorld.Instance.DeltaTime)) < GameWorld.Instance.CurrentRoom.CollisionBox.Left + 220)
                   && !((Position.Y - Sprite.Height / 2 + (velocity.Y * speed * GameWorld.Instance.DeltaTime)) < GameWorld.Instance.CurrentRoom.CollisionBox.Top + 100)
                   && !((Position.Y + Sprite.Height / 2 + (velocity.Y * speed * GameWorld.Instance.DeltaTime)) > GameWorld.Instance.CurrentRoom.CollisionBox.Bottom - 220)))
                   )
            {
                Position += velocity * Speed * GameWorld.Instance.DeltaTime;
            }
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

            if (Sprites == GameWorld.Instance.Sprites[PlayerType.Morten]) //Accidental auto-overtrimming of sprites
                switch (CurrentIndex)
                {
                    case 1 when spriteEffect == SpriteEffects.FlipHorizontally:
                    case 3 when spriteEffect == SpriteEffects.FlipHorizontally:
                        correction.X = 7;
                        break;
                    default:
                        break;
                }
            else if (Sprites == GameWorld.Instance.Sprites[PlayerType.MortenSling] && spriteEffect == SpriteEffects.FlipHorizontally) //Accidental auto-overtrimming of sprites
                correction.X = 30;


            if (swordAttacking && equippedWeapon != null && (WeaponType)equippedWeapon.Type == WeaponType.Melee)
            {
                correction.Y = 40;
                if (spriteEffect == SpriteEffects.FlipHorizontally)
                {
                    correction.X = 40;
                }
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

#if DEBUG
            if (GameWorld.Instance.DrawCollision)
                spriteBatch.DrawString(GameWorld.Instance.GameFont, $"X: {Position.X}\nY: {Position.Y}", Position + new Vector2(0, 100), Color.Green, 0f, Vector2.Zero, 2f, SpriteEffects.None, 1f);
#endif
            if (swordAttacking && CurrentIndex >= Sprites.Length - 1 && GameWorld.Instance.Sprites.TryGetValue(PlayerType.Morten, out var sprites))
            {
                Sprites = sprites;
                swordAttacking = false;
                CurrentIndex = 0;
                ElapsedTime = 0;
                FPS = 8;
            }
            if (slingAttacking && CurrentIndex >= Sprites.Length - 1 && GameWorld.Instance.Sprites.TryGetValue(PlayerType.MortenMunk, out var monksprites))
            {
                Sprites = monksprites;
                slingAttacking = false;
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
            if (other.Type.GetType() == typeof(EnemyType) && damageTimer > damageGracePeriode) //Rikke
            {
                Health -= (other as Enemy).Damage;
                GameWorld.Instance.Sounds[Sound.PlayerDamage].Play();
                damageTimer = 0f;
            }
            else //Simon
                switch (other.Type)
                {
                    case WeaponType.Melee:
                        if (other is Weapon)
                            inventory.Add(other as Weapon);
                        (other as Weapon).IsAlive = false;
                        if (equippedWeapon == null)
                            equippedWeapon = (other as Weapon);
                        GameWorld.Instance.Notify(StatusType.WeaponMelee);
                        break;
                    case WeaponType.Ranged:
                        if (other is Weapon)
                            inventory.Add(other as Weapon);
                        (other as Weapon).IsAlive = false;
                        GameWorld.Instance.Notify(StatusType.WeaponRanged);
                        break;
                    case ItemType.Bible:
                        if (other is Item)
                            inventory.Add(other as Item);
                        (other as Item).IsAlive = false;
                        GameWorld.Instance.Notify(StatusType.Bible);
                        break;
                    case ItemType.Rosary:
                        if (other is Item)
                            inventory.Add(other as Item);
                        (other as Item).IsAlive = false;
                        GameWorld.Instance.Notify(StatusType.Rosary);
                        break;
                    case ItemType.GeesusBlood:
                        (other as Item).IsAlive = false;
                        Health += portionHealth;
                        break;
                    case ItemType.Grail:
                        GameWorld.Instance.WinGame = true;
                        (other as Item).IsAlive = false;
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
            if (IsAlive && !GameWorld.Instance.GamePaused)
                if (!GameWorld.Instance.GamePaused && equippedWeapon != null && !(swordAttacking || slingAttacking))
                {
                    if ((WeaponType)equippedWeapon.Type == WeaponType.Melee && GameWorld.Instance.Sprites.TryGetValue(PlayerType.MortenAngriber, out var sprites)) 
                    {
                        Sprites = sprites;
                        swordAttacking = true;
                        CurrentIndex = 0;
                        ElapsedTime = 0;
                        FPS = 22;
                        GameWorld.Instance.Sounds[Sound.PlayerSwordAttack].Play();
                        meleeAttackDirection = InputHandler.Instance.MousePosition - Position;
                    }
                    else if ((WeaponType)equippedWeapon.Type == WeaponType.Ranged && GameWorld.Instance.Sprites.TryGetValue(PlayerType.MortenSling, out var slingsprites)) //Philip
                    {
                        if ((equippedWeapon as WeaponRanged).RefireRate >= 1f)
                        {
                            Sprites = slingsprites;
                            slingAttacking = true;
                            CurrentIndex = 0;
                            ElapsedTime = 0;
                            FPS = 3;
                        }
                    }
                    equippedWeapon.Attack();
                }
        }

        /// <summary>
        /// Used by InputHandler to change the type of weapon that's equipped
        /// Simon
        /// </summary>
        /// <param name="weapon">Type of weapon to change to</param>
        public void ChangeWeapon(WeaponType weapon)
        {

            if (inventory.Contains(inventory.Find(x => (WeaponType)x.Type == weapon)))
            {
                if (!swordAttacking)
                    equippedWeapon = (Weapon)inventory.Find(x => (WeaponType)x.Type == weapon);
                switch (equippedWeapon.Type) //Changes sprites, depending on weapon type - Philip
                {
                    case WeaponType.Melee:
                        GameWorld.Instance.Sprites.TryGetValue(PlayerType.Morten, out var meleeSprites);
                        Sprites = meleeSprites;
                        Sprite = Sprites[0];
                        Type = PlayerType.Morten;
                        origin = new Vector2(Sprite.Width / 2, Sprite.Height / 2);
                        break;
                    case WeaponType.Ranged:
                        GameWorld.Instance.Sprites.TryGetValue(PlayerType.MortenMunk, out var rangedSprites);
                        Sprites = rangedSprites;
                        Sprite = Sprites[0];
                        Type = PlayerType.MortenMunk;
                        origin = new Vector2(Sprite.Width / 2, Sprite.Height / 2);
                        break;
                    default: break;
                }
            }

            Rectangles = (this as IPPCollidable).CreateRectangles();

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

        /// <summary>
        /// Determines what to do when interaction is possible
        /// Philip, Rikke
        /// </summary>
        /// <param name="gameObject"></param>
        public void Interact(GameObject gameObject)
        {
            switch (gameObject.Type)
            {
                case PuzzleType.OrderPuzzlePlaque:
                    (gameObject as OrderPuzzlePlaque).ChangePlaque();
                    break;
                case PuzzleType.OrderPuzzle:
                    (gameObject as OrderPuzzle).TrySolve();
                    break;
                case PuzzleType.PathfindingPuzzle:
                    (gameObject as PathfindingPuzzle).TrySolve();
                    break;
                case NPCType.Pope:
                case NPCType.CanadaGoose:
                case NPCType.Monk:
                case NPCType.Nun:
                case NPCType.Coffin:
                case NPCType.Hole0:
                case NPCType.Empty:
                case NPCType.Ghost:
                case NPCType.Chest:
                    (gameObject as NPC).Speak();
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// Handles adding items to inventory-list and "equipping" a weapon
        /// Simon
        /// </summary>
        /// <param name="item">Object to be added</param>
        private void AddItemToInventory(GameObject item)
        {

            if (item is Weapon && equippedWeapon == null)
            {
                equippedWeapon = (Weapon)item;
                switch (equippedWeapon.Type) //Changes sprites, depending on weapon type - Philip
                {
                    case WeaponType.Melee:
                        if (GameWorld.Instance.Sprites.TryGetValue(PlayerType.Morten, out var meleeSprites))
                        {
                            Sprites = meleeSprites;
                            Sprite = Sprites[0];
                            origin = new Vector2(Sprite.Width / 2, Sprite.Height / 2);
                            Rectangles = (this as IPPCollidable).CreateRectangles();
                        }
                        break;
                    case WeaponType.Ranged:
                        if (GameWorld.Instance.Sprites.TryGetValue(PlayerType.MortenMunk, out var rangedSprites))
                        {
                            Sprites = rangedSprites;
                            Sprite = Sprites[0];
                            origin = new Vector2(Sprite.Width / 2, Sprite.Height / 2);
                            Rectangles = (this as IPPCollidable).CreateRectangles();
                        }
                        break;
                    default: break;
                }
            }

            inventory.Add(item);

        }

        /// <summary>
        /// Determines what kind of object to add to the inventory, and tries removing it from the "GameWorld" if not already
        /// Simon
        /// </summary>
        /// <param name="id">Identifier for the item to search for/add</param>
        public void AcquireItem(int id)
        {

            GameObject item;
            switch (id)
            {
                case 0:
                    item = GameWorld.Instance.GameObjects.Find(x => x is WeaponMelee);
                    if (item != null)
                    {
                        item.IsAlive = false;
                        Instance.AddItemToInventory(item);
                    }
                    else
                        Instance.AddItemToInventory(new WeaponMelee(WeaponType.Melee, Vector2.Zero));
                    break;
                case 1:
                    item = GameWorld.Instance.GameObjects.Find(x => x is WeaponRanged);
                    if (item != null)
                    {
                        item.IsAlive = false;
                        Instance.AddItemToInventory(item);
                    }
                    else
                        Instance.AddItemToInventory(new WeaponRanged(WeaponType.Ranged, Vector2.Zero));
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Method to get around property effects
        /// Simon
        /// </summary>
        /// <param name="health">Value to set health</param>
        public void SetHealthFromDB(int health)
        {

            this.health = health;

        }
        #endregion
    }
}
