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
    public class ProjectilePool : ObjectPool
    {
        #region Singelton
        private static ProjectilePool instance;

        public static ProjectilePool Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProjectilePool();
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
        /// Creating a new Projectile through ProjectileFactgory
        /// Rikke
        /// </summary>
        /// <param name="type">Projectile type</param>
        /// <param name="spawnPosition">Spawn position</param>
        /// <returns></returns>
        public override GameObject Create(Enum type, Vector2 spawnPosition)
        {
            return ProjectileFactory.Instance.Create(type, spawnPosition);
        }

        #endregion
    }
}
