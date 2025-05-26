using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mortens_Komeback_3.Collider;
using Mortens_Komeback_3.Factory;
using System.Threading;
using Mortens_Komeback_3.Environment;
using Mortens_Komeback_3.State;

namespace Mortens_Komeback_3
{
    public class Enemy : GameObject, IAnimate, ICollidable, IPPCollidable
    {
        #region Fields

        private float speed;
        private bool pauseAStar = true;
        private AStar aStar = new AStar();
        private List<Tile> destinations = new List<Tile>();
        private int destinationsIndex = 0;
        private Vector2 destination;
        private Vector2 playerPreviousPos;
        private bool waitforAStar = false; //Used for making sure that player doens't move before a star proces is done.
        private IState<Enemy> state;

        #endregion

        #region Properties

        public float FPS { get; set; } = 6;
        public Texture2D[] Sprites { get; set; }
        public float ElapsedTime { get; set; }
        public int CurrentIndex { get; set; }
        public int Health { get; set; }
        public List<RectangleData> Rectangles { get; set; } = new List<RectangleData>();
        public float Speed { get => speed; }
        public IState<Enemy> State { get => state; set => state = value; }
        public List<Tile> Destinations { get => destinations; set => destinations = value; }
        public bool PauseAStar { get => pauseAStar; set => pauseAStar = value; }
        public Vector2 Direction { get; set; }
        public Room InRoom { get; set; }
        public bool IgnoreState { get; set; } = false;

        #endregion

        #region Constructor
        public Enemy(Enum type, Vector2 spawnPos) : base(type, spawnPos)
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

            float maxDistance = 1600f;
            foreach (Room room in DoorManager.Rooms) //Simon - checks for closest room
            {
                float distance = Vector2.Distance(Position, room.Position);
                if (distance < maxDistance)
                {
                    maxDistance = distance;
                    InRoom = room;
                }
            }

            (this as IAnimate).Animate();
            (this as IPPCollidable).UpdateRectangles(spriteEffect != SpriteEffects.None);

            if (state != null) //Simon - movement logic
                state.Execute();

            switch (Direction.X) //Simon - flips sprite according to movement
            {
                case > 0:
                    spriteEffect = SpriteEffects.FlipHorizontally;
                    break;
                case < 0:
                    spriteEffect = SpriteEffects.None;
                    break;
                default:
                    break;
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Sprites != null)
                spriteBatch.Draw(Sprites[CurrentIndex], Position, null, drawColor, Rotation, origin, scale, spriteEffect, layer);
            else
                base.Draw(spriteBatch);
        }

        /// <summary>
        /// Sets enemys stats according to data retrieved from database
        /// Simon
        /// </summary>
        public override void Load()
        {

            if (GameWorld.Instance.EnemyStats.TryGetValue((EnemyType)type, out var stats)) //Simon - Does a TryGetValue according to objects type against a dictionary, and retrieves a named tuple containing relevant data if successful
            {
                Health = stats.health;
                Damage = stats.damage;
                speed = stats.speed;
            }

            Thread aStarThread = new Thread(() => RunAStar(this, Player.Instance, DoorManager.Rooms.Find(x => (RoomType)x.Type == RoomType.PopeRoom).Tiles)); //Philip
            aStarThread.IsBackground = true;
            aStarThread.Start();

            if (!IgnoreState) //Simon - for setting a default State
            {
                BossFightState patrol = new BossFightState();
                patrol.Enter(this);
            }

            base.Load();
        }

        /// <summary>
        /// Collision with Enemy
        /// Rikke
        /// </summary>
        /// <param name="other">IColliable</param>
        public void OnCollision(ICollidable other)
        {
            //Take damage by collision
            if (other is Projectile)
            {
                TakeDamage((GameObject)other);
            }

        }

        /// <summary>
        /// Enemy takes damage which makes the Health reduce. When Health is 0 en Enemy dies and will be released fra EnemyPool
        /// Rikke
        /// </summary>
        /// <param name="gameObject">GameObject which give damage to the Enemy</param>
        public void TakeDamage(GameObject gameObject)
        {
            Health -= gameObject.Damage;

            //Enemy dies if Health is 0
            if (Health <= 0)
            {
                IsAlive = false;

                EnemyPool.Instance.ReleaseObject(this);
            }
        }

        /// <summary>
        /// Runs the AStar pathfinding for the enemy. 
        /// Philip
        /// </summary>
        /// <param name="enemy">The enemy that is getting a path </param>
        /// <param name="destinationObject">The objecct that is destination of the path</param>
        /// <param name="tiles">A dictionary of tiles, in the current room. </param>
        public void RunAStar(GameObject enemy, GameObject destinationObject, Dictionary<Vector2, Tile> tiles)
        {
            while (IsAlive) //Thread Runs as long as Enemy is alive.
            {
                while (pauseAStar == true) //Pause astar is managed by the Move method - makes sure the Astar thread doesn't use unnessecary resources
                {
                    Thread.Sleep(10);
                }
                List<Tile> path = aStar.AStarFindPath(enemy, destinationObject, tiles);
                if (path != null)
                {
                    destinationsIndex = 0;
                    destinations = path;
                    pauseAStar = true;
                }

                //waitforAStar = false;
            }
        }
        /// <summary>
        /// Moves the enemy through it's list of destinations. --- Deprecated into PatrolState that can also take a preset path
        /// Philip 
        /// </summary>
        public void Move()
        {

            if (Vector2.Distance(Position, destination) > 7 && !waitforAStar) //If destination is not reached, moves enemy to it's destination
            {
                if (Position.X + 5 < destination.X)
                    Direction += new Vector2(1, 0);
                else if (Position.X - 5 > destination.X)
                    Direction -= new Vector2(1, 0);

                if (Position.Y + 5 < destination.Y)
                    Direction += new Vector2(0, 1);
                else if (Position.Y - 5 > destination.Y)
                    Direction -= new Vector2(0, 1);

                Position += (speed * Direction * GameWorld.Instance.DeltaTime);

                Direction = Vector2.Zero;
            }
            else if (destinationsIndex < destinations.Count - 1 && !waitforAStar) //If destination is reached, and there are more destinations, sets the next
            {
                Debug.WriteLine("Enemy reached destination" + destination);
                destinationsIndex += 1; //NOTICE: first destination is ignored. 
                destination = destinations[destinationsIndex].Position;
                Debug.WriteLine("New destination:" + destination);
            }
            else if (!CollisionBox.Intersects(Player.Instance.CollisionBox) && !(playerPreviousPos == Player.Instance.Position) && !waitforAStar) //If destination is reached, there are no more, and enemy has not reached the player, a new path is set to be calculated by astar
            {
                Debug.WriteLine("Enemy reached final destination");
                playerPreviousPos = Player.Instance.Position;
                Debug.WriteLine("Enemy calls playerpos: " + Player.Instance.Position + "Enemy pos: " + Position);
                waitforAStar = true; //Make sure enemy doens't move until RunAstar-Method is done
                pauseAStar = false; //Makes the aStar thread not sleep
            }

        }
        #endregion
    }
}
