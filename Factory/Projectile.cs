using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mortens_Komeback_3.Collider;

namespace Mortens_Komeback_3.Factory
{
    public class Projectile : GameObject, ICollidable
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructor
        public Projectile(Enum type, Vector2 spawnPos) : base(type, spawnPos)
        {
            this.Damage = 2;
        }



        #endregion

        #region Method
        public void OnCollision(ICollidable other)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
