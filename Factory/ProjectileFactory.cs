﻿using Microsoft.Xna.Framework;
using System;

namespace Mortens_Komeback_3.Factory
{
    public class ProjectileFactory : Factory
    {
        #region Singelton
        private static ProjectileFactory instance;

        public static ProjectileFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProjectileFactory();
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
        /// Creating a new Porjectile
        /// Rikke
        /// </summary>
        /// <param name="type">Projectile type</param>
        /// <param name="spawnPosition"><Spawn position/param>
        /// <returns></returns>
        public override Projectile Create(Enum type, Vector2 spawnPosition)
        {
            //spawnPosition = Player.Instance.Position;

            return new Projectile(type, spawnPosition);
        }
        #endregion
    }
}
