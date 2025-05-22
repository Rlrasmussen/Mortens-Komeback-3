using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3.State
{

    public class ChaseState : IState<Enemy>
    {
        #region Fields

        private Enemy parent;

        #endregion

        #region Properties



        #endregion

        #region Constructor



        #endregion

        #region Method


        public void Enter(Enemy parent)
        {

            this.parent = parent;
            parent.State = this;

        }


        public void Execute()
        {
            
            Vector2 direction = Player.Instance.Position - parent.Position;
            direction.Normalize();
            parent.Position += direction * parent.Speed * GameWorld.Instance.DeltaTime;

        }


        public void Exit()
        {

            parent.State = null;

        }

        #endregion

    }

}
