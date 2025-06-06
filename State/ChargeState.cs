﻿using Microsoft.Xna.Framework;
using Mortens_Komeback_3.Factory;

namespace Mortens_Komeback_3.State
{
    public class ChargeState : IState<Enemy>
    {

        #region Fields

        private Enemy parent;
        private Vector2 direction;
        private float duration;
        private Enemy spawner;

        #endregion
        #region Properties

        public bool OverridesPathfinding { get; set; } = true;

        #endregion
        #region Constructor

        public ChargeState(Vector2 direction, Enemy spawner)
        {

            this.direction = direction;
            this.spawner = spawner;

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
                parent.Position.Y >= parent.InRoom.Position.Y + (parent.InRoom.Sprite.Height * 0.55f) ||
                parent.Position.Y <= parent.InRoom.Position.Y - (parent.InRoom.Sprite.Height * 0.55f) ||
                parent.Position.X >= parent.InRoom.Position.X + (parent.InRoom.Sprite.Width * 0.55f) ||
                parent.Position.X <= parent.InRoom.Position.X - (parent.InRoom.Sprite.Width * 0.55f))
                ) ||
                duration >= 9.5f ||
                !spawner.IsAlive
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
