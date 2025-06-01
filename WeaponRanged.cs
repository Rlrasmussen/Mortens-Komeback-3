using Microsoft.Xna.Framework;
using Mortens_Komeback_3.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3
{
    public class WeaponRanged : Weapon
    {

        #region Fields

        private float refireRate = 1f;


        #endregion

        #region Properties
        public float RefireRate { get => refireRate; private set => refireRate = value; }

        #endregion

        #region Constructor

        /// <summary>
        /// Sets damage of the projectile, sprite is set in base constructor
        /// Simon
        /// </summary>
        /// <param name="type">The sprite of the weapon</param>
        /// <param name="spawnPos">Spawning position of the weapon</param>
        public WeaponRanged(WeaponType type, Vector2 spawnPos) : base(type, spawnPos)
        {
            damage = 2;
        }

        #endregion

        #region Method

        /// <summary>
        /// Handles (re)fire-rate
        /// Simon
        /// </summary>
        /// <param name="gameTime">DeltaTime, obsolete</param>
        public override void Update(GameTime gameTime)
        {

            RefireRate += GameWorld.Instance.DeltaTime;

            base.Update(gameTime);

        }

        /// <summary>
        /// Attack logic when triggered by player
        /// Simon
        /// </summary>
        public override void Attack()
        {
            
            if (RefireRate >= 0.5f)
            {
                RefireRate = 0f;
                GameWorld.Instance.Sounds[Sound.PlayerShoot].Play();
                GameWorld.Instance.SpawnObject(ProjectilePool.Instance.GetObject(AttackType.Egg, Player.Instance.Position));
            }

        }

        #endregion
    }
}
