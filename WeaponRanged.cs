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

        #endregion

        #region Constructor
        public WeaponRanged(WeaponType type, Vector2 spawnPos) : base(type, spawnPos)
        {
            damage = 2;
        }

        #endregion

        #region Method


        public override void Update(GameTime gameTime)
        {

            refireRate += GameWorld.Instance.DeltaTime;

            base.Update(gameTime);

        }


        public override void Attack()
        {
            
            if (refireRate > 1)
            {
                refireRate = 0f;
                GameWorld.Instance.Sounds[Sound.PlayerShoot].Play();
                GameWorld.Instance.SpawnObject(ProjectilePool.Instance.Create());
            }

        }

        #endregion
    }
}
