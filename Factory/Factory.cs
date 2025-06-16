using Microsoft.Xna.Framework;
using System;

namespace Mortens_Komeback_3.Factory
{
    public abstract class Factory
    {

        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Method
        /// <summary>
        /// Creating a GameObject
        /// Rikke
        /// </summary>
        /// <param name="type">Enum type</param>
        /// <param name="spawnPositio">Spawn position</param>
        /// <returns></returns>
        public abstract GameObject Create(Enum type, Vector2 spawnPositio);

        #endregion
    }
}
