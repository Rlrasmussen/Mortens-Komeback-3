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

        /// <summary>
        /// Handles starting logic of the State
        /// Simon
        /// </summary>
        /// <param name="parent">Object that owns the State/subject of its effects</param>
        public void Enter(Enemy parent)
        {

            if (this.parent == null)
                this.parent = parent;
            this.parent.State = this;
            this.parent.IgnoreState = true;
            this.parent.Direction = direction;

        }

        /// <summary>
        /// Movement and despawn logic
        /// Simon
        /// </summary>
        public void Execute()
        {

            parent.IgnoreState = false;

            duration += GameWorld.Instance.DeltaTime;

            parent.Position += direction * (parent.Speed * 1.7f) * GameWorld.Instance.DeltaTime;

            if ((
                parent.InRoom != null && (
                parent.Position.Y >= parent.InRoom.Position.Y + (parent.InRoom.Sprite.Height * 0.75f) || 
                parent.Position.Y <= parent.InRoom.Position.Y - (parent.InRoom.Sprite.Height * 0.75f) || 
                parent.Position.X >= parent.InRoom.Position.X + (parent.InRoom.Sprite.Width * 0.75f) || 
                parent.Position.X <= parent.InRoom.Position.X - (parent.InRoom.Sprite.Width * 0.75f))
                ) || 
                duration >= 9.5f
                )
            {
                Exit();
            }

        }

        /// <summary>
        /// Despawn logic
        /// Simon
        /// </summary>
        public void Exit()
        {

            parent.IsAlive = false;
            EnemyPool.Instance.ReleaseObject(parent);

        }

        #endregion
    }
}
