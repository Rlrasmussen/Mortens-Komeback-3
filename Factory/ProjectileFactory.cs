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
        /// Creating a projectile at the Players position
        /// Rikke
        /// </summary>
        /// <returns></returns>
        public override Projectile Create()
        {
            return new Projectile(AttackType.Egg, Player.Instance.Position);
        }
        #endregion
    }
}
