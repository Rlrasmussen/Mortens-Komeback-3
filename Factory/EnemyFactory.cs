using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using Mortens_Komeback_3.Command;

namespace Mortens_Komeback_3.Factory
{
    internal class EnemyFactory : Factory
    {
        #region Singelton
        private static EnemyFactory instance;

        public static EnemyFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EnemyFactory();
                }

                return instance;
            }
        }
        #endregion

        #region Fields
        private Vector2 position;
        private int nestNumbers = 4; 

        #endregion

        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Method
        /// <summary>
        /// Spawner Enemies in the figth with Goosifer
        /// Rikke
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Enemy Create()
        {
            //Spawn enemies in the Goosifer fight. Can spawn from choosen positions
            int nest = GameWorld.Instance.Random.Next(1, nestNumbers);

            //Spawn position
            switch (nest)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                default:
                    break;
            }

            //Can spawn any other goose than Goosifer
            int goose = GameWorld.Instance.Random.Next(0, Enum.GetNames(typeof(EnemyType)).Length);

            //Returning a enemy
            return new Enemy((EnemyType)goose, position);
        }

        /// <summary>
        /// Spawning Goosifer
        /// Rikke
        /// </summary>
        /// <returns></returns>
        public Enemy CreateGoosifer()
        {
            //this.position = 
            
            return new Enemy(EnemyType.Goosifer, position);
        }

        /// <summary>
        /// Spawn a specific goose - can not make a Goosifer
        /// Rikke
        /// </summary>
        /// <param name="type">Enemytype - can not be Goosifer</param>
        /// <param name="spawnposition">Spawn position</param>
        /// <returns></returns>
        public Enemy CreateSpecificGoose(EnemyType type, Vector2 spawnposition)
        {
            if (type is EnemyType.Goosifer) //If type is Goosifer it will change to another Enemytype
            {
                return new Enemy(EnemyType.WalkingGoose, spawnposition);
            }
            else
            {
                return new Enemy(type, spawnposition);
            }
        }

        #endregion
    }
}
