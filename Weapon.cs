using Microsoft.Xna.Framework;
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
