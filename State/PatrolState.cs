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
        private List<Vector2> patrolPath;
        private ChaseState chasePlayer = new ChaseState();

        #endregion

        #region Properties

        #endregion

        #region Constructor

        public PatrolState()
        {

            chasePlayer.PreviousState = this;

        }


        public PatrolState(List<Vector2> waypoints)
        {

            chasePlayer.PreviousState = this;
            patrolPath = waypoints;

        }

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

            parent.IgnoreState = false;

            if (parent.Destinations.Count > 0 && patrolPath == null)
            {
                foreach (Tile tile in parent.Destinations)
                {
                    waypoints.Enqueue(tile.Position);
                }
                target = waypoints.Dequeue();
                parent.Destinations.Clear();
            }

            if (Vector2.Distance(parent.Position, target) < 15 && waypoints.Count > 0)
                target = waypoints.Dequeue();
            else if (waypoints.Count == 0 && parent.PauseAStar && parent.Destinations.Count == 0)
            {
                if (patrolPath == null)
                    parent.PauseAStar = false;
                else
                    foreach (Vector2 waypoint in patrolPath)
                        waypoints.Enqueue(waypoint);
            }

            if (target != Vector2.Zero && Vector2.Distance(parent.Position, Player.Instance.Position) > 15)
            {
                Vector2 direction = target - parent.Position;
                direction.Normalize();
                parent.Direction = direction;
                parent.Position += direction * parent.Speed * GameWorld.Instance.DeltaTime;
            }

            if (Vector2.Distance(parent.Position, Player.Instance.Position) < 200)
                Exit();

        }


        public void Exit()
        {

            waypoints.Clear();
            chasePlayer.Enter(parent);

        }

        #endregion
    }
}
