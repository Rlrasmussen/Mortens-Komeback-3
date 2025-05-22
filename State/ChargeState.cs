using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Mortens_Komeback_3.Factory;

namespace Mortens_Komeback_3.State
{
    public class ChargeState : IState<Enemy>
    {

        #region Fields

        private Enemy parent;
        private Vector2 direction;
        private float duration;

        #endregion
        #region Properties



        #endregion
        #region Constructor

        public ChargeState(Vector2 direction)
        {

            this.direction = direction;

        }

        #endregion
        #region Methods


        public void Enter(Enemy parent)
        {

            if (this.parent == null)
                this.parent = parent;
            this.parent.State = this;

        }


        public void Execute()
        {

            duration += GameWorld.Instance.DeltaTime;

            parent.Position += direction * (parent.Speed * 1.5f) * GameWorld.Instance.DeltaTime;

            if ((parent.InRoom != null && (parent.Position.Y >= parent.InRoom.Position.Y + 1000 || parent.Position.Y <= parent.InRoom.Position.Y - 1000 || parent.Position.X >= parent.InRoom.Position.X + 1500 || parent.Position.X <= parent.InRoom.Position.X - 1500)) || duration >= 15f)
            {
                Exit();
            }

        }


        public void Exit()
        {

            parent.IgnoreState = false;
            parent.IsAlive = false;
            EnemyPool.Instance.ReleaseObject(parent);

        }

        #endregion
    }
}
