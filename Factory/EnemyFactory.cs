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
        private Enemy enemyGO;
        private Vector2 position;

        #endregion

        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Method
        public override Enemy Create()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
