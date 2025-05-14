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
        private Projectile projectileGO;
        private Vector2 position;

        #endregion

        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Method
        public override Projectile Create()
        {
            //this.position =
            throw new NotImplementedException();
        }
        #endregion
    }
}
