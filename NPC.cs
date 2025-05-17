using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3
{
    public class NPC : GameObject
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructor
        public NPC(Enum type, Vector2 spawnPos) : base(type, spawnPos)
        {
            layer = 0.6f;
        }

        #endregion

        #region Method

        #endregion
    }
}
