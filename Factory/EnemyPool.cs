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
    internal class EnemyPool : ObjectPool
    {
        #region Singelton
        private static EnemyPool instance;

        public static EnemyPool Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EnemyPool();
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
        /// Creating an enemy with EnemyFacory (not Goosifer) for the Goosifer figth
        /// Rikke
        /// </summary>
        /// <returns></returns>
        public override Enemy Create()
        {
            return EnemyFactory.Instance.Create();
        }

        /// <summary>
        /// Creating Goosifer with EnemyFactory
        /// Rikke
        /// </summary>
        /// <returns></returns>
        public Enemy CreateGoosifer()
        {
            return EnemyFactory.Instance.CreateGoosifer();
        }

        /// <summary>
        /// Creating a specific goose at spawnposition
        /// Rikke
        /// </summary>
        /// <param name="type">Enemytype</param>
        /// <param name="spawnposition">Spawnposition</param>
        /// <returns></returns>
        public Enemy CreateSpecificGoose(EnemyType type, Vector2 spawnposition)
        {
            return EnemyFactory.Instance.CreateSpecificGoose(type, spawnposition);
        }
        #endregion
    }
}
