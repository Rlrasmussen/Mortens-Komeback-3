using Microsoft.Xna.Framework;
using System;

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
        /// Creating an Enemy from the instance of EnemyFactory
        /// Rikke
        /// </summary>
        /// <param name="type">Enemy type</param>
        /// <param name="spawnPosition">Spawn Position</param>
        /// <returns></returns>
        public override GameObject Create(Enum type, Vector2 spawnPosition)
        {
            return EnemyFactory.Instance.Create(type, spawnPosition);
        }

        #endregion
    }
}
