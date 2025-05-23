using Microsoft.Xna.Framework;
using Mortens_Komeback_3.Factory;
using Mortens_Komeback_3.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }; //Random pattern calculated by ChatGPT

        #endregion
        #region Properties



        #endregion
        #region Constructor



        #endregion
        #region Methods


        public void Enter(Enemy parent)
        {

            this.parent = parent;
            parent.State = this;
            parent.IgnoreState = true;
            foreach (Vector2 direction in initialVectors)
                directions.Enqueue(direction);

        }


        public void Execute()
        {

            parent.IgnoreState = false;

            directionChange += GameWorld.Instance.DeltaTime;
            spawnEnemies += GameWorld.Instance.DeltaTime;
            spewFire += GameWorld.Instance.DeltaTime;

            if (directionChange >= 1.5f)
            {
                direction = directions.Dequeue();
                directions.Enqueue(direction);
                directionChange = 0;
            }

            parent.Position += direction * parent.Speed * GameWorld.Instance.DeltaTime;

            if (spawnEnemies >= 10)
                SpawnEnemies();

            if (spewFire >= 1.5f)
                SpewFire();

        }


        public void Exit()
        {



        }


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
                        GameObject enemy = EnemyPool.Instance.CreateSpecificGoose(EnemyType.AggroGoose, parent.InRoom.Position + new Vector2(parent.InRoom.Sprite.Width / 2 + 200, -(parent.InRoom.Sprite.Height / 2) + increment + 100));
                        GameWorld.Instance.SpawnObject(enemy);
                        ChargeState chargePlayer = new ChargeState(new Vector2(-1, 0));
                        chargePlayer.Enter(enemy as Enemy);
                        increment += (float)(parent.InRoom.Sprite.Height / enemyRightAmount);
                    }
                    break;
                case 1:
                    int enemyTopAmount = 15;
                    for (int i = 0; i < enemyTopAmount; i++)
                    {
                        GameObject enemy = EnemyPool.Instance.CreateSpecificGoose(EnemyType.AggroGoose, parent.InRoom.Position + new Vector2(-(parent.InRoom.Sprite.Width / 2 + 200) + increment, -(parent.InRoom.Sprite.Height / 2 + 200)));
                        GameWorld.Instance.SpawnObject(enemy);
                        ChargeState chargePlayer = new ChargeState(new Vector2(0, 1));
                        chargePlayer.Enter(enemy as Enemy);
                        increment += (float)(parent.InRoom.Sprite.Width / enemyTopAmount) + 35;
                    }
                    break;
            }


        }


        private void SpewFire()
        {

            spewFire = 0;
            GameWorld.Instance.SpawnObject(new GoosiferFire(OverlayObjects.Heart, parent.Position, 0f, parent.Damage));

        }

        #endregion
    }

}
