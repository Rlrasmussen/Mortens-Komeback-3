using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Mortens_Komeback_3.State
{
    public class PatrolState : IState<Enemy>
    {
        #region Fields

        private Enemy parent;
        private Queue<Vector2> waypoints = new Queue<Vector2>();
        private Vector2 target;

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

            if (parent.Destinations.Count > 0)
            {
                foreach (Tile tile in parent.Destinations)
                {
                    waypoints.Enqueue(tile.Position);
                }
                target = waypoints.Dequeue();
                parent.Destinations.Clear();
            }

            if (target != Vector2.Zero)
            {
                Vector2 direction = target - parent.Position;
                direction.Normalize();
                parent.Position += direction * parent.Speed * GameWorld.Instance.DeltaTime;
            }

            if (Vector2.Distance(parent.Position, target) < 10 && waypoints.Count > 0)
                target = waypoints.Dequeue();
            else if (waypoints.Count == 0 && parent.PauseAStar && parent.Destinations.Count == 0)
                parent.PauseAStar = false;
            

            if (Vector2.Distance(parent.Position, Player.Instance.Position) < 200)
                Exit();

        }


        public void Exit()
        {

            ChaseState chasePlayer = new ChaseState();
            chasePlayer.Enter(parent);

        }

        #endregion
    }
}
