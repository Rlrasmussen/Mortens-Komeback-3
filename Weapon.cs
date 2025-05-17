using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mortens_Komeback_3.Collider;

namespace Mortens_Komeback_3
{
    public abstract class Weapon : GameObject, ICollidable
    {

        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructor


        protected Weapon(WeaponType type, Vector2 spawnPos) : base(type, spawnPos)
        {
        }

        #endregion

        #region Method

        /// <summary>
        /// Forced unique method
        /// </summary>
        public abstract void Attack();

        /// <summary>
        /// Effect that happens when a collision is triggered in GameWorld
        /// </summary>
        /// <param name="other">Other object partaking in collision</param>
        public void OnCollision(ICollidable other)
        {
            
        }

        #endregion
    }
}
