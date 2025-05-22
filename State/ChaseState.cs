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

        public IState<Enemy> PreviousState { get; set; }

        #endregion

        #region Constructor



        #endregion

        #region Method


        public void Enter(Enemy parent)
        {

            if (this.parent == null)
                this.parent = parent;
            this.parent.State = this;

        }


        public void Execute()
        {

            Vector2 direction = Player.Instance.Position - parent.Position;
            direction.Normalize();
            parent.Direction = direction;
            parent.Position += direction * parent.Speed * GameWorld.Instance.DeltaTime;

            if (Vector2.Distance(parent.Position, Player.Instance.Position) > 300)
                Exit();

        }


        public void Exit()
        {

            parent.PauseAStar = false;
            PreviousState.Enter(parent);

        }

        #endregion

    }

}
