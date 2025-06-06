﻿using Microsoft.Xna.Framework;
using System;

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

        #endregion

        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Method
        /// <summary>
        /// Creating a new Enemy
        /// Rikke
        /// </summary>
        /// <param name="type">Enemytype</param>
        /// <param name="spawnPositio">Spawn position</param>
        /// <returns></returns>
        public override GameObject Create(Enum type, Vector2 spawnPositio)
        {
            return new Enemy(type, spawnPositio);

        }
        #endregion
    }
}
