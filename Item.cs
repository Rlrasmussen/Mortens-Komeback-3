using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mortens_Komeback_3.Collider;

namespace Mortens_Komeback_3
{
    public class Item : GameObject, ICollidable
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructor
        public Item(Enum type, Vector2 spawnPos) : base(type, spawnPos)
        {
        }

        public void OnCollision(ICollidable other)
        {
            //throw new NotImplementedException();
        }

        #endregion

        #region Method

        #endregion
    }
}
