using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mortens_Komeback_3.Collider;

namespace Mortens_Komeback_3.Menu
{
    public class Button : ICollidable
    {
        #region Fields
        public bool isPRessed;
        public Button action;
        #endregion

        #region Properties
        public Enum Type => throw new NotImplementedException();

        public Rectangle CollisionBox => throw new NotImplementedException();

        public Vector2 Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Texture2D Sprite => throw new NotImplementedException();
        #endregion

        #region Constructor

        #endregion

        #region Method



        public void OnCollision(ICollidable other)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
