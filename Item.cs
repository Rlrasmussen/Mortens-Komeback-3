using Microsoft.Xna.Framework;
using Mortens_Komeback_3.Collider;
using System;

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
            layer = 0.6f;
        }


        #endregion

        #region Method


        public void OnCollision(ICollidable other)
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
