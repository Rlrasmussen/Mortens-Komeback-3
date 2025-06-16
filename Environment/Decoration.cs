using Microsoft.Xna.Framework;
using System;

namespace Mortens_Komeback_3.Environment
{
    public class Decoration : GameObject
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructor
        public Decoration(Enum type, Vector2 spawnPos, float rotation) : base(type, spawnPos)
        {
            Rotation = rotation;
        }

        #endregion

        #region Method
        
        #endregion
    }
}
