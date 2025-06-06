using Microsoft.Xna.Framework;
using Mortens_Komeback_3.Factory;
using System.Collections.Generic;

namespace Mortens_Komeback_3.State
{

    public class BossFightState : IState<Enemy>
    {

        #region Fields

        private Enemy parent;
        private Queue<Vector2> directions = new Queue<Vector2>();
        private Vector2 direction;
        private float directionChange = 1.5f;
        private float spawnEnemies;
        private float spewFire;
        private List<Vector2> initialVectors = new List<Vector2>()
        {
            new Vector2(0.62f, -0.78f),
            new Vector2(-0.89f, -0.46f),
            new Vector2(0.36f, 0.93f),
            new Vector2(0.02f, -1.00f),
            new Vector2(-0.77f, 0.63f),
            new Vector2(0.93f, -0.37f),
            new Vector2(0.21f, 0.98f),
            new Vector2(-0.48f, 0.07f)
        }; //Random pattern of normalized Vector2's calculated by ChatGPT to end around same place as it starts

        #endregion
        #region Properties

        public bool OverridesPathfinding { get; set; } = true;

        #endregion
        #region Constructor



        #endregion
        #region Methods

        /// <summary>
        /// Handles starting logic of the State
        /// Simon
        /// </summary>
        /// <param name="parent">Object that owns the State/subject of its effects</param>
        public void Enter(Enemy parent)
        {

            this.parent = parent;
            parent.State = this;
            parent.IgnoreState = true;
            foreach (Vector2 direction in initialVectors)
                directions.Enqueue(direction);

        }

        /// <summary>
        /// Handle movement logic and timers for attacks and wave-spawning
        /// Simon
        /// </summary>
        public void Execute()
        {

            parent.IgnoreState = false;

            directionChange += GameWorld.Instance.DeltaTime;
            spawnEnemies += GameWorld.Instance.DeltaTime;
            spewFire += GameWorld.Instance.DeltaTime;

            if (directionChange >= 1.5f)
            {
                direction = directions.Dequeue();
                parent.Direction = Player.Instance.Position - parent.Position;
                directions.Enqueue(direction);
                directionChange = 0;
            }

            parent.Position += direction * parent.Speed * GameWorld.Instance.DeltaTime;

            if (spawnEnemies >= 10)
                SpawnEnemies();

            if (spewFire >= 1.5f)
                SpewFire();

        }

        /// <summary>
        /// Unused for this State
        /// Simon
        /// </summary>
        public void Exit()
        {

            if (parent.Health <= 0)
                GameWorld.Instance.SpawnObject(new Item(ItemType.Grail, parent.Position));

        }

        /// <summary>
        /// Spawns 10-15 enemies depending which side they "charge" from with an even spread
        /// Simon
        /// </summary>
        private void SpawnEnemies()
        {

            spawnEnemies = 0;
            float increment = 0;

            switch (GameWorld.Instance.Random.Next(0, 2))
            {
                case 0:
                    int enemyRightAmount = 10;
                    for (int i = 0; i < enemyRightAmount; i++)
                    {
                        GameObject enemy = EnemyPool.Instance.GetObject(EnemyType.AggroGoose, parent.InRoom.Position + new Vector2(parent.InRoom.Sprite.Width / 2 + 75, -(parent.InRoom.Sprite.Height / 2) + increment + 75));
                        GameWorld.Instance.SpawnObject(enemy);
                        ChargeState chargePlayer = new ChargeState(new Vector2(-1, 0), parent);
                        chargePlayer.Enter(enemy as Enemy);
                        increment += (float)(parent.InRoom.Sprite.Height / enemyRightAmount);
                    }
                    break;
                case 1:
                    int enemyTopAmount = 15;
                    for (int i = 0; i < enemyTopAmount; i++)
                    {
                        GameObject enemy = EnemyPool.Instance.GetObject(EnemyType.AggroGoose, parent.InRoom.Position + new Vector2(-(parent.InRoom.Sprite.Width / 2 + 75) + increment, -(parent.InRoom.Sprite.Height / 2 + 75)));
                        GameWorld.Instance.SpawnObject(enemy);
                        ChargeState chargePlayer = new ChargeState(new Vector2(0, 1), parent);
                        chargePlayer.Enter(enemy as Enemy);
                        increment += (float)(parent.InRoom.Sprite.Width / enemyTopAmount) + 35;
                    }
                    break;
            }


        }

        /// <summary>
        /// Fire-breathing attack that fires a projectile (AvSurface - simply because the comparability and needed mechanics were already in place)
        /// Simon
        /// </summary>
        private void SpewFire()
        {

            spewFire = 0;
            GameWorld.Instance.SpawnObject(new GoosiferFire(SurfaceType.Fireball, parent.Position, 0, parent.Damage, parent));

        }

        #endregion
    }

}
